using UnityEngine;

public class UI_BaseCanvasGroup : MonoBehaviour
{
    protected CanvasGroup canvasGroup;

    protected virtual void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    protected virtual void ShowUI(bool interactable=true, bool blocksRaycasts=true)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = interactable;
        canvasGroup.blocksRaycasts = blocksRaycasts;
    }
    protected virtual void HideUI()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}