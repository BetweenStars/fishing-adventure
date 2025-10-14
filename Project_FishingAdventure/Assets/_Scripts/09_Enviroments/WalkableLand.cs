using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Timeline;

public class WalkableLand : MonoBehaviour, IInteractable
{
    public InteractType interactType { get; } = InteractType.LAND;

    public void Interact()
    {
        PlayerManager.player.playerStateManager.ChangeState(new PlayerIdleState());
    }
}
