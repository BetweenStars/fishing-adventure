using UnityEngine;

public enum InteractType
{
    DOOR,
    SHIP,
    NPC
}

public interface IInteractable
{
    InteractType interactType { get; }
    public void Interact();
}
