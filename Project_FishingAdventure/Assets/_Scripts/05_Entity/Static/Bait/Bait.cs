using System.Collections;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;

public class Bait : StaticEntity
{
    public BaitDef_SO baitDef => staticEntityDef as BaitDef_SO;

    public float duration = 0.5f;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Coroutine moveCoroutine;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Hide();
    }

    private IEnumerator MoveCoroutine(Vector3 targetPos)
    {
        Vector3 startPos = transform.position;

        float elapsedTime = 0.0f;

        if (duration <= 0)
        {
            transform.position = targetPos;
            yield break;
        }

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(elapsedTime / duration);

            transform.position = Vector3.Lerp(startPos, targetPos, t);

            yield return null;
        }

        transform.position = targetPos;

        moveCoroutine = null;
    }

    public void Throw(Vector3 startPos, Vector3 targetPos)
    {
        transform.position = startPos;

        if (moveCoroutine != null) { StopCoroutine(moveCoroutine); }

        moveCoroutine = StartCoroutine(MoveCoroutine(targetPos));

        Show();
    }
    public IEnumerator Recall()
    {
        if (moveCoroutine != null) { StopCoroutine(moveCoroutine); }

        yield return StartCoroutine(MoveCoroutine(PlayerManager.player.rotTipTransform.position));

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