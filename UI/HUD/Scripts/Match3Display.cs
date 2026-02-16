using Godot;
using System;
using System.Collections.Generic;
using CraterSprite;
using CraterSprite.Match3;

public partial class Match3Display : Node
{
    [Export]
    private Godot.Collections.Array<ColorRect> displays
    {
        set => _colorDisplays = CraterFunctions.ConvertArray(value);
        get => CraterFunctions.ConvertToGodotArray(_colorDisplays);
    }
    
    [Export] private int _playerIndex;

    private List<ColorRect> _colorDisplays = [];
    
    public override void _EnterTree()
    {
        GameMode.instance.onPlayerSpawned.AddListener(PlayerSpawned);
    }

    public override void _ExitTree()
    {
        GameMode.instance.onPlayerSpawned.RemoveListener(PlayerSpawned);
    }

    private void PlayerSpawned(int index, Node2D playerRoot)
    {
        if (index != _playerIndex)
        {
            return;
        }

        GD.Print($"[HUD] Binding HUD to player {index}");
        var playerState = CraterFunctions.GetNodeByClass<PlayerState>(playerRoot);
        if (playerState != null)
        {
            BindContainer(playerState.container);
        }
    }

    public void BindContainer(Match3Container container)
    {
        container.onOrbsChanged.AddListener(ContainerChanged);
        ContainerChanged(container.orbs);
    }

    private void ContainerChanged(List<MatchType> container)
    {
        GD.Print($"[HUD] Displaying orbs: {string.Join(", ", container)}");
        for (var i = 0; i < displays.Count; ++i)
        {
            displays[i].Color = i < container.Count ? Match3RecipeTable.GetColor(container[i]) : new Color(0.0f, 0.0f, 0.0f, 0.2f);
        }
    }
}
