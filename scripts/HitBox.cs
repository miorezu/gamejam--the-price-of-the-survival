using Godot;
using System;

public partial class HitBox : Area2D
{
    [Export] public int Damage;
    [Export] public HurtBox ExceptHurtbox;
    public override void _EnterTree()
    {
        AreaEntered += OnAreaEntered;
    }

    public override void _ExitTree()
    {
        AreaEntered -= OnAreaEntered;
    }

    private void OnAreaEntered(Area2D area)
    {
        if (area is HurtBox hurtBox && area != ExceptHurtbox)
        {
            hurtBox.TakeDamage(Damage);
        }
    }
}
