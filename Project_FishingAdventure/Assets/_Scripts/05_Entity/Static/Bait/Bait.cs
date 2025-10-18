using System.Collections;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;

public class Bait : StaticEntity
{
    public BaitDef_SO baitDef => staticEntityDef as BaitDef_SO;

    public float throwDuration = 0.5f;
    public float baseBobbingSpeed { get; private set; } = 4.0f;
    public float fastBobbingSpeed{ get; private set; } = 12.0f;
    private float bobbingSpeed;
    public float bobbingAmplitude = 0.1f;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Coroutine moveCoroutine;
    private Coroutine bobbingCoroutine;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetBobbingSpeed(baseBobbingSpeed);
        Hide();
    }

    private IEnumerator MoveCoroutine(Vector3 targetPos, float duration)
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

    private IEnumerator BobbingCoroutine()
    {
        Vector3 basePosition = transform.position;
        float elapsedTime = 0f;

        while (true)
        {
            elapsedTime += Time.deltaTime * bobbingSpeed;

            float yOffset = Mathf.Sin(elapsedTime) * bobbingAmplitude;

            transform.position = basePosition + new Vector3(0, yOffset, 0);

            yield return null;
        }
    }

    private IEnumerator ThrowSequence(Vector3 targetPos)
    {
        yield return StartCoroutine(MoveCoroutine(targetPos, throwDuration));

        bobbingCoroutine = StartCoroutine(BobbingCoroutine());
    }

    public void Throw(Vector3 startPos, Vector3 targetPos)
    {
        transform.position = startPos;
        SetBobbingSpeed(baseBobbingSpeed);

        if (moveCoroutine != null) { StopCoroutine(moveCoroutine); moveCoroutine = null; }
        if(bobbingCoroutine!=null){ StopCoroutine(bobbingCoroutine); bobbingCoroutine = null; }

        moveCoroutine = StartCoroutine(ThrowSequence(targetPos));

        Show();
    }

    public IEnumerator Recall()
    {
        if (moveCoroutine != null) { StopCoroutine(moveCoroutine); moveCoroutine = null; }
        if (bobbingCoroutine != null) { StopCoroutine(bobbingCoroutine); bobbingCoroutine = null; }

        yield return StartCoroutine(MoveCoroutine(PlayerManager.player.rodTipTransform.position, throwDuration));

        Hide();
    }

    public void SetBobbingSpeed(float speed)
    {
        bobbingSpeed = speed;
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