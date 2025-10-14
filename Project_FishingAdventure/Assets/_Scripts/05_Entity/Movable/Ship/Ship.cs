using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceProviders.Simulation;

public class Ship : MovableEntity, IInteractable
{
    public ShipDef_SO shipDef => movableEntityDef as ShipDef_SO;
    public Transform playerAnchor;

    public InteractType interactType => InteractType.SHIP;

    public ShipMovement shipMovement { get; private set; }

    private void Awake()
    {
        shipMovement = GetComponent<ShipMovement>();
        shipMovement.Initialize(this);
        shipMovement.enabled = false;
    }

    public void ActivateControl()
    {
        shipMovement.enabled = true;
        shipMovement.shiprb.bodyType = RigidbodyType2D.Dynamic;
    }
    public void DeactivateControl()
    {
        shipMovement.enabled = false;
        shipMovement.shiprb.bodyType = RigidbodyType2D.Static;
    }

    public virtual void Interact()
    {
        PlayerManager.player.playerStateManager.ChangeState(new PlayerRidingState());
    }
}