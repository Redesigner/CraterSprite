using CraterSprite.Effects;
using Godot;

namespace CraterSprite;

public partial class GameMode : Node
{
    public static GameMode instance { get; private set; }
    
    public StatusEffectList statusEffects { get; private set; }

    public override void _Ready()
    {
        instance = this;

        statusEffects = ResourceLoader.Load<StatusEffectList>("res://Game/Effects/SL_Effects.tres");
        ImGuiGodot.ImGuiGD.ToolInit();
    }
}