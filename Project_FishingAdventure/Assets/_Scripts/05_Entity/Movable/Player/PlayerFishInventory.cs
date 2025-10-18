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
    
    public void SellFish()
    {
        if (fishList != null && fishList.Count > 0)
        {
            Debug.Log($"{FishDataBase.Instance.GetFishDefByID(fishList[0].fishID).entityID} has sold! You got {fishList[0].price}$");
            PlayerManager.player.playerMoney.AddMoney(fishList[0].price);
            fishList.RemoveAt(0);
        }
        else
        {
            Debug.Log("You have no fish");
        }
    }
}