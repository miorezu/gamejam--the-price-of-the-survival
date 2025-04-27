using Godot;
using System;

public partial class SpawnObject : Area2D
{
    [Export] public Timer SpawnTimer;
    [Export] public CollisionShape2D SpawnArea;
    [Export] public PackedScene SpawnObjectScene;

    public override void _EnterTree()
    {
        SpawnTimer.Timeout += SpawnTimerOnTimeout;
    }

    public override void _ExitTree()
    {
        SpawnTimer.Timeout -= SpawnTimerOnTimeout;
    }

    private void SpawnTimerOnTimeout()
    {
        SpawnObjects();
    }

    private void SpawnObjects()
    {
        if (SpawnArea.Shape is RectangleShape2D areaShape)
        {
            var spawnObject = SpawnObjectScene.Instantiate<Node2D>();
            spawnObject.GlobalPosition = GlobalPosition;
            var spawnAreaSize = areaShape.Size;
            var x = GD.RandRange(-areaShape.Size.X / 2, spawnAreaSize.X / 2);
            var y = GD.RandRange(-spawnAreaSize.Y / 2, spawnAreaSize.Y / 2);
            spawnObject.GlobalPosition += new Vector2((float)x, (float)y);
            GetParent().AddChild(spawnObject);
        }
    }
}