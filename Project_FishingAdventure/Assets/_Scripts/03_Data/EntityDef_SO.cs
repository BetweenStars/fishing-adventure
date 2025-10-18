using UnityEngine;

public enum EntityType
{
    PLAYER = 0,
    SHIP = 1,
    FISHING_ROD = 100,
    BAIT = 200,
    FISH = 300
}

public abstract class EntityDef_SO : ScriptableObject
{
    [Header("Entity Properties")]
    [SerializeField] private int _entityID = -1;
    public int entityID => _entityID;
    public abstract EntityType entityType { get; }
    public string entityName;

#if UNITY_EDITOR
    public void SetID(int newId)
    {
        _entityID = newId;
    }

    public int StartID => (int)entityType;
#endif
}
