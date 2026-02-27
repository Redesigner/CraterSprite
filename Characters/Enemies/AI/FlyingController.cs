using CraterSprite.Characters.Scripts;
using Godot;

namespace CraterSprite.Characters.Enemies.AI.Scripts;

public partial class FlyingController : Node
{
    [Export] private KinematicFlyer _character;
    [Export] private Node2D _target;

    public override void _EnterTree()
    {
        GameMode.instance.onPlayerSpawned.AddListener(PlayerSpawned);
    }

    public override void _ExitTree()
    {
        GameMode.instance.onPlayerSpawned.RemoveListener(PlayerSpawned);
    }


    public override void _Process(double delta)
    {
        if (_target == null)
        {
            return;
        }
        _character.SetMoveInput(_target.GlobalPosition - _character.GlobalPosition);
    }

    private void PlayerSpawned(int i, Node2D player)
    {
        // I'll probably move this hook somewhere else
        if (i != 0)
        {
            return;
        }

        SetTarget(player);
    }

    private void SetTarget(Node2D player)
    {
        _target = player;
        player.TreeExited += () =>
        {
            _target = null;
            _character.SetMoveInput(Vector2.Zero);
        };
    }
}
