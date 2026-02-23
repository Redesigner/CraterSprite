using Godot;

namespace CraterSprite;

[GlobalClass]
public partial class GameModeSettings : Resource
{
    [Export] public PackedScene player;
    [Export] public uint playerCount = 2;
    // Remove this later
    [Export] public PackedScene gem;
    [Export] public SpawnTable.SpawnTable spawnTable;
    
    // How long it takes to spawn a new object for both players
    [Export] public float globalSpawnTime = 10.0f;
}