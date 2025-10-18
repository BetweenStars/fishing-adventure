using UnityEngine;

[CreateAssetMenu(fileName = "NewFishingRod", menuName = "ScriptableObject/Entity/Static/FishingRodSO")]
public class FishingRodDef_SO : StaticEntityDef_SO
{
    public override EntityType entityType => EntityType.FISHING_ROD;
    public float price;
}
