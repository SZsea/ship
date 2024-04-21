using cfg;
using Godot;
using System;

public class EnemyScence : SZBaseNode2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.

    [Export]
    public PackedScene _BulletScence;


    private cfg.enemyInfoCfg _monsterData = new cfg.enemyInfoCfg();
    private cfg.bulletInfoCfg _bulletData = new cfg.bulletInfoCfg();

    //小地图引用
    public Node objectRe;
    public cfg.enemyInfoCfg monsterData
    {
        get
        {
            return _monsterData;
        }
    }


    public override void _Ready()
    {
      
    }

    public Enemy_normal _ownEnemy;
    public void initWithMonsterId(int id)
    {
        foreach (var item in cfg.enemyInfoCfg.GetAllList())
        {
            if(item.id == id)
            {
                _monsterData = item;
            }
        }
        Enemy_normal enemy1;
        if(_monsterData.isRandom) //随机怪物
        {
            if(_monsterData.isBoss ==true)
            {
                int randNumber = cfg.rangEnemyCfg.GetAllBoss().Count;
                randNumber = (int)(randNumber * GD.Randf() - 0.001);

                rangEnemyCfg str = cfg.rangEnemyCfg.GetAllBoss()[randNumber];
                var enemy = GD.Load<PackedScene>(str.path);
                enemy1 = (Enemy_normal)enemy.Instance();
                _monsterData.health = _monsterData.health + gameDate().playerEverPlayedLevel * 100;
            }
            else
            {
                int randNumber = cfg.rangEnemyCfg.GetAllNormal().Count;
                randNumber = (int)(randNumber * GD.Randf() - 0.001);

                rangEnemyCfg str = cfg.rangEnemyCfg.GetAllNormal()[randNumber];
                var enemy = GD.Load<PackedScene>(str.path);
                enemy1 = (Enemy_normal)enemy.Instance();
                _monsterData.health = _monsterData.health + gameDate().playerEverPlayedLevel * 5;
            }

        }else
        {
            var enemy = GD.Load<PackedScene>(_monsterData.path);
            enemy1 = (Enemy_normal)enemy.Instance();
        }

        _ownEnemy = enemy1;
        AddChild(enemy1);
        enemy1.setBodyParents(this);
        //初始化怪物位置
        //enemy1.GlobalPosition = new Vector2(GD.Randf() * 2 > 1 ? -400 + GD.Randf() * 200f : 400 + GD.Randf() * 200f, 
                                           // GD.Randf() * 2 > 1 ? -400 + GD.Randf() * 200f : 400 + GD.Randf() * 200f) + mainRoot().gameScenceIns.playerScence.GlobalPosition;
        enemy1.GlobalPosition = GlobalConstant.getRandBitrth(mainRoot().gameScenceIns.playerScence.GlobalPosition, 600f, 1000f);
       // GD.Print("enemy1.GlobalPosition " + enemy1.GlobalPosition);
        foreach (var item in cfg.bulletInfoCfg.GetAllList())
        {
            if (item.id == _monsterData.bulletId)
            {
                _bulletData = item;
            }   
        }
        if(_monsterData.isRandom)
        {
            int randNumber = cfg.bulletInfoCfg.GetAllList().Count;
            randNumber = (int)(8f * GD.Randf() - 0.001);//随机敌人弹幕
            bulletInfoCfg str = cfg.bulletInfoCfg.GetAllList()[randNumber];
            _monsterData.bulletId = str.id;
            _bulletData = str;
            _monsterData.aimDistance = (int)(200f + 300f * GD.Randf());
            _monsterData.onceShootMaxNumber = 1 + (int)(GD.Randf() * 3.0f);
            _monsterData.reload_oneShoot = 0.1f + (int)(GD.Randf() * 0.5f);
            _monsterData.reload_time = 1 + (int)(GD.Randf() * 2f);
            //GD.Print("*&*&*^^&*^*&" + _bulletData.id);
        }

        

        enemy1._damage = _monsterData.damge;
    }

    private int _bulletNumber = 0;
    public void shoot()
    {

        var bullet1 = (BulletScence)_BulletScence.Instance();
        mainRoot().gameScenceIns.bulletPool.AddChild(bullet1);
        _bulletNumber++;
       if (_ownEnemy._shootPosition.Count>0)
        {
            for(int i=0;i< _ownEnemy._shootPosition.Count;i++)
            {
                if (_bulletNumber % _ownEnemy._shootPosition.Count == i)
                    bullet1.InitWithTSCN(_bulletData.path, _ownEnemy._shootPosition[i].GlobalPosition, mainRoot().gameScenceIns.playerScence.Position, BulletScence.bulletType.bulletType_enemy, _monsterData.damge, _bulletData.mode, _bulletNumber);
            }
         
        }
        else
        {
            bullet1.InitWithTSCN(_bulletData.path, _ownEnemy.Position, mainRoot().gameScenceIns.playerScence.Position, BulletScence.bulletType.bulletType_enemy, _monsterData.damge, _bulletData.mode, _bulletNumber);
        }
        

    }


    private float _pseudo_timer;
    private int _bulletOnceShootCount = 0;
    private int _shootTime = 0;
    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);

        if (IsInstanceValid(_ownEnemy))
        {
            //GD.Print("@##"+ _monsterData.bulletId);
            var dir = mainRoot().gameScenceIns.playerScence.Position - _ownEnemy.Position;
            _ownEnemy.setDirToPlayer(dir);
            if (!_ownEnemy.isFinished)
                return;
            if (_monsterData.bulletId == 0)
                return;
            if (dir.Length() > _monsterData.aimDistance)
            {
                _ownEnemy.endAttack();
                return;
            }

            _pseudo_timer += delta;
            if(_shootTime == 0 && _pseudo_timer > _bulletOnceShootCount * _monsterData.reload_oneShoot)
            {
                _bulletOnceShootCount += 1;
                _ownEnemy.attack();
                shoot();
                if (_bulletOnceShootCount >= _monsterData.onceShootMaxNumber)
                {
                    _pseudo_timer -= _bulletOnceShootCount * _monsterData.reload_oneShoot;
                    _bulletOnceShootCount = 0;
                    _shootTime++;

                }
            }else if (_shootTime > 0 && _pseudo_timer > _monsterData.reload_time + _bulletOnceShootCount * _monsterData.reload_oneShoot)
            {        
                _bulletOnceShootCount += 1;
                _ownEnemy.attack();               
                shoot();
                if (_bulletOnceShootCount >= _monsterData.onceShootMaxNumber)
                {
                    _pseudo_timer -= _monsterData.reload_time + _bulletOnceShootCount * _monsterData.reload_oneShoot;
                    _bulletOnceShootCount = 0;
                    _shootTime++;
                }

            }else if(_pseudo_timer < _monsterData.reload_time)
            {
                _ownEnemy.endAttack();
            }
        }

    }

    public float DistanceToPlayer()
    {

        return _ownEnemy.distanceToPlayer();
    }

    public Vector2 EnemyPosition()
    {
        return _ownEnemy.GlobalPosition;
    }

    public Vector2 DistanceToPlayerV()
    {
        return _ownEnemy.distanceToPlayerV();
    }


    public void on_healthchange(int value,Enemy_normal enmey)
    {
        _monsterData.health -= value;
        var labelPacked = GD.Load<PackedScene>("res://scence/UI/CustomTemporaryLabel.tscn");
        var label = (CustomTemporaryLabel)labelPacked.Instance();
        this.AddChild(label);
        label.Text = value.ToString();
        label.RectPosition = _ownEnemy.Position;
        //GD.Print(label.RectPosition + "on_healthchange" + _ownEnemy.Position);
        label.trick = true;
        if (_monsterData.health <= 0)
        {
            gameDate().goldChange(_monsterData.rewardPrice);
            mainRoot().gameScenceIns.updateUIGold();
            //要先处理怪物死亡动画 再处理飘字 最后销毁
            enmey.destroy();
            //queueFreeAll();
        }

    }

    public void suicide()
    {
        //gameDate().goldChange(_monsterData.rewardPrice);
        //mainRoot().gameScenceIns.updateUIGold();
        //要先处理怪物死亡动画 再处理飘字 最后销毁
        _ownEnemy.destroy();
    }


    public void queueFreeAll()
    {
        if(IsInstanceValid(objectRe))
            objectRe.QueueFree();//小地图的图标去掉
        QueueFree();
    }

}
