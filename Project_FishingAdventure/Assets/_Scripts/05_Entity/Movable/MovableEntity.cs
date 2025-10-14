using UnityEngine;

public abstract class MovableEntity : Entity
{
    public MovableEntityDef_SO movableEntityDef => entityDef as MovableEntityDef_SO;
}
