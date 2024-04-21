using Godot;
using System;
using System.Data;
using System.Diagnostics;
using System.Collections.Generic;

public class MainScence : SZBaseNode2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    //测试属性



    [Export]
    public PackedScene PackedMainMenuScence;
    [Export]
    public PackedScene PackedGameScence;
    [Export]
    public PackedScene PackedShipBulidScence;
    [Export]
    public PackedScene ProgressBuildScence;
    [Export]
    public NodePath data;
    [Export]
    public NodePath ViewRoot;
    [Export]
    public NodePath ChangeViewPath;


    private Node2D _viewRoot;
    private MainMenuScence _mainMenuScenceIns;
    private GameScence _gameScenceIns;
    private ShipBulidScence _shipBulidScence;
    private ProgressScence _progressScence;
    private ScenceManager _scenceManager;



    public override void _Ready()
    {

        if (GlobalConstant.READYMODE)
        {
            GD.Print("MainScence_Ready()" + this);
        }
        gameDate().LoadGame();
        gameDate().LoadForeverData();
        GD.Randomize();
        _viewRoot = (Node2D)GetNode(ViewRoot);
        _scenceManager = (ScenceManager)GetNode(ChangeViewPath);
        _mainMenuScenceIns = (MainMenuScence)PackedMainMenuScence.Instance();
        _viewRoot.AddChild(_mainMenuScenceIns);
        //gameDate().clearData();
        
        
    }



    public override void _PhysicsProcess(float delta)
    {

    }

    //默认初始关卡为101  测试关卡为1001
    private int _nextLevel = 101;


    private int _lastLevel = 601;

    private void startBulid(baseBulidPart playerBulidData, int nextLevl)
    {
        if(IsInstanceValid(playerBulidData))
            if (playerBulidData.GetParent() != gameDate())
                gameDate().playerBulidData = playerBulidData.copy();
        _nextLevel = nextLevl;
        gameDate().playerEverPlayedLevel = _nextLevel;

        _shipBulidScence = (ShipBulidScence)PackedShipBulidScence.Instance();
        _viewRoot.AddChild(_shipBulidScence);
        _shipBulidScence._bg.randomA = _mainMenuScenceIns._bg.randomA;
        _shipBulidScence._bg.randomB = _mainMenuScenceIns._bg.randomB;
        _shipBulidScence._bg.init();//初始化背景 与前面一个背景统一

        _shipBulidScence.LevelIndex = _nextLevel;
        if(_lastLevel == _nextLevel)
        {
            gameDate().playerLastPlayerLevel++;
        }

        if (IsInstanceValid(_mainMenuScenceIns))
            _mainMenuScenceIns.QueueFree();
        if (IsInstanceValid(_gameScenceIns))
            _gameScenceIns.QueueFree();
        //Debug.Print("startBulid(baseBulidPart playerBulidData, int nextLevl)");
    }

    private void startBulid()
    {
        _scenceManager.initWith();

        if (IsInstanceValid(gameDate().playerBulidData))
            gameDate().playerBulidData.QueueFree();
        gameDate().playerUsingPaced.Clear();
        gameDate().playerBulidPacked.Clear();
        gameDate().clearData();
        gameDate().resumeGold();

        _shipBulidScence = (ShipBulidScence)PackedShipBulidScence.Instance();
        _viewRoot.AddChild(_shipBulidScence);
        _shipBulidScence._bg.randomA = _mainMenuScenceIns._bg.randomA;
        _shipBulidScence._bg.randomB = _mainMenuScenceIns._bg.randomB;
        _shipBulidScence._bg.init();//初始化背景 与前面一个背景统一

        
        _shipBulidScence.LevelIndex = _nextLevel;
        gameDate().playerEverPlayedLevel = _nextLevel;


        if (IsInstanceValid(_mainMenuScenceIns))
            _mainMenuScenceIns.QueueFree();
        if (IsInstanceValid(_gameScenceIns))
            _gameScenceIns.QueueFree();
        //Debug.Print("startBulid()");
    }

    private void continueBulid()
    {
        _nextLevel = gameDate().playerEverPlayedLevel;

        _shipBulidScence = (ShipBulidScence)PackedShipBulidScence.Instance();
        _viewRoot.AddChild(_shipBulidScence);

        

        if (IsInstanceValid(_mainMenuScenceIns))
            _mainMenuScenceIns.QueueFree();
        if (IsInstanceValid(_gameScenceIns))
            _gameScenceIns.QueueFree();
    }


    private void startGame(baseBulidPart playerBulidData)
    {

        _scenceManager.initWith();

        gameDate().SaveGame();
        if (playerBulidData.GetParent() != gameDate())
            gameDate().playerBulidData = playerBulidData.copy();
        if (IsInstanceValid(_gameScenceIns))
            _gameScenceIns.QueueFree();
        if (_nextLevel == 0)
        {
            _mainMenuScenceIns = (MainMenuScence)PackedMainMenuScence.Instance();
            _viewRoot.AddChild(_mainMenuScenceIns);
        }else
        {
            _gameScenceIns = (GameScence)PackedGameScence.Instance();
            _viewRoot.AddChild(_gameScenceIns);
            _gameScenceIns.initWithData(gameDate().playerBulidData, _nextLevel);
        }

        if (IsInstanceValid(_shipBulidScence))
            _shipBulidScence.QueueFree();
        if (IsInstanceValid(gameDate().playerBulidData))
            gameDate().playerBulidData.QueueFree();

    }



    private void gameOver()
    {

        _gameScenceIns.QueueFree();
        _mainMenuScenceIns = (MainMenuScence)PackedMainMenuScence.Instance();
        _viewRoot.AddChild(_mainMenuScenceIns);
        if (IsInstanceValid(gameDate().playerBulidData))
            gameDate().playerBulidData.QueueFree();
        gameDate().playerUsingPaced.Clear();
        gameDate().clearData();
        gameDate().resumeGold();
    }



    public GameScence gameScenceIns
    {
        get
        {
            if (IsInstanceValid(_gameScenceIns))
            {
                return _gameScenceIns;
            }
            else
            {
                GD.Print("_gameScence has been deleted");
                return null;
            }
        }
    }


    private void detailsGame()
    {
        _mainMenuScenceIns.hideButton();
        _progressScence = (ProgressScence)ProgressBuildScence.Instance();
        _progressScence.thislanguage = gameDate().languageNow;
        _progressScence.fatherScence = this;
        _progressScence.gameData = gameDate();
        
        _viewRoot.AddChild(_progressScence);
        if (gameDate().languageNow == GameData.language.english)
        {
            foreach (var cfg_A in cfg.translateCfg.GetAllList())
            {
                if (cfg_A.id == 119)
                    _progressScence.setquitButtonText(cfg_A.english);
            }

        }
        else
        {
            foreach (var cfg_A in cfg.translateCfg.GetAllList())
            {
                if (cfg_A.id == 119)
                    _progressScence.setquitButtonText(cfg_A.chinese);

            }
        }
        _progressScence.show();
    }

    private void quitDetailsGame()
    {
        _mainMenuScenceIns.showButton();
        _progressScence.QueueFree();

    }

    private void Changelanguage()
    {

    }
}
