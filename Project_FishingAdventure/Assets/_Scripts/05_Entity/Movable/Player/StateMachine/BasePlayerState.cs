using UnityEngine;

public abstract class BasePlayerState
{
    protected PlayerStateManager stateManager;
    public PlayerState state;

    public virtual void EnterState() { }
    public virtual void UpdateState() { }
    public virtual void ExitState() { }

    public virtual void Initialize(PlayerStateManager stateManager) { this.stateManager = stateManager; }
}
