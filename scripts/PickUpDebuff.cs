using Godot;
using System;
using Godot.Collections;

public partial class PickUpDebuff : Node2D
{
    [Export] public Array<int> SpeedDebuffs = [];
    public int RandomDebuff;
    public int DebuffTime;

    public override void _Ready()
    {
        GD.Randomize();
        RandomDebuff = SpeedDebuffs[(int)(GD.Randi() % SpeedDebuffs.Count)];
        DebuffTime = (int)GD.RandRange(5, 25);
    }
}