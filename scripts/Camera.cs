using Godot;
using System;

public partial class Camera : Camera2D
{
    [Export] public float ZoomValue;

    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed("WheelUp"))
        {
            Zoom = Zoom with { X = Zoom.X + ZoomValue, Y = Zoom.Y + ZoomValue };
        }

        if (Input.IsActionJustPressed("WheelDown"))
        {
            Zoom = Zoom with { X = Zoom.X - ZoomValue, Y = Zoom.Y - ZoomValue };
        }
    }
}