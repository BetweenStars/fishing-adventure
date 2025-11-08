using UnityEngine;

public class PlayerOnLandState : BasePlayerState
{
    public PlayerOnLandState() { state = PlayerState.ONLAND; }

    public override void EnterState()
    {
        base.EnterState();

        PlayerManager.player.GetOffShip();
    }

    public override void ExitState()
    {
        base.ExitState();

        PlayerManager.player.playerAnimationController.animator.SetBool("IsWalking", false);
    }
}