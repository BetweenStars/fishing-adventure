using UnityEngine;

public class Player : MovableEntity
{
    public PlayerDef_SO playerDef => movableEntityDef as PlayerDef_SO;

    public PlayerMovement playerMovement { get; private set; }
    public PlayerShipMovement playerShipMovement { get; private set; }
    public PlayerInteract playerInteract { get; private set; }
    public PlayerStateManager playerStateManager { get; private set; }

    void Awake()
    {

        playerMovement = GetComponent<PlayerMovement>();
        playerMovement?.Initialize(this);

        playerShipMovement = GetComponent<PlayerShipMovement>();
        playerShipMovement.Initialize(this);
        playerShipMovement.enabled = false;

        playerInteract = GetComponent<PlayerInteract>();

        playerStateManager = GetComponentInChildren<PlayerStateManager>();
    }

    public void GetOnShip(Ship ship)
    {
        playerStateManager.ChangeState(new PlayerRidingState());

        playerMovement.enabled = false;
        playerShipMovement.enabled = true;

        transform.position = ship.playerAnchor.transform.position;
        ship.transform.parent = transform;

        playerShipMovement.SetShip(ship);
    }
    public void GetOutShip()
    {
        playerStateManager.ChangeState(new PlayerIdleState());

        playerShipMovement.InitShip();

        playerMovement.enabled = true;
        playerShipMovement.enabled = false;
    }
}
