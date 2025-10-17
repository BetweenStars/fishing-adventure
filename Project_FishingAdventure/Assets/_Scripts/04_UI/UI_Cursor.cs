using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class UI_Cursor : UI_BaseCanvasGroup
{
    private Image cursorImage;
    private RectTransform rectTransform;

    private PlayerInteract playerInteract;

    [SerializeField] private List<Sprite> cursorSprites;

    protected override void Awake()
    {
        base.Awake();

        canvasGroup = GetComponent<CanvasGroup>();
        cursorImage = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();

        ShowUI(false, false);

        cursorImage.sprite = cursorSprites[0];
    }

    private void OnEnable()
    {
        Cursor.visible = false;

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
        if (canvasGroup.alpha > 0f)
        {
            Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
            rectTransform.position = mouseScreenPosition;
        }
    }

    private void LateUpdate()
    {
        if(!Cursor.visible){ Cursor.visible = false; }
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
    }
}
