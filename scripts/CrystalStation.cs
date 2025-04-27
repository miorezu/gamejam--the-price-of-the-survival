using Godot;
using System;

public partial class CrystalStation : StaticBody2D
{
    [Export] public TextureProgressBar EnergyBar;
    [Export] public Timer Timer;
    [Export] public AnimatedSprite2D AnimatedSprite;
    [Export] public AudioStreamPlayer BreakingAudio;

    public override void _EnterTree()
    {
        Timer.Timeout += TimerOnTimeout;
    }

    public override void _ExitTree()
    {
        Timer.Timeout -= TimerOnTimeout;
    }

    public override void _Process(double delta)
    {
        AnimatedSprite.Play("Idle");
        GD.Print(EnergyBar.Value);
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
    }

    private void TimerOnTimeout()
    {
        EnergyBar.Value -= 10;
    }
}