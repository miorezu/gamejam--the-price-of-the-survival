using Godot;
using System;

public partial class HurtBox : Area2D
{
    [Export] public int MaxHp;
    private int _currentHp;

    public override void _Ready()
    {
        Revive();
    }

    public bool IsAlive => _currentHp > 0;

    public void TakeDamage(int damage)
    {
        if (damage >= _currentHp)
        {
            UpdateHp(damage);
            Die();
            return;
        }

        UpdateHp(damage);
    }

    private void UpdateHp(int damage)
    {
        _currentHp -= damage;
    }

    private void Die()
    {
        if (GetParent() is CrystalStation)
        {
            return;
        }
        GetParent().QueueFree();
    }

    private void Revive()
    {
        _currentHp = MaxHp;
    }
}
