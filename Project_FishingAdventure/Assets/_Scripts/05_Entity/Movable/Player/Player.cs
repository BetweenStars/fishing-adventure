using UnityEngine;

public class Player : MovableEntity
{
    public PlayerDef_SO playerDef => movableEntityDef as PlayerDef_SO;

    public PlayerMovement playerMovement { get; private set; }
    public PlayerInteract playerInteract { get; private set; }
    public PlayerStateManager playerStateManager { get; private set; }

    public BoxCollider2D playerCollider{ get; private set; }

    void Awake()
    {

        playerMovement = GetComponent<PlayerMovement>();
        playerMovement?.Initialize(this);

        playerInteract = GetComponent<PlayerInteract>();
        playerInteract?.Initialize(this);

        playerStateManager = GetComponentInChildren<PlayerStateManager>();

        playerCollider = GetComponent<BoxCollider2D>();
    }

    public void GetOnShip(Ship ship)
    {
        playerStateManager.ChangeState(new PlayerRidingState());

        playerMovement.enabled = false;
        playerMovement.rb.bodyType = RigidbodyType2D.Kinematic;
        playerCollider.enabled = false;

        transform.position = ship.playerAnchor.transform.position;

        transform.parent = ship.transform;
    }
    public void GetOffShip()
    {
        playerStateManager.ChangeState(new PlayerIdleState());

        playerMovement.enabled = true;
        playerMovement.rb.bodyType = RigidbodyType2D.Dynamic;
        playerCollider.enabled = true;

        transform.parent = WorldManager.Instance.entityParent;
    }
}
