using Godot;
using System;
using System.Diagnostics;

public class Enemy_normal : baseEnemy
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.

    //管理怪物运动和生成
    //管理boss



    public float _damage;



    public override void _Ready()
    {
        base._Ready();
        
    }



    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        

        if (!_alive)
            return;

        KinematicCollision2D collisionInfo;
        //行动模式
        if (!isBoss)
        {
            collisionInfo = MoveAndCollide(_dir.Normalized() * _speed * delta);

        }else
        {
            if(_dir.Length() >= 300f )
            {
                collisionInfo = MoveAndCollide(_dir.Normalized() * _speed * delta);
            }
            else
            {
                collisionInfo = MoveAndCollide(_dir.Normalized() * 1f * delta);
            }
            
        }

        
        LookAt(_dir + this.Position);
        if (collisionInfo != null)
        {
            if(collisionInfo.Collider.HasMethod("playerReciveDamge"))
            {
                collisionInfo.Collider.Call("playerReciveDamge", _damage);
            }
       

        }


    }

}
