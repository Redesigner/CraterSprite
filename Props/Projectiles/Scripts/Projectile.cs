using Godot;
using System;
using CraterSprite;

public partial class Projectile : Node2D
{
    private EncodedObjectAsId _owner;

    public void SetOwner(CharacterStats owner)
    {
        _owner = new EncodedObjectAsId
        {
            ObjectId = owner.GetInstanceId()
        };
    }
}
