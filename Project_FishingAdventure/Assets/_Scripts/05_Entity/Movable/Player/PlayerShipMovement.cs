using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShipMovement : MonoBehaviour
{
    private Player player;
    private Ship ship;
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

    public void SetShip(Ship ship)
    {
        this.ship = ship;
        currentSpeed = ship.shipDef.baseSpeed;
    }

    public void InitShip()
    {
        ship.transform.parent = WorldManager.Instance.entityParent;
        ship = null;
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

        SetupInputActions();
    }
}