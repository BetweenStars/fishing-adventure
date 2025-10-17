using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class UI_InventoryHUD : UI_BaseCanvasGroup
{
    private UI_InventorySlotItem[] itemSlots;

    protected override void Awake()
    {
        base.Awake();

        itemSlots = GetComponentsInChildren<UI_InventorySlotItem>();

        HideUI();
    }

    private void OnEnable() { InputManager.Instance.inventoryAction.action.started += HandleInventoryInputAction; }
    private void OnDisable() { InputManager.Instance.inventoryAction.action.started -= HandleInventoryInputAction; }
    private void OnDestroy() { InputManager.Instance.inventoryAction.action.started -= HandleInventoryInputAction; }

    protected override void ShowUI(bool interactable = true, bool blocksRaycasts = true)
    {
        base.ShowUI(interactable, blocksRaycasts);

        List<FishData> fishDatas = new();
        fishDatas = PlayerManager.player.playerFishInventory.fishList.ToList();

        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (i < fishDatas.Count)
            {
                itemSlots[i].SetItemImage(fishDatas[i].fishDef.sprite);
            }
            else
            {
                itemSlots[i].SetItemImage(null);
            }
        }
    }

    private void HandleInventoryInputAction(InputAction.CallbackContext context)
    {
        if(Time.timeScale==0f){ return; }
        //나중에 플레이어 상태에 따라 못켜게 하기
        if (canvasGroup.alpha == 0f) { ShowUI(); }
        else{ HideUI(); }
    }
}
