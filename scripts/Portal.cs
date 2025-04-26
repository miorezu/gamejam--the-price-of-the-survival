using Godot;
using System;

public partial class Portal : Node2D
{
    [Export] public Timer LiveTimer;

    public override void _EnterTree()
    {
        LiveTimer.Timeout += LiveTimerOnTimeout;
    }

    public override void _ExitTree()
    {
        LiveTimer.Timeout -= LiveTimerOnTimeout;
    }

    private void LiveTimerOnTimeout()
    {
        QueueFree();
    }
}