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
        _currentHp -= damage;
        _currentHp = Mathf.Max(_currentHp, 0);

        if (GetParent() is CrystalStation station)
        {
            station.ReduceEnergy(damage);
        }

        if (_currentHp <= 0)
        {
            Die();
        }
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