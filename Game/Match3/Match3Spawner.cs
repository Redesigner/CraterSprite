using System.Collections.Generic;
using CraterSprite.Props;
using Godot;

namespace CraterSprite.Game.Match3;

public partial class Match3Spawner : Node2D
{
    
    // Spawn table for enemies only
    [Export] private SpawnTable.SpawnTable _enemySpawnTable;
    
    // How long it takes to spawn a new enemy
    [Export] private float enemySpawnTime = 10.0f;

    [Export] private uint _maxEnemyCount = 5;
    [Export] private uint _currentEnemyCount = 0;
    
    private Queue<PackedScene> _pendingSpawns = [];
    // Pending enemy spawns is a separate queue, because we'll register the death callback once the enemies spawn
    private Queue<PackedScene> _pendingEnemySpawns = [];
    private List<EnemySpawner> _enemySpawners = [];

    private float _currentEnemySpawnTime = 0.0f;

    public override void _Ready()
    {
        _enemySpawners = CraterFunctions.GetAllNodesByClass<EnemySpawner>(this);
        GD.Print($"[Match3Spawner] Found {_enemySpawners.Count} spawners");
    }

    public override void _Process(double delta)
    {
        var deltaTime = (float)delta;

        UpdateEnemySpawns(deltaTime);
        UpdatePickupSpawns(deltaTime);
    }

    public void QueueSpawn(PackedScene enemySpawn)
    {
        GD.Print($"[Match3Spawner] Queueing spawn of {enemySpawn.GetPath()}");
        // Try to spawn right away
        var spawner = GetRandomAvailableSpawner();
        if (spawner == null)
        {
            _pendingSpawns.Enqueue(enemySpawn);
            return;
        }
        
        spawner.SpawnEnemy(enemySpawn);
    }

    private void QueueEnemySpawn(PackedScene enemySpawn)
    {
        GD.Print($"[Match3Spawner] Queueing spawn of {enemySpawn.GetPath()}");
        // Try to spawn right away
        
        // Enemy count immediately increases, regardless of whether the enemy goes in the queue or not
        ++_currentEnemyCount;
        var spawner = GetRandomAvailableSpawner();
        if (spawner == null)
        {
            _pendingSpawns.Enqueue(enemySpawn);
            return;
        }
        
        var spawnedEnemy = spawner.SpawnEnemy(enemySpawn);
        if (spawnedEnemy == null)
        {
            return;
        }

        spawnedEnemy.TreeExited += () =>
        { if (_currentEnemyCount > 0)
        {
            --_currentEnemyCount;
        } };
    }

    public void QueueRelativeSpawn(PackedScene spawn, Vector2 offset)
    {
        var newOffset = offset;
        newOffset.X *= -1.0f;
        CraterFunctions.CreateInstanceDeferred<Node2D>(this, spawn, GlobalPosition + newOffset);
    }

    private EnemySpawner GetRandomAvailableSpawner()
    {
        var pendingSpawners = new List<EnemySpawner>(_enemySpawners);

        for (var i = 0; i < _enemySpawners.Count; ++i)
        {
            var chosenSpawner = CraterMath.ChooseRandom(pendingSpawners);
            if (chosenSpawner.CanSpawn())
            {
                return chosenSpawner;
            }
            pendingSpawners.Remove(chosenSpawner);
        }

        return null;
    }

    private void UpdateEnemySpawns(float deltaTime)
    {
        if (_currentEnemyCount >= _maxEnemyCount)
        {
            return;
        }
        
        _currentEnemySpawnTime += deltaTime;

        if (_currentEnemySpawnTime < enemySpawnTime)
        {
            return;
        }
        
        QueueEnemySpawn(_enemySpawnTable.GetRandomEntry());
        _currentEnemySpawnTime -= enemySpawnTime;
    }

    private void UpdatePickupSpawns(float deltaTime)
    {
        
    }
}