using System.Collections.Generic;
using UnityEditor.Purchasing;
using UnityEngine;

public class FishingManager : MonoBehaviour
{
    public static FishingManager Instance;

    public List<FishDef_SO> basicFishList;

    public PixelLineDrawer pixelLineDrawer { get; private set; }

    public Bait bait{ get; private set; }

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

    private void Start()
    {
        pixelLineDrawer = GetComponentInChildren<PixelLineDrawer>();
        pixelLineDrawer.enabled = false;

        bait = GetComponentInChildren<Bait>();
    }

    public FishDef_SO BaitedFish()
    {
        return basicFishList[Random.Range(0, basicFishList.Count)];
    }
    public void CaughtFish(FishDef_SO fishDef)
    {
        FishData newData = new FishData(fishDef, 10, 10);
        PlayerManager.player.playerFishInventory.AddFish(newData);
    }

    public void ThrowBait()
    {
        Vector3 baitPos = PlayerManager.player.playerInteract.interactedPos;
        bait.Throw(PlayerManager.player.rotTipTransform.position, baitPos);
        pixelLineDrawer.enabled = true;
        pixelLineDrawer.SetStartEndTransforms(PlayerManager.player.rotTipTransform, bait.transform);
    }
    public void RecallBait()
    {
        bait.Recall();
        pixelLineDrawer.enabled = false;
    }
}
