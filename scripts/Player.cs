using Godot;
using System;
using System.Collections.Generic;

public partial class Player : CharacterBody2D
{
    [Export] public int Speed;
    [Export] public int NormalSpeed;
    public AnimatedSprite2D Sprite;
    [Export] public AnimatedSprite2D GooseSprite;
    [Export] public AnimatedSprite2D NormalSprite;
    [Export] public AudioStreamPlayer Audio;
    [Export] public CpuParticles2D FootStepsParticles;
    [Export] public Area2D InteractableArea;
    [Export] public AudioStreamPlayer Quack;
    [Export] public Teleport TeleportSkill;
    private List<InteractableComponent> _nearbyObjects = [];
    [Export] public PlayerInventory Inventory;
    [Export] public PackedScene DistanceSpell;

    public bool IsTeleporting = false;

    public override void _Ready()
    {
        Sprite = NormalSprite;
    }

    public override void _EnterTree()
    {
        InteractableArea.AreaEntered += InteractableAreaOnAreaEntered;
        InteractableArea.AreaExited += InteractableAreaOnAreaExited;
    }

    public override void _ExitTree()
    {
        InteractableArea.AreaEntered -= InteractableAreaOnAreaEntered;
        InteractableArea.AreaExited -= InteractableAreaOnAreaExited;
    }

    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed("RMB"))
        {
            TryTeleport();
        }

        if (Input.IsActionJustPressed("E"))
        {
            var body = FindClosestObject();
            if (body == null)
                return;
            body.Interact(this);
            _nearbyObjects.Remove(body);
        }
        
        if (Input.IsActionJustPressed("LMB"))
        {
            var distanceSpell = DistanceSpell.Instantiate<Node2D>();
            distanceSpell.GlobalPosition = GlobalPosition;
            GetParent().AddChild(distanceSpell);
        }

    }
    private InteractableComponent FindClosestObject()
    {
        InteractableComponent closestObject = null;
        var closestDistance = float.MaxValue;
        foreach (var obj in _nearbyObjects)
        {
            var distance = (obj.GlobalPosition - GlobalPosition).Length();
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestObject = obj;
            }
        }

        return closestObject;
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
            _nearbyObjects.Add(component);
            GD.Print(area.GetParent().Name);
        }
    }
    
    private void InteractableAreaOnAreaExited(Area2D area)
    {
        if (area is InteractableComponent interactableComponent)
            _nearbyObjects.Remove(interactableComponent);
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
        Sprite = GooseSprite;
    }

    private void SwitchToNormal()
    {
        GooseSprite.Visible = false;
        NormalSprite.Visible = true;
        Sprite = NormalSprite;
    }

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
        if (IsTeleporting) return;
        Sprite.Play("Idle");
        FootStepsParticles.Emitting = false;
        Sprite.FlipH = GetLocalMousePosition().X < 0;
    }

    private void DisplayRun(Vector2 inputDirection)
    {
        if (IsTeleporting) return;
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
        if (GooseSprite.Visible && !Quack.Playing)
        {
            Quack.Play();
        }

        if (!Audio.Playing)
        {
            Audio.Play();
        }
    }

    private async void TryTeleport()
    {
        if (TeleportSkill == null || Sprite == null) return;
        IsTeleporting = true;
        Sprite.Play("StartTeleport");

        var timer = GetTree().CreateTimer(1);
        await ToSignal(timer, "timeout");

        TeleportSkill.Use(this, GetGlobalMousePosition());
    }
}