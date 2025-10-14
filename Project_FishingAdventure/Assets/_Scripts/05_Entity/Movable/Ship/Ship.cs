using UnityEngine;

public class Ship : MovableEntity, IInteractable
{
    public ShipDef_SO shipDef => movableEntityDef as ShipDef_SO;
    public Transform playerAnchor;

    public InteractType interactType => InteractType.SHIP;

    public virtual void Interact()
    {
        PlayerManager.player.playerStateManager.ChangeState(new PlayerRidingState());
    }
}