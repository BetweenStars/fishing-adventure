using System.Collections;
using System.Collections.Generic;
using UnityEditor.Purchasing;
using UnityEngine;

public class FishingManager : MonoBehaviour
{
    public static FishingManager Instance;

    public List<FishDef_SO> basicFishList;

    public PixelLineDrawer pixelLineDrawer { get; private set; }

    public Bait bait { get; private set; }

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

    public FishData BaitedFish()
    {
        FishDef_SO fishDef = FishDataBase.Instance.GetRandomFishDef();
        float size = fishDef.baseSize * Random.Range(1.0f, 1.25f);
        float price = fishDef.basePrice * Random.Range(1.0f, 1.25f);
        FishData fishData = new FishData(fishDef.entityID, size, price);

        return fishData;
    }
    
    public void CaughtFish(FishData fishData)
    {
        PlayerManager.player.playerFishInventory.AddFish(fishData);
    }

    public void ThrowBait()
    {
        Vector3 baitPos = PlayerManager.player.playerInteract.interactedPos;

        bait.Throw(PlayerManager.player.rodTipTransform.position, baitPos);

        pixelLineDrawer.enabled = true;
        pixelLineDrawer.SetStartEndTransforms(PlayerManager.player.rodTipTransform, bait.transform);
    }

    public IEnumerator RecallBait()
    {
        yield return StartCoroutine(bait.Recall());

        pixelLineDrawer.enabled = false;
    }
}
