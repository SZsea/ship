using Godot;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
public class GameUILayer : CanvasLayer
{

    [Export]
    public NodePath HealthprogressBarPath;
    [Export]
    public NodePath LevelLeftTimeTextPath;
    [Export]
    public NodePath LevelLeftTimeTimerPath;
    [Export]
    public NodePath ProgressBarLabelPath;
    [Export]
    public NodePath GoldValuePath;
    [Export]
    public NodePath MiniMapPath;
    [Export]
    public NodePath levelPath;
    [Export]
    public NodePath SiginPath;
    [Export]
    public NodePath lowHeathPath;
    [Export]
    public NodePath SignLabelPath;


    private ProgressBar _healthprogressBar;
    private Label _levelLeftTimeText;
    private Timer _levelLeftTimeTimer;
    private Timer _temporaryTimer;
    private Label _progressBarLabel;
    private Label _GloadValueLabel;
    private miniMap _miniMap;
    private Label _level;
    private TextureProgress _sign;
    private LowHeathControl _lowHealth;
    private Label _signLabel;

    public GameData.language thislanuage = GameData.language.chinese;


    public override void _Ready()
    {
        if (GlobalConstant.READYMODE)
        {
            GD.Print("GameUILayer_Ready" + this);
        }
        _healthprogressBar = (ProgressBar)GetNode(HealthprogressBarPath);
        _levelLeftTimeText = (Label)GetNode(LevelLeftTimeTextPath);
        _levelLeftTimeTimer = (Timer)GetNode(LevelLeftTimeTimerPath);
        _progressBarLabel = (Label)GetNode(ProgressBarLabelPath);
        _GloadValueLabel = (Label)GetNode(GoldValuePath);
        _miniMap = (miniMap)GetNode(MiniMapPath);
        _level = (Label)GetNode(levelPath);
        _sign = (TextureProgress)GetNode(SiginPath);
        _lowHealth = (LowHeathControl)GetNode(lowHeathPath);
        _lowHealth.init();

        _levelLeftTimeTimer.Connect("timeout", this, "_on_Timer_timeout");
        _temporaryTimer = new Timer();
        _temporaryTimer.OneShot = true;
        _temporaryTimer.Connect("timeout", this, "updateTimeText");
        _temporaryTimer.Autostart = false;
        AddChild(_temporaryTimer);
        _signLabel = (Label)GetNode(SignLabelPath);
       // _levelLeftTimeTimer.Start(60f);

    }
    public void updateLanguage()
    {
        if (thislanuage == GameData.language.chinese)
        {
            _signLabel.Text = "信号";
        }
        else
        {
            _signLabel.Text = "Signal";
        }
    }

    //更新飞船那个信号
    public void updateSignProgress(float number)
    {
        byte g = (byte)(number / 100.0f * 255.0f);
        byte b = (byte)(number / 100.0f * 255.0f);
        
        _sign.TintProgress = Color.Color8(255, g, b);

       // GD.Print("_sign.TintProgress" + _sign.TintProgress);

        _sign.Value = number;
    }



    public void updateLevelInfo(string text)
    {
        _level.Text = text;
    }

    //初始值 最大值
    public void updateHealthMinMax(float mini,float Max)
    {
        _healthprogressBar.MinValue = mini;
        _healthprogressBar.MaxValue = Max;
        _healthprogressBar.Value = Max;
        _progressBarLabel.Text = _healthprogressBar.MaxValue + "/" + _healthprogressBar.MaxValue;
    }

    //现有值
    public void updateHealthCurrentValue(float value)
    {
        _healthprogressBar.Value = value;
        _progressBarLabel.Text = value + "/" + _healthprogressBar.MaxValue;

        if (value / _healthprogressBar.MaxValue <= 0.5f)
        {
            if (value / _healthprogressBar.MaxValue <= 0.2f)
            {
                _lowHealth.veryLowHealth();
            }
            else
            {
                _lowHealth.lowHeath();
            }
        }
            

    }


    public void startTimer(float time)
    {
        _levelLeftTimeTimer.Start(time);
        _temporaryTimer.Start(0.1f);
    }

    private void _on_Timer_timeout()
    {
        if(_healthprogressBar.Value > 0)
        {
            GameScence gameScence = (GameScence)GetParent();
            gameScence.nextLevel();
            //GD.Print("gameScence.nextLevel()" + GetParent());
        }
    }

    private void updateTimeText()
    {
        int a = (int)(_levelLeftTimeTimer.TimeLeft * 10);
        float b = a / 10f;
        if(thislanuage == GameData.language.chinese)
        {
            _levelLeftTimeText.Text = b.ToString() + "秒";
        }else
        {
            _levelLeftTimeText.Text = b.ToString() + "sec";
        }
        
        _temporaryTimer.Start(0.1f);
    }

    public void updateGoldValue(int number)
    {
        if (thislanuage == GameData.language.chinese)
        {
            _GloadValueLabel.Text = number.ToString() + "  金币";
        }
        else
        {
            _GloadValueLabel.Text = number.ToString() + "  gold";
        }
        
    }

    public void updateMiniMap(List<EnemyScence> mapdata)
    {
        _miniMap.updateMap(mapdata);
    }



}
