using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public enum PlayerState { ONLAND, RIDING, INTERACT, FISHING }

public class PlayerStateManager : MonoBehaviour
{
    public BasePlayerState currentState;
    public PlayerState state;
    public bool isRiding { get; private set; } = false;

    private void Start()
    {
        ChangeState(new PlayerOnLandState());
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

    public Coroutine StartStateCoroutine(IEnumerator coroutine)
    {
        return StartCoroutine(coroutine);
    }

    public void StopStateCoroutine(Coroutine coroutine)
    {
        if (coroutine == null) return;

        StopCoroutine(coroutine);
    }
}
