using System;
using UnityEngine;

[Serializable]
public class FishData
{
    public FishDef_SO fishDef;
    public float size;
    public float price;

    public FishData(FishDef_SO fishDef, float size, float price)
    {
        this.fishDef = fishDef;
        this.size = size;
        this.price = price;
    }
}
