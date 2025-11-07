using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    private Player player;
    public Rigidbody2D rb;

    public float currentSpeed = 0f;

    private Vector2 currentInputVector;

    private void FixedUpdate()
    {
        rb.linearVelocity = currentInputVector * currentSpeed;
    }

    private void SetInputActions()
    {
        if (InputManager.Instance.movementAction == null)
        {
            Debug.LogError("[Player Movement] Movement Action Reference is null...");
            return;
        }

        InputManager.Instance.movementAction.action.performed += OnMovementInput;
        InputManager.Instance.movementAction.action.canceled += OnMovementInput;
    }

    private void OnMovementInput(InputAction.CallbackContext context)
    {
        currentInputVector = context.ReadValue<Vector2>().normalized;
    }

    public void Initialize(Player player)
    {
        this.player = player;

        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;

        currentSpeed = player.playerDef.baseSpeed;

        SetInputActions();
    }
}
