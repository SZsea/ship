using Godot;
using System;

public class ProgressScence : CanvasLayer
{


    [Export]
    public NodePath quitButtonPath;
    [Export]
    public NodePath scrollContainer;
    [Export]
    public NodePath detailContainer;

    private Button _quitButton;
    private Progress_ScrollContainer _ScrollContainer;
    private bulidUIScroll _detailScroll;

    public MainScence fatherScence;
    public GameData gameData;

    public GameData.language thislanguage;
    public override void _Ready()
    {
        base._Ready();
        _quitButton = (Button)GetNode(quitButtonPath);
        _quitButton.Connect("pressed", this, "_quitProgress");
        _ScrollContainer = (Progress_ScrollContainer)GetNode(scrollContainer);
        _ScrollContainer.fatherView = this;
        //_quitButton.Text = "退出";
        _detailScroll = (bulidUIScroll)GetNode(detailContainer);


    }

    public void setquitButtonText(string text)
    {
        GD.Print(text);
        _quitButton.Text = text;
    }

    public void show()
    {
        _ScrollContainer.initWithData(cfg.playerCoreCfg.GetAllList(), gameData.unlockedPackedForever);
        
    }


    private void _quitProgress()
    {
        fatherScence.CallDeferred("quitDetailsGame");
    }

    public void updateinformation(int number)
    {
        foreach(cfg.playerCoreCfg palyer in cfg.playerCoreCfg.GetAllList())
        {
            if(palyer.id == number)
            {
                _detailScroll.updateWithData(palyer, thislanguage);
            }


        }
        
    }
}
