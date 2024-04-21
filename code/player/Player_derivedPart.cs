using Godot;
using System;

public class Player_derivedPart: baseDerivedPart
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
    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
    }

    private int _bulletNumber = 0;
    private int _bulletNumberNow = 0;

    public override void shoot()
    {
        base.shoot();
        //var bullet = GD.Load<PackedScene>("res://scence/BulletScence.tscn");
        //var bullet1 = (BulletScence)bullet.Instance();
        //_playerScenceParents.bulletPool.AddChild(bullet1);
        //var damge = _shipBulidData.damage * _playerScenceParents._shipCoefficient;
        //_bulletNumber++;
        //if (_bulletNumberNow < _shootPosition.Count)
        //{
        //    bullet1.InitWithTSCN(_bulletData.path, _shootPosition[_bulletNumberNow].GlobalPosition, _playerScenceParents.nearEnemyToPlayerPos, BulletScence.bulletType.bulletType_player, damge, _bulletData.mode, _bulletNumber);
        //    _bulletNumberNow++;
        //}
        //else
        //{
        //    _bulletNumberNow = 0;
        //    bullet1.InitWithTSCN(_bulletData.path, _shootPosition[_bulletNumberNow].GlobalPosition, _playerScenceParents.nearEnemyToPlayerPos, BulletScence.bulletType.bulletType_player, damge, _bulletData.mode, _bulletNumber);
        //}
    }


    //由动画直接调用
    public void shootByAnimation()
    {
        var bullet = GD.Load<PackedScene>("res://scence/BulletScence.tscn");
        var bullet1 = (BulletScence)bullet.Instance();
        _playerScenceParents.bulletPool.AddChild(bullet1);
        var damge = _shipBulidData.damage * _playerScenceParents._shipCoefficient;
        _bulletNumber++;

        if(weapon_auto_target == weaponShootMode.weaponShootMode_Line)
        {
            bullet1.InitWithTSCN(_bulletData.path, _shootPosition[_bulletNumberNow].GlobalPosition, _shootPosition[_bulletNumberNow].GlobalPosition + Vector2.Up.Rotated( _shootPosition[_bulletNumberNow].GlobalRotation), BulletScence.bulletType.bulletType_player, damge, _bulletData.mode, _bulletNumber);
            //GD.Print(_shootPosition[_bulletNumberNow].GlobalRotation);
            return;
        }


        if (weapon_auto_target == weaponShootMode.weaponShootMode_SpreedLine)
        {
            if (_bulletNumberNow >= _shootPosition.Count)
            {
                _bulletNumberNow = 0;
            }

            int onceShoot = 5;
            //
            Vector2 targetPosition = new Vector2();
            if (_bulletNumber == 1)
            {
                targetPosition = _shootPosition[_bulletNumberNow].GlobalPosition + Vector2.Up.Rotated(_shootPosition[_bulletNumberNow].GlobalRotation) - Vector2.Right.Rotated(_shootPosition[_bulletNumberNow].GlobalRotation) * 90f;
                //GD.Print("111111111111" + Vector2.Right.Rotated(_shootPosition[_bulletNumberNow].GlobalRotation));
            }
            else
            {
                targetPosition = _shootPosition[_bulletNumberNow].GlobalPosition + Vector2.Up.Rotated(_shootPosition[_bulletNumberNow].GlobalRotation);
            }

            int y = (_bulletNumber - 1 ) % (onceShoot * 2);
            float angle = 20f;
            if (y < onceShoot)
            {


                targetPosition = targetPosition + Vector2.Right.Rotated(_shootPosition[_bulletNumberNow].GlobalRotation) * angle;
            }else
            {

                targetPosition = targetPosition + Vector2.Right.Rotated(_shootPosition[_bulletNumberNow].GlobalRotation) * -angle;
            }

            

            if (IsInstanceValid(weapon))
                weapon.LookAt((targetPosition - weapon.GlobalPosition).Rotated((float)Math.PI / 2) + weapon.GlobalPosition);

            if (_weaponArray.Count > 0)
            {
                for (int i = 0; i < _weaponArray.Count; i++)
                {
                    _weaponArray[i].LookAt((targetPosition - _weaponArray[i].GlobalPosition).Rotated((float)Math.PI / 2) + _weaponArray[i].GlobalPosition);
                }
            }
            if (_bulletNumberNow < _shootPosition.Count)
            {
                bullet1.InitWithTSCN(_bulletData.path, _shootPosition[_bulletNumberNow].GlobalPosition, targetPosition, BulletScence.bulletType.bulletType_player, damge, _bulletData.mode, _bulletNumber);
                _bulletNumberNow++;
            }
            else
            {
                _bulletNumberNow = 0;
                bullet1.InitWithTSCN(_bulletData.path, _shootPosition[_bulletNumberNow].GlobalPosition, targetPosition, BulletScence.bulletType.bulletType_player, damge, _bulletData.mode, _bulletNumber);
            }


            //bullet1.InitWithTSCN(_bulletData.path, _shootPosition[_bulletNumberNow].GlobalPosition, targetPosition, BulletScence.bulletType.bulletType_player, damge, _bulletData.mode, _bulletNumber);
            //GD.Print(_shootPosition[_bulletNumberNow].GlobalRotation);
            return;
        }


        if (weapon_target_number > 1)
        {
            //GD.Print("weapon_target_number" + weapon_target_number);
            if (_weaponArray.Count > 0)
            {
                int a = (int)(GD.Randf() * weapon_target_number);
                if(a > (_playerScenceParents.nearEnemyToPlayerPos.Count - 1 ))
                {
                    a = 0;
                }
                //GD.Print("weapon_target_number" + a);
                for (int i = 0; i < _weaponArray.Count; i++)
                {

                    _weaponArray[i].LookAt((_playerScenceParents.nearEnemyToPlayerPos[a] - _weaponArray[i].GlobalPosition).Rotated((float)Math.PI / 2) + _weaponArray[i].GlobalPosition);

                }
                if (_bulletNumberNow < _shootPosition.Count)
                {
                    bullet1.InitWithTSCN(_bulletData.path, _shootPosition[_bulletNumberNow].GlobalPosition, _playerScenceParents.nearEnemyToPlayerPos[a], BulletScence.bulletType.bulletType_player, damge, _bulletData.mode, _bulletNumber);
                    _bulletNumberNow++;
                }
                else
                {
                    _bulletNumberNow = 0;
                    bullet1.InitWithTSCN(_bulletData.path, _shootPosition[_bulletNumberNow].GlobalPosition, _playerScenceParents.nearEnemyToPlayerPos[a], BulletScence.bulletType.bulletType_player, damge, _bulletData.mode, _bulletNumber);
                }
            }


        }
        else
        {
            if (IsInstanceValid(weapon))
                weapon.LookAt((_playerScenceParents.nearEnemyToPlayerPos[0] - weapon.GlobalPosition).Rotated((float)Math.PI / 2) + weapon.GlobalPosition);

            if (_weaponArray.Count > 0)
            {
                for (int i = 0; i < _weaponArray.Count; i++)
                {
                    _weaponArray[i].LookAt((_playerScenceParents.nearEnemyToPlayerPos[0] - _weaponArray[i].GlobalPosition).Rotated((float)Math.PI / 2) + _weaponArray[i].GlobalPosition);
                }
            }
            if (_bulletNumberNow < _shootPosition.Count)
            {
                bullet1.InitWithTSCN(_bulletData.path, _shootPosition[_bulletNumberNow].GlobalPosition, _playerScenceParents.nearEnemyToPlayerPos[0], BulletScence.bulletType.bulletType_player, damge, _bulletData.mode, _bulletNumber);
                _bulletNumberNow++;
            }
            else
            {
                _bulletNumberNow = 0;
                bullet1.InitWithTSCN(_bulletData.path, _shootPosition[_bulletNumberNow].GlobalPosition, _playerScenceParents.nearEnemyToPlayerPos[0], BulletScence.bulletType.bulletType_player, damge, _bulletData.mode, _bulletNumber);
            }
        }




       // GD.Print("_lockedEmemyLoaction + "+_lockedEmemyLoaction);
    }

}
