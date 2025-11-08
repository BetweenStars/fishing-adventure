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

        if (rb.linearVelocity.magnitude != 0) { player.playerAnimationController.animator.SetBool("IsWalking", true); }
        else { player.playerAnimationController.animator.SetBool("IsWalking", false); }

        player.playerAnimationController.animator.SetFloat("xDir", rb.linearVelocityX);
        player.playerAnimationController.animator.SetFloat("yDir", rb.linearVelocityY);
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
