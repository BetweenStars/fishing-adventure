using UnityEngine;

public class Player : MovableEntity
{
    public PlayerDef_SO playerDef => movableEntityDef as PlayerDef_SO;

    public PlayerMovement playerMovement { get; private set; }
    public PlayerInteract playerInteract { get; private set; }
    public PlayerStateManager playerStateManager { get; private set; }

    public PlayerFishInventory playerFishInventory { get; private set; }
    public PlayerMoney playerMoney { get; private set; }
    public PlayerInventory playerInventory{ get; private set; }

    public BoxCollider2D playerCollider { get; private set; }

    [field: SerializeField] public Transform rodTipTransform { get; private set; }

    void Awake()
    {

        playerMovement = GetComponentInChildren<PlayerMovement>();
        playerMovement?.Initialize(this);

        playerInteract = GetComponentInChildren<PlayerInteract>();
        playerInteract?.Initialize(this);

        playerStateManager = GetComponentInChildren<PlayerStateManager>();

        playerFishInventory = new();
        playerMoney = new(SaveLoadManager.Instance.gameSaveData.money);
        playerInventory = new();

        playerCollider = GetComponent<BoxCollider2D>();
    }

    public void GetOnShip(Ship ship)
    {
        playerCollider.enabled = false;
        playerMovement.rb.bodyType = RigidbodyType2D.Kinematic;
        playerMovement.rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
        playerMovement.enabled = false;

        SetPlayerPosition(ship.playerAnchor.transform.position);

        transform.parent = ship.transform;
    }
    public void GetOffShip()
    {
        playerMovement.enabled = true;
        playerMovement.rb.bodyType = RigidbodyType2D.Dynamic;
        playerMovement.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        playerCollider.enabled = true;

        SetPlayerPosition(playerInteract.interactedPos);

        transform.parent = WorldManager.Instance.entityParent;
    }

    public void SetPlayerPosition(Vector2 pos)
    {
        transform.position = pos;
    }
}
