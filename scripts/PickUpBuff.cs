using Godot;
using System;
using Godot.Collections;

public partial class PickUpBuff : Node2D
{
    [Export] public Array<int> Buffs = [];
    [Export] public double GooseChance;
    public int RandomBuff;
    public int BuffTime;

    public override void _Ready()
    {
        GD.Randomize();
        RandomBuff = Buffs[(int)(GD.Randi() % Buffs.Count)];
        BuffTime = (int)GD.RandRange(5, 25);
    }

    public bool TryTurnIntoGoose()
    {
        return GD.Randf() <= GooseChance;
    }
}