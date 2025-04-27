using Godot;
using System;

public partial class InteractableComponent : Area2D
{
    [Signal]
    public delegate void PlayerInteractedEventHandler(Player player);

    [Export] public bool MultiplyUse;
    [Export] public bool AutoDispose;
    
    public void Interact(Player player)
    {
        EmitSignalPlayerInteracted(player);
        if (MultiplyUse)
        {
            SwitchCollisionLayer();
        }

        if (AutoDispose)
        {
            GetParent().QueueFree();
        }
    }

    public async void SwitchCollisionLayer()
    {
        CollisionLayer &= ~(uint)(1 << 0);
        await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
        CollisionLayer = (uint)(1 << 0);
    }
}
