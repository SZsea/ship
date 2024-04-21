using Godot;
using System;
using System.Diagnostics;
public class BulidUILayer : CanvasLayer
{

    [Export]
    public NodePath backGroudPath;
    [Export]
    public NodePath ScrollContainerPath;
    [Export]
    public NodePath GoldTextPath;

    private Panel _backGroud;
    private bulidUIScroll _ScrollContainer;
    private Label _goldText;
    public override void _Ready()
    {
        if (GlobalConstant.READYMODE)
        {
            GD.Print("BulidUILayer" + this);
        }

        _backGroud = GetNode<Panel>(backGroudPath);
        _ScrollContainer = GetNode<bulidUIScroll>(ScrollContainerPath);
        _goldText = GetNode<Label>(GoldTextPath);

    }

    public GameData.language thislanuage = GameData.language.chinese;
    public void updateWithData(baseBulidPart data, GameData.language thislanguage)
    {
        _ScrollContainer.updateWithData(data, thislanguage);
        thislanuage = thislanguage;
    }

    public void updateGold(int number)
    {
        if(thislanuage == GameData.language.chinese)
            _goldText.Text = "金币:" + number.ToString();
        if (thislanuage == GameData.language.english)
            _goldText.Text = "gold:" + number.ToString();


    }
}
