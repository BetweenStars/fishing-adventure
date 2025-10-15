using System.Collections;
using UnityEngine;

public class PlayerFishingState : BasePlayerState
{
    public PlayerFishingState() { state = PlayerState.FISHING; }

    private Coroutine fishingCoroutine;

    public override void EnterState()
    {
        base.EnterState();

        WorldManager.ship.DeactivateControl();

        fishingCoroutine = stateManager.StartStateCoroutine(FishingCoroutine());
    }

    public override void ExitState()
    {
        base.ExitState();

        stateManager.StopStateCoroutine(fishingCoroutine);
    }

    private IEnumerator FishingCoroutine()
    {
        Debug.Log("Start fishing");
        yield return new WaitForSeconds(1.5f);
        Debug.Log("End Fishing");
        stateManager.ChangeState(new PlayerRidingState());
    }
}