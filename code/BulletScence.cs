using Godot;
using System;

public class BulletScence : SZBaseNode2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.

    public enum bulletType
    {
        bulletType_enemy, //敌人
        bulletType_player, //玩家
    }



    private baseBullet _ownBullet;
    public override void _Ready()
    {
        
    }

    public void InitWithTSCN(string src,Vector2 position, Vector2 target, bulletType type,float damage, int mode,int number)
    {
        switch(type)
        {
            case bulletType.bulletType_enemy:
                {
                    var bullet = GD.Load<PackedScene>(src);
                    //GD.Print("123123123" + src);
                    var bullet1 = (Enemy_bullet)bullet.Instance();
                    bullet1.damage = damage;
                    bullet1.setBodyParents(this);                    
                    _ownBullet = bullet1;
                    AddChild(bullet1);
                    _ownBullet.Position = position;
                    _ownBullet.setBulletTrackMode(target, 100f, mode, number);
                }
                break;
            case bulletType.bulletType_player:
                {
                    
                    var bullet = GD.Load<PackedScene>(src);
                    var bullet1 = (player_bullet)bullet.Instance();
                    bullet1.damage = damage;
                    bullet1.setBodyParents(this);                  
                    _ownBullet = bullet1;
                    AddChild(bullet1);
                    _ownBullet.Position = position;
                    _ownBullet.setBulletTrackMode(target, 100f, mode, number);
                }
                break;
        }        

    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);

        //Position += dir.Normalized() * 1f;
    }
}
