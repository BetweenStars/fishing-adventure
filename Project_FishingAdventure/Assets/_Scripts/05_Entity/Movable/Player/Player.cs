using UnityEngine;

public class Player : MovableEntity
{
    public PlayerDef_SO playerDef => movableEntityDef as PlayerDef_SO;

    public PlayerMovement playerMovement { get; private set; }
    public PlayerInteract playerInteract { get; private set; }
    public PlayerStateManager playerStateManager { get; private set; }

    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerMovement?.Initialize(this);

        playerInteract = GetComponent<PlayerInteract>();

        playerStateManager = GetComponentInChildren<PlayerStateManager>();
    }
}
