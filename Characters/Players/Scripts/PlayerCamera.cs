using Godot;

namespace CraterSprite;

public partial class PlayerCamera : Camera2D
{
    [Export] private uint _playerIndex;
    
    public override void _Ready()
    {
        CraterFunctions.FindNodeByClass<RemoteTransform2D>(GameMode.instance.players[(int)_playerIndex]).SetRemoteNode(GetPath());
        ResetSmoothing();
    }
}