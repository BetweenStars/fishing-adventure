using UnityEngine;

public enum InteractType
{
    NONE,
    SHIP,
    LAND,
    FISHING,
    NPC
}

public interface IInteractable
{
    InteractType interactType { get; }
    public void Interact();
}
