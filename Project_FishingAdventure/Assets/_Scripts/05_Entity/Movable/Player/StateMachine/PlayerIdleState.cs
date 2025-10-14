using System.Data.Common;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerIdleState : BasePlayerState
{
    public PlayerIdleState() { state = PlayerState.IDLE; }

    public override void EnterState()
    {
        base.EnterState();

        InputManager.Instance.movementAction.action.performed += OnMove;
    }

    public override void ExitState()
    {
        base.ExitState();

        InputManager.Instance.movementAction.action.performed -= OnMove;
    }

    private void OnDestroy()
    {
        InputManager.Instance.movementAction.action.performed -= OnMove;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        Debug.Log("move");
        stateManager.ChangeState(new PlayerMovingState());
    }
}