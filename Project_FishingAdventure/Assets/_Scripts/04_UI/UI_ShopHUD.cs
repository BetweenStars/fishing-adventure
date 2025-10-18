using UnityEngine;

public class UI_ShopHUD:UI_BaseCanvasGroup
{
    public FishingRodDef_SO tempRodDef;
    public void OnSellBtnClicked()
    {
        PlayerManager.player.playerFishInventory.SellFish();
    }
    public void OnButBtnClicked()
    {
        if (PlayerManager.player.playerMoney.TrySpendMoney(tempRodDef.price))
        {
            PlayerManager.player.playerInventory.fishingRods.Add(tempRodDef);
            Debug.Log($"You bought {tempRodDef.entityName}! You spent {tempRodDef.price}$");
        }
        else
        {
            Debug.Log("You don't have enough money");
        }
    }
}
