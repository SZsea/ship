using Godot;
using System;
using static GameData;
using static playerControl;

public class PauseUILayer : CanvasLayer
{


    [Export]
    public NodePath ContinueButtonPath;
    [Export]
    public NodePath returnButtonPath;
    [Export]
    public NodePath LablePath;


    private Button _continueButton;
    private Button _returnButton;
    private Label _label;

    private language _languageNow = language.chinese;

    public language languageNow
    {
        set
        {
            if (value == language.english)
            {
                _continueButton.Text = "close";
                _returnButton.Text = "return";
                _label.Text = "pasue";
            }

            if (value == language.chinese)
            {
                _continueButton.Text = "继续";
                _returnButton.Text = "返回";
                _label.Text = "暂停中";
            }
            _languageNow = value;
        }
        get
        {
            return _languageNow;
        }
    }
    public override void _Ready()
    {
        _continueButton = (Button)GetNode(ContinueButtonPath);
        _returnButton = (Button)GetNode(returnButtonPath);
        _label = (Label)GetNode(LablePath);

        _returnButton.Connect("pressed", this, "returnToMain");
        _continueButton.Connect("pressed", this, "cancel");
    }



    private void returnToMain()
    {
        GetTree().Paused = false;
        Hide();
        MainScence main = (MainScence)GetNode("/root/Main");
        main.CallDeferred("gameOver");
    }



    //全局的暂停

    private bool _isPause = false;

    public void cancel()
    {
        GD.Print("22222222222222222222222" + _isPause);
        if (_isPause)
        {

            Hide();
            GetTree().Paused = false;
            _isPause = false;
        }
        else
        {
            _isPause = true;
            GetTree().Paused = true;
            
            Show();


        }

    }

    public operateMode operateModeUsing = operateMode.keybord;


    public override void _Input(InputEvent @event)
    {
        switch (operateModeUsing)
        {
            case operateMode.keybord:
                {
                    if (Input.IsActionPressed("ui_cancel"))
                    {
                        //GD.Print("pressPasuse");
                        cancel();
                    }
                }
                break;
        }
    }

}
