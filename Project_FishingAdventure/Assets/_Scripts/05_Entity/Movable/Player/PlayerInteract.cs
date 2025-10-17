using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    private Player player;

    public event Action<InteractType> OnInteractableChanged;

    [Header("Interact Settings")]
    public float interactDistance = 1.0f;
    private IInteractable interactable = null;
    [SerializeField]
    private LayerMask interactableLayerMask;
    public Vector2 interactedPos { get; private set; } = Vector2.zero;

    private void Update()
    {
        FindInteractableOnMousePos();
    }

    public bool HasInteractable()
    {
        return interactable != null;
    }

    private void FindInteractableOnMousePos()
    {
        IInteractable lastInteractable = interactable;

        interactable = null;

        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);

        Vector2 rayOrigin = new Vector2(mouseWorldPosition.x, mouseWorldPosition.y);

        Collider2D hit = Physics2D.OverlapPoint(rayOrigin, interactableLayerMask);

        if (hit != null)
        {
            var col = hit.GetComponentInParent<IInteractable>();

            if (col != null)
            {
                interactable = GetValidInteractable(col);
            }
        }

        if (!HasInteractable() && PlayerManager.player.playerStateManager.currentState.state == PlayerState.RIDING)
        {
            interactable = FishingInteractable.Instance;
        }

        if (Vector2.Distance(mouseWorldPosition, transform.position) > interactDistance)
        {
            interactable = null;
        }

        InteractType newType = (interactable != null)
                ? interactable.interactType
                : InteractType.NONE;

        if (lastInteractable != interactable) { OnInteractableChanged?.Invoke(newType); }
    }

    private IInteractable GetValidInteractable(IInteractable interactable)
    {
        PlayerState currentState = PlayerManager.player.playerStateManager.currentState.state;

        if (currentState == PlayerState.ONLAND)
        {
            if (interactable.interactType != InteractType.LAND)
            {
                return interactable;
            }
        }
        else if (currentState == PlayerState.RIDING)
        {
            if (interactable.interactType != InteractType.SHIP)
            {
                return interactable;
            }
        }

        return null;
    }

    public void Initialize(Player player)
    {
        this.player = player;
    }

    private void OnEnable() { InputManager.Instance.interactAction.action.started += HandleInteractInputAction; }
    private void OnDisable() { InputManager.Instance.interactAction.action.started -= HandleInteractInputAction; }
    private void OnDestroy() { InputManager.Instance.interactAction.action.started -= HandleInteractInputAction; }

    private void HandleInteractInputAction(InputAction.CallbackContext context)
    {
        if (interactable == null) return;

        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();

        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);

        interactedPos = mouseWorldPosition;

        interactable.Interact();
    }
}
