using Godot;
using System;

public partial class Player : CharacterBody2D
{
    [Export] public int Speed;
    [Export] public AnimatedSprite2D Sprite;
    [Export] public AudioStreamPlayer Audio;
    [Export] public CpuParticles2D FootStepsParticles;

    private void Move()
    {
        Vector2 inputDirection = Input.GetVector("A", "D", "W", "S");
        Sprite.FlipH = inputDirection.X < 0;
        Velocity = inputDirection;
        Velocity = Velocity.Normalized() * Speed;
        if (inputDirection == Vector2.Zero)
        {
            DisplayIdle();
        }
        else
        {
            DisplayRun(inputDirection);
            PlayMusic();
        }
    }
    
    public override void _Process(double delta)
    {
        Move();
        MoveAndSlide();
    }

    private void DisplayIdle()
    {
        Sprite.Play("Idle");
        FootStepsParticles.Emitting = false;
        Sprite.FlipH = GetLocalMousePosition().X < 0;
    }

    private void DisplayRun(Vector2 inputDirection)
    {
        FootStepsParticles.Emitting = true;
        FootStepsParticles.Direction = FootStepsParticles.Direction with { X = -1 };
        if (inputDirection.X < 0)
        {
            FootStepsParticles.Direction = FootStepsParticles.Direction with { X = 1 };
        }

        Sprite.Play("Run");
    }

    private void PlayMusic()
    {
        if (!Audio.Playing)
        {
            Audio.Play();
        }
    }
}