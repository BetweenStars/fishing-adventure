using UnityEngine;

public enum FishRarity
{
    COMMON,RARE,EPIC,UNIQUE,MYTH
}

[CreateAssetMenu(fileName = "NewFish", menuName = "ScriptableObject/Entity/Static/FishSO")]
public class FishDef_SO : StaticEntityDef_SO
{
    public override EntityType entityType => EntityType.FISH;
    public Sprite sprite;

    public FishRarity rarity;

    public float baseSize;
    public float basePrice;

    public int weight;
}
