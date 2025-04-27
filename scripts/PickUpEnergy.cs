using Godot;
using System;

public partial class PickUpEnergy : Node2D
{
    [Export] public AnimatedSprite2D AnimatedSprite;
    [Export] public InteractableComponent InteractableComponent;
    [Export] public PlayerInventory.ItemTypes Type;

    public override void _EnterTree()
    {
        InteractableComponent.PlayerInteracted += OnPlayerInteracted;
    }

    public override void _ExitTree()
    {
        InteractableComponent.PlayerInteracted -= OnPlayerInteracted;
    }

    private void OnPlayerInteracted(Player player)
    {
        player.Inventory.IncreaseItem(Type);
    }
}