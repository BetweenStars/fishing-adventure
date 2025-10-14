using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [Header("Interact Settings")]
    public float interactDistance = 1.0f;
    private IInteractable interactable = null;

    private void Update()
    {
        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();

        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        Vector2 rayOrigin = new Vector2(mouseWorldPosition.x, mouseWorldPosition.y);

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.zero);

        if (hit.collider != null)
        {
            var col = hit.collider.GetComponent<IInteractable>();

            if (col != null && Vector2.Distance(hit.collider.transform.position, transform.position) <= interactDistance)
            {
                interactable = col;
            }
            else
            {
                interactable = null;
            }
        }
        else
        {
            interactable = null;
        }
    }

    public bool HasInteractable()
    {
        return interactable != null;
    }

    private void OnEnable() { InputManager.Instance.interactAction.action.started += OnClickInteractable; }
    private void OnDisable() { InputManager.Instance.interactAction.action.started -= OnClickInteractable; }
    private void OnDestroy() { InputManager.Instance.interactAction.action.started -= OnClickInteractable; }

    private void OnClickInteractable(InputAction.CallbackContext context)
    {
        if (interactable == null) return;

        interactable.Interact();
    }
}
