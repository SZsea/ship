using cfg;
using Godot;
using System;
using System.Diagnostics;

public class MainMenuScence : SZBaseNode2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.

    [Export]
    public NodePath Camera;
    [Export]
    public NodePath startButton;
    [Export]
    public NodePath settingButton;
    [Export]
    public NodePath detailsButton;
    [Export]
    public NodePath quitButton;
    [Export]
    public NodePath selfButton;
    [Export]
    public NodePath continueButton;
    [Export]
    public NodePath bgPath;

    private Camera2D _camera;
    private Button _startButton;
    private Button _settingButton;
    private Button _detailsButton;
    private Button _quitButton;
    private Button _selfButton;
    private Button _continueButton;
    public GameMainBg _bg;
    public override void _Ready()
    {
        base._Ready();
        if (GlobalConstant.READYMODE)
        {
            GD.Print("MainMenuScence_Ready()" + this);
        }
        _camera = (Camera2D)GetNode(Camera);
        _camera.Current = true;

        _startButton = (Button)GetNode(startButton);
        _settingButton = (Button)GetNode(settingButton);
        _detailsButton = (Button)GetNode(detailsButton);
        _quitButton = (Button)GetNode(quitButton);
        _selfButton = (Button)GetNode(selfButton);
        _continueButton = (Button)GetNode(continueButton);
        _bg = GetNode<GameMainBg>(bgPath);
        _bg.init();

        _startButton.Connect("pressed", this, "startGame");
        _settingButton.Connect("pressed", this, "settingGame");
        _detailsButton.Connect("pressed", this, "detailsGame");
        _quitButton.Connect("pressed", this, "quitGame");
        _selfButton.Connect("pressed", this, "selfGame");
        _continueButton.Connect("pressed", this, "continueGame");

        //多语言修改
        setLanguage();

        if (gameDate().playerBulidData == null)
        {
            _continueButton.Disabled = true;
            //GD.Print("1111111111");
        }else
        {
            _continueButton.Disabled = false;
            //GD.Print("111111111122222");
        }
    }

    private void setLanguage()
    {
        if (gameDate().languageNow == GameData.language.english)
        {
            foreach (var cfg_A in cfg.translateCfg.GetAllList())
            {
                if(cfg_A.id == 101)
                    _continueButton.Text = cfg_A.english;
                if (cfg_A.id == 103)
                    _settingButton.Text = cfg_A.english;
                if (cfg_A.id == 104)
                    _detailsButton.Text = cfg_A.english;
                if (cfg_A.id == 105)
                    _quitButton.Text = cfg_A.english;
                if (cfg_A.id == 106)
                    _selfButton.Text = cfg_A.english;
                if (cfg_A.id == 102)
                    _startButton.Text = cfg_A.english;

            }
                
        }
        else
        {
            foreach (var cfg_A in cfg.translateCfg.GetAllList())
            {
                if (cfg_A.id == 101)
                    _continueButton.Text = cfg_A.chinese;
                if (cfg_A.id == 103)
                    _settingButton.Text = cfg_A.chinese;
                if (cfg_A.id == 104)
                    _detailsButton.Text = cfg_A.chinese;
                if (cfg_A.id == 105)
                    _quitButton.Text = cfg_A.chinese;
                if (cfg_A.id == 106)
                    _selfButton.Text = cfg_A.chinese;
                if (cfg_A.id == 102)
                    _startButton.Text = cfg_A.chinese;

            }
        }

    }



    //_startButton 开始游戏


    private void continueGame()
    {
        mainRoot().CallDeferred("continueBulid");
    }


    private void startGame()
    {
        
        mainRoot().CallDeferred("startBulid");
    
    }

    private void settingGame()
    {
        //mainRoot().CallDeferred("language");
        if(gameDate().languageNow == GameData.language.english)
        {
            gameDate().languageNow = GameData.language.chinese;
        }else
        {
            gameDate().languageNow = GameData.language.english;
        }
        setLanguage();
    }

    private void detailsGame()
    {
        mainRoot().CallDeferred("detailsGame");
    }
    private void quitGame()
    {
        GetTree().Quit();
    }

    //所有按钮隐藏
    public void hideButton()
    {
        _startButton.Hide();
        _settingButton.Hide();
        _detailsButton.Hide();
        _quitButton.Hide();
        _continueButton.Hide();
    }

    //所有按钮显示
    public void showButton()
    {
        _startButton.Show();
        _settingButton.Show();
        _detailsButton.Show();
        _quitButton.Show();
        _continueButton.Show();
    }

    public void selfGame()
    {

    }
}
