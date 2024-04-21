using Godot;
using System;
using System.Diagnostics;

public class Enemy_bullet : baseBullet
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
        
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }



    public override void _on_Node2D_body_entered(Godot.Object area)
    {
        base._on_Node2D_body_entered(area);
        if (area.HasMethod("playerReciveDamge"))
        {
            area.Call("playerReciveDamge", damage);
            _bulletScenceParents.QueueFree();
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);

    }


}
