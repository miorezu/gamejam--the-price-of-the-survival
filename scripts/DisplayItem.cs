using Godot;
using System;

public partial class DisplayItem : Control
{
    [Export] public Label Label;
    [Export] public PlayerInventory.ItemTypes ItemType;

    public void Show(string text)
    {
        Label.Text = text;
    }
}
