using UnityEngine;

public class FishingInteractable : IInteractable
{
    private static FishingInteractable instance;
    public static FishingInteractable Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new FishingInteractable();
            }
            return instance;
        }
    }

    private FishingInteractable() { }
    
    public InteractType interactType => InteractType.FISHING;

    public void Interact()
    {
        PlayerManager.player.playerStateManager.ChangeState(new PlayerFishingState());
    }
}
