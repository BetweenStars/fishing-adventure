using System.Collections.Generic;
using UnityEngine;

public class FishingManager : MonoBehaviour
{
    public static FishingManager Instance;

    public List<FishDef_SO> basicFishList;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    public FishDef_SO BaitedFish()
    {
        return basicFishList[Random.Range(0,basicFishList.Count)];
    }
    public void CaughtFish(FishDef_SO fishDef)
    {
        FishData newData = new FishData(fishDef, 10, 10);
        PlayerManager.player.playerFishInventory.AddFish(newData);
    }
}
