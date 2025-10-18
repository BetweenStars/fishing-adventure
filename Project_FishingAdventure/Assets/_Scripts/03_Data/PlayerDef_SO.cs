using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayer", menuName = "ScriptableObject/Entity/Movable/PlayerSO")]
public class PlayerDef_SO : MovableEntityDef_SO
{
    public override EntityType entityType => EntityType.PLAYER;
}