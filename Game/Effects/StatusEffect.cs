using Godot;

namespace CraterSprite.Effects;

public enum EffectAccumulator
{
    None,
    BaseValueOnly,
    Additive,
    Multiplicative,
    Maximum,
    Minimum
}

[GlobalClass]
public partial class StatusEffect : Resource
{
    [Export] public string effectName;
    
    [Export] public EffectAccumulator accumulator;
}