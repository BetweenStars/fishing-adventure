using UnityEngine;
using UnityEngine.InputSystem;

public class ShipMovement : MonoBehaviour
{
    public Ship ship;
    public Rigidbody2D shiprb;

    public float currentSpeed = 0f;

    private Vector2 currentInputVector;

    private void FixedUpdate()
    {
        shiprb.linearVelocity = currentInputVector * currentSpeed;
    }

    private void SetInpuActions()
    {
        if (InputManager.Instance.movementAction == null)
        {
            Debug.LogError("[Ship Movement] Movement Action Reference is null...");
            return;
        }

        InputManager.Instance.movementAction.action.performed += OnMovementInput;
        InputManager.Instance.movementAction.action.canceled += OnMovementInput;
    }

    private void OnMovementInput(InputAction.CallbackContext context)
    {
        currentInputVector = context.ReadValue<Vector2>().normalized;
    }

    public void Initialize(Ship ship)
    {
        this.ship = ship;

        shiprb = GetComponent<Rigidbody2D>();
        shiprb.gravityScale = 0f;
        shiprb.freezeRotation = true;

        currentSpeed = ship.shipDef.baseSpeed;

        SetInpuActions();
    }
}