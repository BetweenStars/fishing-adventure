using System.Collections.Generic;
using UnityEngine;

public class PlayerFishInventory
{
    public List<FishData> fishList = new();

    public PlayerFishInventory(){ fishList = new(); }
    
    public void AddFish(FishData fishData)
    {
        fishList.Add(fishData);
    }
}