using Godot;
using System;
using CraterSprite.Effects;

namespace CraterSprite;

public enum Team
{
    Left,
    Right,
    Enemy,
    Unaffiliated
}


/**
 * Class for holding relevant character stats information
 * including health
 */
public partial class CharacterStats : Node
{
    private readonly StatusEffectContainer _effects = new();

    public override void _Ready()
    {
        _effects.ApplyStatusEffectInstance(new StatusEffectInstance(GameMode.instance.statusEffects.health, this));
    }
}
