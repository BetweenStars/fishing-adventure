using UnityEngine;

public enum InteractType
{
    DOOR,
    SHIP,
    NPC,
    LAND
}

public interface IInteractable
{
    InteractType interactType { get; }
    public void Interact();
}
