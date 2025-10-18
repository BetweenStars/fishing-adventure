using UnityEngine;

[CreateAssetMenu(fileName = "NewShip", menuName = "ScriptableObject/Entity/Movable/ShipSO")]
public class ShipDef_SO : MovableEntityDef_SO
{
    public override EntityType entityType => EntityType.SHIP;
}
