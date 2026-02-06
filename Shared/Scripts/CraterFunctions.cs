using Godot;

namespace CraterSprite;

public class CraterFunctions
{
    static T GetNodeByClass<T>(Node parent)
        where T : Node
    {
        foreach (var child in parent.GetChildren())
        {
            if (child is T node)
            {
                return node;
            }
        }

        return null;
    }

    static T GetNodeByClassFromParent<T>(Node self)
        where T : Node
    {
        var parent = self.GetParent();
        return parent == null ? null : GetNodeByClass<T>(parent);
    }

    static T GetNodeByClassFromRoot<T>(Node self)
        where T : Node
    {
        var root = self.Owner;
        return root == null ? null : GetNodeByClass<T>(root);
    }
}