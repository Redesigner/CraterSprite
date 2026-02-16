using System.Collections.Generic;
using CraterSprite.Match3;
using Godot;

namespace CraterSprite;

public class Match3Container
{
    public readonly List<MatchType> orbs = [];

    public CraterEvent<List<MatchType>> onOrbsChanged = new();

    public void AddOrb(MatchType matchType)
    {
        // Container shouldn't hold the none type
        if (matchType == MatchType.None)
        {
            return;
        }
        
        orbs.Add(matchType);
        OrbsChanged();
    }

    private void OrbsFull()
    {
        
    }

    private void OrbsChanged()
    {
        if (!GameMode.instance.recipes.CanMake(orbs))
        {
            // Notify spawn manager
            GD.Print("Could not spawn anything with this combination.");
            orbs.Clear();
        }
        
        onOrbsChanged.Invoke(orbs);
    }
}