using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class UI_InteractCursor : MonoBehaviour
{
    private Image cursorImage;
    private RectTransform rectTransform;

    private PlayerInteract playerInteract;

    [SerializeField] private List<Sprite> cursorSprites;

    private void Awake()
    {
        cursorImage = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();

        cursorImage.color = ImageUtils.GetTrasparencyColor(cursorImage.color, 0);
    }

    private void OnEnable()
    {
        PlayerManager.OnPlayerReady += HandlePlayerReady;

        if (PlayerManager.player != null && PlayerManager.player.playerInteract != null)
        {
            HandlePlayerReady();
        }
    }
    private void OnDisable()
    {
        PlayerManager.OnPlayerReady -= HandlePlayerReady;

        if (playerInteract != null)
        {
            playerInteract.onInteractableChanged -= UpdateInteractUISprite;
        }
    }
    private void OnDestroy() { OnDisable(); }

    private void Update()
    {
        if (cursorImage.color.a != 0)
        {
            Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
            rectTransform.position = mouseScreenPosition;
        }
    }

    private void HandlePlayerReady()
    {
        playerInteract = PlayerManager.player.playerInteract;
        playerInteract.onInteractableChanged += UpdateInteractUISprite;

        PlayerManager.OnPlayerReady -= HandlePlayerReady;
    }

    private void UpdateInteractUISprite(InteractType type)
    {
        cursorImage.sprite = cursorSprites[(int)type];

        if (type == InteractType.NONE) { cursorImage.color = ImageUtils.GetTrasparencyColor(cursorImage.color, 0); }
        else { cursorImage.color = ImageUtils.GetTrasparencyColor(cursorImage.color, 1); }
    }
}
