using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
    [Export] public int Speed = 100;
    [Export] public Area2D DetectionArea;


    private CrystalStation _currentTarget = null;


    public override void _Ready()
    {
        if (DetectionArea == null)
        {
            GD.PrintErr("DetectionArea is not assigned in the Enemy script!");
        }
    }


    public override void _EnterTree()
    {
        if (DetectionArea != null)
        {
            DetectionArea.BodyEntered += OnDetectionAreaBodyEntered;

            DetectionArea.BodyExited += OnDetectionAreaBodyExited;
        }
    }


    public override void _ExitTree()
    {
        if (DetectionArea != null)
        {
            DetectionArea.BodyEntered -= OnDetectionAreaBodyEntered;
            DetectionArea.BodyExited -= OnDetectionAreaBodyExited;
        }
    }


    public override void _Process(double delta)
    {
        Vector2 targetPosition;


        if (_currentTarget != null && IsInstanceValid(_currentTarget))
        {
            targetPosition = _currentTarget.GlobalPosition;
        }
        else
        {
            targetPosition = Vector2.Zero;
            _currentTarget = null;
        }


        if (GlobalPosition.DistanceSquaredTo(targetPosition) > 1.0f)
        {
            Vector2 direction = (targetPosition - GlobalPosition).Normalized();

            Velocity = direction * Speed;
        }
        else
        {
            Velocity = Vector2.Zero;
        }


        MoveAndSlide();
    }


    private void OnDetectionAreaBodyEntered(Node2D body)
    {
        if (body is CrystalStation station)
        {
            GD.Print("CrystalStation detected by type!");
            _currentTarget = station;
        }
    }


    private void OnDetectionAreaBodyExited(Node2D body)
    {
        if (body == _currentTarget)
        {
            GD.Print("CrystalStation lost!");
            _currentTarget = null;
        }
    }
}