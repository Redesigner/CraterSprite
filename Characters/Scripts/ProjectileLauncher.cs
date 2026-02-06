using Godot;

namespace CraterSprite;
public partial class ProjectileLauncher : Node2D
{
    [Export] private PackedScene _projectile;

    public void FireProjectile()
    {
        if (_projectile == null)
        {
            return;
        }
        
        var projectileInstance = _projectile.Instantiate();
        if (projectileInstance == null)
        {
            return;
        }
        
        if (projectileInstance is not Node2D node2D)
        {
            projectileInstance.QueueFree();
            return;
        }

        node2D.SetGlobalPosition(GetGlobalPosition());
        GetTree().GetRoot().AddChild(node2D);
    }
}
