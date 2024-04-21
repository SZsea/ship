using Godot;
using System;

public class GameOverLayer : CanvasLayer
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.

    [Export]
    public NodePath continueButtonPath;
    [Export]
    public NodePath backColorPath;
    [Export]
    public NodePath GoldNumberPath;

    private Button _continueButton;
    private ColorRect _backColorRect;
    private Label _goldNumber;
    
    public override void _Ready()
    {
        _continueButton = (Button)GetNode(continueButtonPath);
        _backColorRect = (ColorRect)GetNode(backColorPath);
        _goldNumber = (Label)GetNode(GoldNumberPath);

        _continueButton.Connect("pressed", this.GetParent(), "gameOver");

    }

    //自定义的显示
    public void customizeShow(int gold)
    {
        _isShow = true;
        _goldNumber.Text = gold.ToString();
    }


    private bool _isShow = false;

    private float _backColorAlpth = 0.9f;
    public override void _Process(float delta)
    {
        if(_isShow && _backColorRect.Modulate.a != _backColorAlpth)
        {
            if(!this.Visible)
                this.Show();
            float newAlph = _backColorRect.Modulate.a;
            newAlph = newAlph + 0.05f;
            newAlph = newAlph > _backColorAlpth ? _backColorAlpth : newAlph;
            _backColorRect.Modulate = new Color(1f, 1f, 1f, newAlph);
        }
    }



}
