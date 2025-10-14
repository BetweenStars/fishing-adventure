using Unity.VisualScripting;
using UnityEngine;

public enum PlayerState { IDLE, MOVING, RIDING, INTERACT, FISHING }

public class PlayerStateManager : MonoBehaviour
{
    public BasePlayerState currentState;
    public PlayerState state;
    public bool isRiding { get; private set; } = false;

    private void Awake()
    {
        ChangeState(new PlayerIdleState());
    }

    private void Update()
    {
        currentState?.UpdateState();
    }

    public void ChangeState(BasePlayerState newState)
    {
        currentState?.ExitState();

        currentState = newState;
        state = newState.state;
        currentState.Initialize(this);

        currentState?.EnterState();
    }
}
