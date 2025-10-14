using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceProviders.Simulation;

public class Ship : MovableEntity, IInteractable
{
    public ShipDef_SO shipDef => movableEntityDef as ShipDef_SO;
    public Transform playerAnchor;

    public InteractType interactType => InteractType.SHIP;

    public virtual void Interact()
    {
        PlayerManager.player.GetOnShip(this);
    }
}