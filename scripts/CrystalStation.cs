using Godot;
using System;

public partial class CrystalStation : StaticBody2D
{
    [Export] public TextureProgressBar EnergyBar;
    [Export] public int MaxEnergy;
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

    public override void _Ready()
    {
        EnergyBar.Value = EnergyBar.MaxValue = MaxEnergy;
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
        AnimatedSprite.Play("Destroing");
        AnimatedSprite.AnimationFinished += () => AnimatedSprite.Play("Broken");
        SetProcess(false);
        Timer.Stop();
        EnergyBar.QueueFree();
        InteractableComponent.QueueFree();
    }

    private void TimerOnTimeout()
    {
        WasteEnergy();
    }

    private void WasteEnergy()
    {
        EnergyBar.Value -= EnergyPerSecond;
    }

    private void AddEnergy()
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