using System.Collections.Generic;
using System.Linq;
using Godot;

namespace CraterSprite.Match3;

[GlobalClass]
public partial class Match3RecipeTable : Resource
{
    [Export]
    private Godot.Collections.Array<Match3Recipe> recipes
    {
        set => _recipes = CraterFunctions.ConvertArray(value);
        get => CraterFunctions.ConvertToGodotArray(_recipes);
    }

    private List<Match3Recipe> _recipes = [];

    public bool CanMake(List<MatchType> ingredients)
    {
        return ingredients.Count <= 3 && _recipes.Any(recipe => recipe.CanMake(ingredients));
    }

    public PackedScene GetEnemy(List<MatchType> ingredients)
    {
        return ingredients.Count == 3 ? _recipes.Find(recipe => recipe.CanMake(ingredients))?.enemy : null;
    }

    public int count => _recipes.Count;

    public static Color GetColor(MatchType matchType)
    {
        return matchType switch
        {
            MatchType.Fire => new Color(1.0f, 0.0f, 0.0f),
            MatchType.Water => new Color(0.0f, 0.0f, 1.0f),
            MatchType.Wind => new Color(0.0f, 1.0f, 0.0f),
            MatchType.Light => new Color(0.9f, 0.9f, 0.5f),
            MatchType.Dark => new Color(0.8f, 0.1f, 0.8f),
            _ => new Color(0.0f, 0.0f, 0.0f, 0.0f)
        };
    }
}