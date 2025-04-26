using Godot;
using System;

public partial class Player : CharacterBody2D
{
    [Export] public int Speed;
    [Export] public AnimatedSprite2D Sprite;

    public void GetInput()
    {
        Vector2 inputDirection = Input.GetVector("A", "D", "W", "S");
        Sprite.FlipH = inputDirection.X < 0;
        Velocity = inputDirection * Speed;
        if (inputDirection == Vector2.Zero)
        {
            Sprite.Play("idle");
        }
        else
        {
            Sprite.Play("Run");
        }
    }
    public override void _PhysicsProcess(double delta)
    {
        GetInput();
        MoveAndSlide();
    }
}
