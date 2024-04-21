using Godot;
using System;
using System.Diagnostics;
public class Player_coreEngine : baseCoreEngine
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.




    public override void _Ready()
    {
        base._Ready();
    }


    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
    }

    private int _bulletNumber = 0;
    private int _bulletNumberNow = 0;

    protected override void shoot()
    {
        base.shoot();
        var bullet = GD.Load<PackedScene>("res://scence/BulletScence.tscn");
        var bullet1 = (BulletScence)bullet.Instance();
        _playerScenceParents.bulletPool.AddChild(bullet1);
        var damge = _shipBulidData.damage * _playerScenceParents._shipCoefficient;
        _bulletNumber++;

        if(_bulletNumberNow < _shootPosition.Count)
        {
            //GD.Print("_bulletData.path"+ _bulletData);
            bullet1.InitWithTSCN(_bulletData.path, _shootPosition[_bulletNumberNow].GlobalPosition, _playerScenceParents.nearEnemyToPlayerPos[0], BulletScence.bulletType.bulletType_player, damge, _bulletData.mode, _bulletNumber);
            _bulletNumberNow++;
        }
        else
        {
            _bulletNumberNow = 0;
            bullet1.InitWithTSCN(_bulletData.path, _shootPosition[_bulletNumberNow].GlobalPosition, _playerScenceParents.nearEnemyToPlayerPos[0], BulletScence.bulletType.bulletType_player, damge, _bulletData.mode, _bulletNumber);
        }

    }
}
