using System.Collections;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;

public class Bait : StaticEntity
{
    public BaitDef_SO baitDef => staticEntityDef as BaitDef_SO;

    [SerializeField]private SpriteRenderer spriteRenderer;

    private Coroutine moveCoroutine;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Hide();
    }

    private IEnumerator MoveCoroutine(Vector3 targetPos)
    {
        transform.position = targetPos;

        yield return null;
    }

    public void Throw(Vector3 startPos, Vector3 targetPos)
    {
        transform.position = startPos;

        if (moveCoroutine == null)
        {
            moveCoroutine = StartCoroutine(MoveCoroutine(targetPos));
        }

        Show();
    }
    public void Recall()
    {
        if (moveCoroutine != null) { StopCoroutine(moveCoroutine); }

        Hide();
    }

    private void Show()
    {
        spriteRenderer.color = ImageUtils.GetTransparencyColor(spriteRenderer.color, 1);
    }
    private void Hide()
    {
        spriteRenderer.color = ImageUtils.GetTransparencyColor(spriteRenderer.color, 0);
    }
}