using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    private Player player;
    private Rigidbody2D rb;

    public float currentSpeed = 0f;

    private Vector2 currentInputVector;

    private void FixedUpdate()
    {
        rb.linearVelocity = currentInputVector * currentSpeed;
    }

    private void SetupInputActions()
    {
        if (InputManager.Instance.movementAction == null)
        {
            Debug.LogError("Movement Action Reference is null...");
            return;
        }

        InputManager.Instance.movementAction.action.performed += OnMovementInputRead;
        InputManager.Instance.movementAction.action.canceled += OnMovementInputRead;
    }

    private void OnMovementInputRead(InputAction.CallbackContext context)
    {
        currentInputVector = context.ReadValue<Vector2>().normalized;
    }

    public void Initialize(Player player)
    {
        this.player = player;

        Setup();
    }

    private void Setup()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;

        currentSpeed = player.playerDef.baseSpeed;

        SetupInputActions();
    }
}
