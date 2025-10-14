using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovingState : BasePlayerState
{
    public PlayerMovingState() { state = PlayerState.MOVING; }

    public override void EnterState()
    {
        base.EnterState();

        InputManager.Instance.movementAction.action.canceled += OnStop;
    }

    public override void ExitState()
    {
        base.ExitState();

        InputManager.Instance.movementAction.action.canceled -= OnStop;
    }

    private void OnDestroy()
    {
        InputManager.Instance.movementAction.action.canceled -= OnStop;
    }

    private void OnStop(InputAction.CallbackContext context)
    {
        stateManager.ChangeState(new PlayerIdleState());
    }
}