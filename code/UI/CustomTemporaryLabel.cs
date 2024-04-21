using Godot;
using System;

public class CustomTemporaryLabel : Label
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    private const float _lifeTime = 1f;
    private float _temptime1 = 0;
    private float _temptime2 = 0;
    public bool trick = false;
    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        if (!trick)
            return;
        _temptime1 += delta;
        _temptime2 += delta;
        if(_temptime2 > 0.1f)
        {
            this.RectGlobalPosition += Vector2.Up * 10f;
            _temptime2 = 0;
        }
        if(_temptime1 > _lifeTime)
        {
            this.QueueFree();
        }
    }
}
