using UnityEngine;

public class PlayerRidingState : BasePlayerState
{
    public PlayerRidingState() { state = PlayerState.RIDING; }

    public override void EnterState()
    {
        base.EnterState();

        PlayerManager.player.playerMovement.enabled = false;
    }

    public override void ExitState()
    {
        base.ExitState();

        PlayerManager.player.playerMovement.enabled = true;
    }
}