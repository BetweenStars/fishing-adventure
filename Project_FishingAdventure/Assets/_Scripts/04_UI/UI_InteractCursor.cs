using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class UI_InteractCursor : MonoBehaviour
{
    private Image cursorImage;
    private RectTransform rectTransform;

    private void Awake()
    {
        cursorImage = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();

        cursorImage.color = ImageUtils.GetTrasparencyColor(cursorImage, 0);
    }

    private void Update()
    {
        if (PlayerManager.player.playerInteract.HasInteractable())
        {
            cursorImage.color = ImageUtils.GetTrasparencyColor(cursorImage, 1);

            Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
            rectTransform.position = mouseScreenPosition;
        }
        else
        {
            cursorImage.color = ImageUtils.GetTrasparencyColor(cursorImage, 0);
        }
    }
}
