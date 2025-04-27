using Godot;
using System;
using Godot.Collections;

public partial class PlayerInventory : Control
{
    [Export] public PackedScene DisplayItem;
    [Export] public Control DisplayItemsParent;
    public System.Collections.Generic.Dictionary<ItemTypes, int> Items = new();

    public enum ItemTypes
    {
        MagicFlower,
    }
    
    public void IncreaseItem(ItemTypes itemType)
    {
        if (Items.ContainsKey(itemType))
        {
            Items[itemType]++;
            UpdateDisplayItem(itemType);
            Output();
            return;
        }

        Items.Add(itemType, 1);
        AddNewDisplayItem(itemType);
        Output();
    }
    private void AddNewDisplayItem(ItemTypes itemType)
    {
        var displayItem = DisplayItem.Instantiate<DisplayItem>();
        displayItem.ItemType = itemType;
        displayItem.Show(OneOutput(itemType));
        DisplayItemsParent.AddChild(displayItem);
    }
    private void UpdateDisplayItem(ItemTypes itemType)
    {
        Array<Node> children = DisplayItemsParent.GetChildren();
        foreach (var child in children)
        {
            if (child is DisplayItem item && item.ItemType == itemType)
            {
                item.Show(OneOutput(itemType));
            }
        }
    }
    
    private string OneOutput(ItemTypes itemType)
    {
        return ($"{itemType} : {Items[itemType]}");
    }

    private void Output()
    {
        foreach (var pair in Items)
        {
            GD.Print($"{pair.Key} : {pair.Value}");
        }
    }

    public void DecreaseItem(ItemTypes itemType)
    {
        if (!HaveItem(itemType))
        {
            GD.Print("no item " + itemType);
            return;
        }
        Items[itemType]--;
        UpdateDisplayItem(itemType);
    }

    public bool HaveItem(ItemTypes itemType)
    {
        return Items[itemType] != 0;
    }
}
