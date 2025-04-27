using Godot;
using System;

public partial class Spell : CharacterBody2D
{
    [Export] public int Speed;
    [Export] public AnimatedSprite2D AnimatedSprite2D;
    private Vector2 _direction;
    [Export] public Area2D CollisionArea;
    [Export] public Timer LiveTimer;
    [Export] public AudioStreamPlayer SpawnAudio;

    public override void _EnterTree()
    {
        CollisionArea.AreaEntered += CollisionAreaOnAreaEntered;
        LiveTimer.Timeout += LiveTimerOnTimeout;
    }

    public override void _ExitTree()
    {
        CollisionArea.AreaEntered -= CollisionAreaOnAreaEntered;
        LiveTimer.Timeout -= LiveTimerOnTimeout;
    }

    public override void _Ready()
    {
        _direction = GetGlobalMousePosition() - GlobalPosition;
        _direction = _direction.Normalized(); 
    }

    public override void _Process(double delta)
    {
        Velocity = _direction * Speed;
        MoveAndSlide();

        AnimatedSprite2D.Play("Idle");

        if (_direction.Length() > 0)
        {
            AnimatedSprite2D.Rotation = _direction.Angle();
        }
    }

    private void CollisionAreaOnAreaEntered(Area2D area)
    {
        if (area is HurtBox)
        {
            QueueFree();
        }
    }

    private void LiveTimerOnTimeout()
    {
        QueueFree();
    }
}