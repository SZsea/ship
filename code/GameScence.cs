using cfg;
using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Reflection.Emit;

public class GameScence : SZBaseNode2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.

    [Export]
    public PackedScene Player;
    [Export]
    public PackedScene Enemy;
    [Export]
    public NodePath battleNode;
    [Export]
    public NodePath battleGameUI;
    [Export]
    public NodePath PauseUI;
    [Export]
    public NodePath GameOverLayerPath;
    [Export]
    public NodePath VictoryTimerPath;
    [Export]
    public NodePath VictoryColorPath;

    private Node2D _battleNode;

    private PlayerScence _playerScence;
    private StageScence _stageScenece;
    private Node2D _enemyPool;
    private Node2D _bulletPool;
    private GameUILayer _battleGameUI;
    private PauseUILayer _puseUI;
    private GameOverLayer _gameOverLayer;
    private Timer _victoryTimer;
    private ColorRect _victoryColor;

    public PlayerScence playerScence
    {
        get
        {
            if (IsInstanceValid(_playerScence))
            {
                return _playerScence;
            }
            else
            {
                GD.Print("playerScence has been deleted" + _playerScence);
                return null;
            }
        }
    }
    public Node2D bulletPool
    {
        get
        {
            return _bulletPool;
        }
    }

    public Node2D enemyPool
    {
        get
        {
            return _enemyPool;
        }
    }


    public override void _Ready()
    {
        if (GlobalConstant.READYMODE)
        {
            GD.Print("GameScence_Ready()" + this);
        }

        _battleNode = (Node2D)GetNode(battleNode);
        _playerScence = (PlayerScence)Player.Instance();
        _battleGameUI = (GameUILayer)GetNode(battleGameUI);
        _puseUI = (PauseUILayer)GetNode(PauseUI);
        _gameOverLayer = (GameOverLayer)GetNode(GameOverLayerPath);
        _victoryTimer = (Timer)GetNode(VictoryTimerPath);
        _victoryTimer.Connect("timeout", this, "_on_Timer_timeout");
        _victoryColor = (ColorRect)GetNode(VictoryColorPath);

        _enemyPool = new Node2D();
        _bulletPool = new Node2D();
        _battleNode.AddChild(_enemyPool);
        _battleNode.AddChild(_playerScence);
        _battleNode.AddChild(_bulletPool);
        _battleGameUI.thislanuage = gameDate().languageNow;
        _battleGameUI.updateLanguage();

        _playerScence.bulletPool = _bulletPool;
        updateUIGold();
        _puseUI.languageNow = gameDate().languageNow;
    }

    private int _currentLevel;
    private List<int> _monsterList = new List<int>(); //怪物表
    private List<int> _refreshTimeList = new List<int>();//刷新表
    private List<int> _monsterMaxList = new List<int>();//最大数量
    private List<int> _monsterOnceMiniList = new List<int>();//单次刷新最小
    private List<int> _monsterOnceMaxList = new List<int>();//单次刷新最大
    private List<float> _refreshIntervalList = new List<float>();//刷新间隔
    private int _bossId;
    private EnemyScence _boss;
    private int _limitTime;
    private float _timer = 0f; //计时器
    private List<int> _monsterCounter = new List<int>();//计时器
    private List<int> _monsterCounterOnce = new List<int>();//计时器
    private List<float> _refreshCounterList = new List<float>();//计时器
    private List<float> _refreshCounterTwiceList = new List<float>();//计时器
    public void initWithData(baseBulidPart  data,int level)
    {
        _currentLevel = level;
        _playerScence.init(data);
        foreach (var item in cfg.stageLevelInfoCfg.GetAllList())
        {
            if (item.id == _currentLevel)
            {
                _monsterList = item.monsterList;
                _bossId = item.bossId;
                _limitTime = item.limitTime;
                _refreshTimeList = item.refreshTimeList;
                _monsterMaxList = item.monsterMaxList;
                _monsterOnceMiniList = item.monsterOnceMiniList;
                _monsterOnceMaxList = item.monsterOnceMaxList;
                _refreshIntervalList = item.refreshIntervalList;

                var bg = GD.Load<PackedScene>(item.path);
                _stageScenece = (StageScence)bg.Instance();

                //_stageScenece = new StageScence();              
                _battleNode.AddChild(_stageScenece);
                _battleNode.MoveChild(_stageScenece, 0);
                _battleGameUI.updateLevelInfo(item.name);
                //_stageScenece.loadSrouce(item.path);

            }
        }
        for (int a = 0; a < _monsterList.Count; a++)
        {
            _monsterCounter.Add(0);
            _refreshCounterList.Add(0);
            _refreshCounterTwiceList.Add(0);
            _monsterCounterOnce.Add(0);
        }

        _battleGameUI.startTimer(_limitTime);
        
        //GD.Print("_limitTime" + _limitTime);
    }
    private List<EnemyScence> _enemyScenceLsit = new List<EnemyScence>();

    private List<EnemyScence> enemyScenceLsit
    {
        get
        {
            for(int i = 0;i< _enemyScenceLsit.Count; i++ )
            {
                if (!IsInstanceValid(_enemyScenceLsit[i]))
                {
                    _enemyScenceLsit.Remove(_enemyScenceLsit[i]);
                }
            }
            return _enemyScenceLsit;
        }
    }

    private float _refreshMapTime = 0.05f; //场景动态加载时间间隔

    private bool _battleOver = false;
    public override void _PhysicsProcess(float delta)
    {
        if (_battleOver)
        {
            float newAlph = _victoryColor.Modulate.a;
            newAlph = newAlph + 0.01f;
            newAlph = newAlph >= 1f ? 1f : newAlph;
            _victoryColor.Modulate = new Color(1f, 1f, 1f, newAlph);

            return;
        }
            
        base._PhysicsProcess(delta);
        _timer += delta;
        _refreshMapTime += delta;
        if(_refreshMapTime > 0.05f)
        {
            _refreshMapTime = 0;
            //_stageScenece.updateMap(_playerScence.GlobalPosition);//场景动态加载

            _battleGameUI.updateMiniMap(enemyScenceLsit);//小地图刷新

            //是否在战斗区域刷新
            if (playerScence.playerBulidData.GlobalPosition.Abs().x < _stageScenece._fightAreaArea.x || playerScence.playerBulidData.GlobalPosition.Abs().y < _stageScenece._fightAreaArea.y)
            {
                if (playerScence.playerBulidData.GlobalPosition.Length() > 800.0f)
                {
                    var number = (playerScence.playerBulidData.GlobalPosition.Length() - 800.0f) / _stageScenece._fightAreaArea.x;

                    if (number > 1)
                        number = 1;
                    _battleGameUI.updateSignProgress(20f + 80f * (1 - number));
                }

            }
            else
            {
                _battleGameUI.updateSignProgress(20f);
            }

        }





        for (int a =0; a < _monsterList.Count;a++)
        {
            _refreshCounterList[a] += delta;
            _refreshCounterTwiceList[a] += delta;

            if(_timer > _refreshTimeList[a] && _monsterCounter[a] < _monsterMaxList[a])
            {
                if (_refreshCounterList[a] > _refreshIntervalList[a])
                {
                    if (_refreshCounterTwiceList[a] > 0.1)
                    {
                        _refreshCounterTwiceList[a] = 0;
                        EnemyScence enemy = (EnemyScence)Enemy.Instance();                       
                        _enemyPool.AddChild(enemy);
                        _enemyScenceLsit.Add(enemy);
                        enemy.initWithMonsterId(_monsterList[a]);
                        if(_bossId == _monsterList[a])
                        {
                            _boss = enemy;
                        }
                        _monsterCounter[a]++;
                        _monsterCounterOnce[a]++;
                        if (_monsterCounterOnce[a] >= _monsterOnceMiniList[a] && _monsterCounterOnce[a] <= _monsterOnceMaxList[a])
                        {
                            if (GD.RandRange(0, 2) > 1)
                            {
                                _monsterCounterOnce[a] = 0;
                                _refreshCounterList[a] = 0;
                            }
                        }
                        if (_monsterCounterOnce[a] > _monsterOnceMaxList[a])
                        {
                            _monsterCounterOnce[a] = 0;
                            _refreshCounterList[a] = 0;
                        }
                    }

                }
            }

        }


    }






    //游戏胜利 演出
    public void nextLevel()
    {
        _battleOver = true;
        foreach(EnemyScence enemy in _enemyScenceLsit)
        {
            if(!enemy._ownEnemy._isBoss)
                enemy.suicide();
        }
        _victoryTimer.WaitTime = 1.5f;
        _victoryTimer.Start();
        //mainRoot().CallDeferred("startGame", _playerScence.playerBulidData, nextLevl);

    }
    private void _on_Timer_timeout()
    {
        float nextLevl = 0;
        foreach (var item in cfg.stageLevelInfoCfg.GetAllList())
        {
            if (item.id == _currentLevel)
            {
                //打败boss的进入关卡判断
                if (IsInstanceValid(_boss))
                {
                    nextLevl = item.nextLevelV;
                }
                else
                {
                    nextLevl = item.nextLevelF;
                }


                gameDate().goldChange(item.rewardPrice);
                _battleGameUI.updateGoldValue(gameDate().PlayersGold);
            }
        }
        mainRoot().CallDeferred("startBulid", _playerScence.playerBulidData, nextLevl);
    }

    //游戏失败 演出
    public void gameFailed()
    {
        GetTree().Paused = true;
        _gameOverLayer.customizeShow(gameDate().PlayerGoldEver);
        


    }

    public void gameOver()
    {
        GetTree().Paused = false;
        mainRoot().CallDeferred("gameOver");
        
    }


    public void updateUIHealthMinMax(float min,float max)
    {
        //Debug.Print("updateUIHealthMinMax" + _battleGameUI);
        _battleGameUI.updateHealthMinMax(min, max);
    }

    public void updateUIHealthValue(float value)
    {
        if(!_battleOver)
            _battleGameUI.updateHealthCurrentValue(value);

    }


    public void updateUIGold()
    {
        _battleGameUI.updateGoldValue(gameDate().PlayersGold);
    }



}
