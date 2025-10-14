using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Timeline;

public class WalkableLand : MonoBehaviour, IInteractable
{
    public InteractType interactType { get; } = InteractType.LAND;

    public void Interact()
    {
        PlayerManager.player.GetOffShip();
        WorldManager.ship.DeactivateControl();
    }
}
