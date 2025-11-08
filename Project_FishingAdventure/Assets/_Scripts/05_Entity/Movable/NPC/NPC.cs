using UnityEngine;

public class NPC : MovableEntity,IInteractable
{
    public NPCDef_SO npcDef => movableEntityDef as NPCDef_SO;

    public InteractType interactType => InteractType.NPC;

    public virtual void Interact() { }
    
    public virtual void PrintDialog(){}
}
