using Godot;
using System;

public partial class Player : CharacterBody2D
{
    [Export] public int Speed;
    [Export] public int NormalSpeed;
    private AnimatedSprite2D _sprite;
    [Export] public AnimatedSprite2D GooseSprite;
    [Export] public AnimatedSprite2D NormalSprite;
    [Export] public AudioStreamPlayer Audio;
    [Export] public CpuParticles2D FootStepsParticles;
    [Export] public Area2D InteractableArea;
    [Export] public AudioStreamPlayer Quack;

    public override void _Ready()
    {
        _sprite = NormalSprite;
    }

    public override void _EnterTree()
    {
        InteractableArea.AreaEntered += InteractableAreaOnAreaEntered;
    }

    public override void _ExitTree()
    {
        InteractableArea.AreaEntered -= InteractableAreaOnAreaEntered;
    }

    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed("RMB"))
        {
            Teleport(GetGlobalMousePosition());
        }
    }

    private void InteractableAreaOnAreaEntered(Area2D area)
    {
        if (area is InteractableComponent component)
        {
            if (component.GetParent() is PickUpDebuff debuff)
            {
                GetDebuff(debuff);
            }
            else if (component.GetParent() is PickUpBuff buff)
            {
                GetBuff(buff);
            }
        }
    }

    private async void GetDebuff(PickUpDebuff debuff)
    {
        Speed = debuff.RandomDebuff;
        debuff.QueueFree();
        await ToSignal(GetTree().CreateTimer(debuff.DebuffTime), "timeout");
        Speed = NormalSpeed;
    }

    private async void GetBuff(PickUpBuff buff)
    {
        Speed = buff.RandomBuff;
        buff.QueueFree();
        if (buff.TryTurnIntoGoose())
        {
            SwitchToGoose();
        }
        await ToSignal(GetTree().CreateTimer(buff.BuffTime), "timeout");
        Speed = NormalSpeed;
        SwitchToNormal();
    }

    private void SwitchToGoose()
    {
        NormalSprite.Visible = false;
        GooseSprite.Visible = true;
        _sprite = GooseSprite;
    }

    private void SwitchToNormal()
    {
        GooseSprite.Visible = false;
        NormalSprite.Visible = true;
        _sprite = NormalSprite;
    }

    private void Move()
    {
        Vector2 inputDirection = Input.GetVector("A", "D", "W", "S");
        _sprite.FlipH = inputDirection.X < 0;
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
        GD.Print(Speed);
    }

    private void DisplayIdle()
    {
        _sprite.Play("Idle");
        FootStepsParticles.Emitting = false;
        _sprite.FlipH = GetLocalMousePosition().X < 0;
    }

    private void DisplayRun(Vector2 inputDirection)
    {
        FootStepsParticles.Emitting = true;
        FootStepsParticles.Direction = FootStepsParticles.Direction with { X = -1 };
        if (inputDirection.X < 0)
        {
            FootStepsParticles.Direction = FootStepsParticles.Direction with { X = 1 };
        }

        _sprite.Play("Run");
    }

    private void PlayMusic()
    {
        if (GooseSprite.Visible && !Quack.Playing)
        {
            Quack.Play();
        }

        if (!Audio.Playing)
        {
            Audio.Play();
        }
    }

    private void Teleport(Vector2 mousePosition)
    {
        GlobalPosition = mousePosition;
    }
}