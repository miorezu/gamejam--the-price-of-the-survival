using Godot;
using System;
using System.Threading.Tasks;

public partial class Teleport : Control
{
    [Export] public float CooldownTime;
    [Export] public TextureProgressBar ProgressBar;
    [Export] public AudioStreamPlayer ReadySound;
    [Export] public double ChanceToSpawn;
    [Export] public PackedScene Portal;
    
    private bool _inCooldown;
    private float _currentCooldown;
    
    public override void _Ready()
    {
        ProgressBar.Value = 100;
    }

    public override void _Process(double delta)
    {
        if (_inCooldown)
        {
            _currentCooldown += (float)delta;
            ProgressBar.Value = Mathf.Clamp((_currentCooldown / CooldownTime) * 100f, 0, 100);

            if (_currentCooldown >= CooldownTime)
            {
                _inCooldown = false;
                PlayReadySound();
            }
        }
    }

    private void PlayReadySound()
    {
        ReadySound.Play();
    }
    
    public void Use(Player player, Vector2 position)
    {
        if (_inCooldown)
        {
            GD.Print("Skill is on cooldown!");
            return;
        }

        if (TrySpawnPortals())
        {
            SpawnPortals(player, position);
        }
        player.GlobalPosition = position;
        StartCooldown();
    }
    
    public bool TrySpawnPortals()
    {
        return GD.Randf() <= ChanceToSpawn;
    }

    public void SpawnPortals(Player player, Vector2 position)
    {
        var bluePortal = CreatePortal(player.GlobalPosition, "Blue");
        var orangePortal = CreatePortal(new Vector2(position.X, position.Y + 25), "Orange");

        GetParent().GetParent().AddChild(bluePortal);
        GetParent().GetParent().AddChild(orangePortal);
    }

    private Node2D CreatePortal(Vector2 position, string animationName)
    {
        var portal = Portal.Instantiate<Node2D>();
        portal.GlobalPosition = position;

        var portalSprite = portal.GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        portalSprite?.Play(animationName);

        return portal;
    }

    private void StartCooldown()
    {
        _currentCooldown = 0.0f;
        _inCooldown = true;
        ProgressBar.Value = 0;
    }
}