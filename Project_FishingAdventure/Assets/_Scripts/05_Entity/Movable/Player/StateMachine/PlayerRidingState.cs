using UnityEngine;

public class PlayerRidingState : BasePlayerState
{
    public PlayerRidingState() { state = PlayerState.RIDING; }

    public override void EnterState()
    {
        base.EnterState();

        PlayerManager.player.GetOnShip(WorldManager.ship);
        WorldManager.ship.ActivateControl();
    }

    public override void ExitState()
    {
        base.ExitState();

        WorldManager.ship.DeactivateControl();
    }
}