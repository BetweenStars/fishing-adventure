using System;
using UnityEngine;

[Serializable]
public class FishData
{
    public int fishID;
    public float size;
    public float price;

    public FishData(int fishID, float size, float price)
    {
        this.fishID = fishID;
        this.size = size;
        this.price = price;
    }
}
