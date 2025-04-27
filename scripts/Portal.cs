using Godot;
using System;

public partial class Portal : Node2D
{
    [Export] public Timer LiveTimer;
    [Export] public AudioStreamPlayer Audio;

    public override void _EnterTree()
    {
        LiveTimer.Timeout += LiveTimerOnTimeout;
    }

    public override void _ExitTree()
    {
        LiveTimer.Timeout -= LiveTimerOnTimeout;
    }

    public override void _Ready()
    {
            Audio.Play();
    }

    private void LiveTimerOnTimeout()
    {
        QueueFree();
    }
}