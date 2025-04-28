using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
    [Export] public Area2D SeeArea;
    [Export] public Timer DirectionTimer;
    [Export] public int Speed;
    [Export] public AnimatedSprite2D Animation;
    private CrystalStation _station;

    public override void _Ready()
    {
        ChangeDirection();
    }

    public override void _EnterTree()
    {
        DirectionTimer.Timeout += ChangeDirection;
        SeeArea.BodyEntered += ChaseStation;
    }

    public override void _ExitTree()
    {
        DirectionTimer.Timeout -= ChangeDirection;
        SeeArea.BodyEntered -= ChaseStation;
    }

    public override void _Process(double delta)
    {
        Animation.Play("EnemyMove");

        if (_station != null)
        {
            Vector2 direction = (_station.GlobalPosition - GlobalPosition).Normalized();

            Velocity += direction;
            Velocity = Velocity.Normalized() * Speed;

            if (direction.X < 0) 
            {
                Animation.FlipH = true;
            }
            else if (direction.X > 0) 
            {
                Animation.FlipH = false;
            }
        }

        MoveAndSlide();
    }

    private void ChangeDirection()
    {
        Velocity = Velocity.Normalized();
        Velocity += new Vector2(GD.RandRange(-1, 1), GD.RandRange(-1, 1));
        Velocity = Velocity.Normalized() * Speed;
    }

    private void ChaseStation(Node2D body)
    {
        if (body is CrystalStation station)
        {
            _station = station;

            Die();
        }
    }

    private void Die()
    {
        GD.Print("Enemy died!");
        QueueFree();
    }
}