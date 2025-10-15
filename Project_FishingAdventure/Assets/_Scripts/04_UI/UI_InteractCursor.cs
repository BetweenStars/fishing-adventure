using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class UI_InteractCursor : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private Image cursorImage;
    private RectTransform rectTransform;

    private PlayerInteract playerInteract;

    [SerializeField] private List<Sprite> cursorSprites;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        cursorImage = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    private void OnEnable()
    {
        PlayerManager.OnPlayerReady += HandlePlayerReady;

        if (PlayerManager.IsPlayerReady)
        {
            HandlePlayerReady();
        }
    }
    private void OnDisable()
    {
        PlayerManager.OnPlayerReady -= HandlePlayerReady;

        if (playerInteract != null)
        {
            playerInteract.OnInteractableChanged -= UpdateInteractUISprite;
        }
    }
    private void OnDestroy() { OnDisable(); }

    private void Update()
    {
        if (canvasGroup.alpha != 0f)
        {
            Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
            rectTransform.position = mouseScreenPosition;
        }
    }

    private void HandlePlayerReady()
    {
        playerInteract = PlayerManager.player.playerInteract;
        playerInteract.OnInteractableChanged += UpdateInteractUISprite;

        PlayerManager.OnPlayerReady -= HandlePlayerReady;
    }

    private void UpdateInteractUISprite(InteractType type)
    {
        cursorImage.sprite = cursorSprites[(int)type];

        if (type == InteractType.NONE) { canvasGroup.alpha = 0f; }
        else { canvasGroup.alpha = 1f; ; }
    }
}
