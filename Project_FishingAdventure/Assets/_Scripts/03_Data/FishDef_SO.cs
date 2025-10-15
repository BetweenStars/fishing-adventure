using UnityEngine;

[CreateAssetMenu(fileName = "NewFish", menuName = "ScriptableObject/Entity/Movable/FishSO")]
public class FishDef_SO : StaticEntityDef_SO
{
    public Sprite sprite;

    public int weight;
}
