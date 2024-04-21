using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public class PlayerScence : SZBaseNode2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.

    [Export]
    public NodePath mCamera;
    [Export]
    public NodePath bodyNode;
    [Export]
    public NodePath invincibleTimer;
    [Export]
    public NodePath PlayerControlPath;


    private Camera2D _camera;
    private Node2D _bodyNode;
    private Timer _invincibleTimer;
    private playerControl _playerControl;

    private int _health;
    private int _speed;
    private int _shiled;

    private Node2D _bulletPool;


    private baseBulidPart _playerBulidData;

    public baseBulidPart playerBulidData
    {
        get
        {
            if (IsInstanceValid(_playerBulidData))
            {
                return _playerBulidData;
            }
            else
            {
                return null;
            }

        }
        set
        {
            _playerBulidData = null; 
            _playerBulidData = value;
            _playerBulidData.GetParent().RemoveChild(_playerBulidData);
            _bodyNode.AddChild(_playerBulidData);

           // GD.Print(_playerBulidData);
        }
    }








    public Node2D bulletPool
    {
        set
        {
            _bulletPool = value;
        }
        get
        {
            if(_bulletPool == null)
            {
                GD.Print("PlayerScence + _bulletPool is null !!!!!!!!!!!!");
            }
            return _bulletPool;
        }
    }


    public override void _Ready()
    {
        base._Ready();
        if (GlobalConstant.READYMODE)
        {
            GD.Print("PlayerScence_Ready()" + this);
        }
        _camera = (Camera2D)GetNode(mCamera);
        _camera.Current = true;
        //控制摄像机镜头大小
        _camera.Zoom = new Vector2(1, 1);

        //GD.Print("111111111  " + _camera.GetViewport().Size);

        _bodyNode = (Node2D)GetNode(bodyNode);
        _invincibleTimer = (Timer)GetNode(invincibleTimer);
        _playerControl = (playerControl)GetNode(PlayerControlPath);



    }





    public float _shipCoefficient;
    public cfg.playerCoreCfg _shipData = new cfg.playerCoreCfg();
    public int _shipRotation;
    public void init(baseBulidPart data)
    {
       
        playerBulidData = data.copy();
        playerBulidData.setBodyParents(this);
        _shipData = playerBulidData.shipData();
        _health = _shipData.health;
        _speed = _shipData.speed;
        _shipRotation = playerBulidData.allNumber() + 1;
        _playerControl.degreesChange = 1+ 3.1f - _shipRotation * 0.1f;
        //GD.Print("_shipRotation" + _shipRotation);
        _shiled = _shipData.shiled;
        _shipCoefficient = 1f + (float)Math.Log((_shipData.corePower + -_shipData.partPower+ 100f)/(-_shipData.partPower*2f+100f), 2);
        //GD.Print("shipData.power   " + _shipData.power);
        //GD.Print("shipData.partPower   " + _shipData.partPower);
        //GD.Print("_shipCoefficient   " + _speed + _shipCoefficient);
        _playerControl.speed = (float)(GlobalConstant.SHIPBASESPEED + _speed * _shipCoefficient);
        //GD.Print("_playerControl.speed" + _playerControl.speed);
        //_playerControl.speed = 1000f;
        mainRoot().gameScenceIns.updateUIHealthMinMax(0, _health);
        playerBulidData.engineON();
        playerBulidData.checkinterfaceChannel();
        playerBulidData.shipCheckAssembledAgain(playerBulidData.setList());


    }





    public void on_healthchange(float value)
    {
        //Debug.Print("_on_Signal_healthchange");
        if(_invincibleTimer.TimeLeft == 0)
        {
            _health -= (int)value;
            _invincibleTimer.Start(0.5f);
            mainRoot().gameScenceIns.updateUIHealthValue(_health);
            playerBulidData.updateTradFromHealth((float)_health / (float)_shipData.health);
           // GD.Print("_health / _shipData.health", _health , _shipData.health);
            if (_health <= 0)
            {
                mainRoot().gameScenceIns.gameFailed();
            }
        }

    }
    //离我方最近的敌方单位 数量为10
    private List<Vector2> _nearEnemyToPlayerPos = new List<Vector2>();
    public List<Vector2> nearEnemyToPlayerPos
    {
        get
        {
            return _nearEnemyToPlayerPos;
        }
    }
    private List<EnemyScence> _enemylist = new List<EnemyScence>();

    private float _deltatime = 0.0f; //更新频率
    public override void _PhysicsProcess(float delta)
    {
        if (_deltatime < 0.1f)
        {
            _deltatime += delta;
            return;
        }
        

        _deltatime = 0.0f;
        if (mainRoot().gameScenceIns.enemyPool.GetChildCount() > 0)
        {
            if(_enemylist.Count > 0)
            {
                _enemylist.Clear();
            }
            for (int i = 0; i < mainRoot().gameScenceIns.enemyPool.GetChildren().Count; i++)
            {
                _enemylist.Add((EnemyScence)mainRoot().gameScenceIns.enemyPool.GetChildren()[i]);
            }
            _enemylist = _enemylist.OrderBy(x => x.DistanceToPlayer()).ToList();

            _nearEnemyToPlayerPos.Clear();

            for (int i= 0;i<_enemylist.Count;i++)
            {
                _nearEnemyToPlayerPos.Add( _enemylist[i].EnemyPosition());
            }
           
            //Debug.Print("list.First()" + _enemylist.First() + "dis" + _enemylist.First().DistanceToPlayer());
        }

    }

}
