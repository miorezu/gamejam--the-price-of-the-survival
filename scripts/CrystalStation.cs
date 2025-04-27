using Godot;
using System;

public partial class CrystalStation : StaticBody2D
{
    [Export] public TextureProgressBar EnergyBar;
    [Export] public Timer Timer;
    [Export] public AnimatedSprite2D AnimatedSprite;
    [Export] public AudioStreamPlayer BreakingAudio;
    [Export] public int EnergyPerSecond;
    [Export] public InteractableComponent InteractableComponent;
    [Export] public PlayerInventory.ItemTypes Type;
    [Export] public int EnergyFromItem;

    public override void _EnterTree()
    {
        Timer.Timeout += TimerOnTimeout;
        InteractableComponent.PlayerInteracted += OnPlayerInteracted;
    }

    public override void _ExitTree()
    {
        Timer.Timeout -= TimerOnTimeout;
        InteractableComponent.PlayerInteracted -= OnPlayerInteracted;
    }

    public override void _Process(double delta)
    {
        AnimatedSprite.Play("Idle");
        if (EnergyBar.Value == 0)
        {
            Die();
        }
    }

    private void Die()
    {
        BreakingAudio.Play();
        AnimatedSprite.Play("Broken");
        SetProcess(false);
        Timer.Stop();
    }

    private void TimerOnTimeout()
    {
        WasteEnergy();
    }

    private void WasteEnergy()
    {
        EnergyBar.Value -= EnergyPerSecond;
    }

    public void AddEnergy()
    {
        EnergyBar.Value += EnergyFromItem;
    }

    private void OnPlayerInteracted(Player player)
    {
        if (player.Inventory.HaveItem(Type))
        {
            AddEnergy();
        }
        player.Inventory.DecreaseItem(Type);
    }
}