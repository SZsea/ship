using Godot;
using System;
using System.Collections.Generic;

public class GameMainBg : Control
{
    [Export]
    public NodePath bgLayer1Picture1Path;
    [Export]
    public NodePath bgLayer1Picture2Path;
    [Export]
    public NodePath bgLayer2Picture1Path;
    [Export]
    public NodePath bgLayer2Picture2Path;
    [Export]
    public NodePath corePlayerPath;

    private TextureRect _bgLayerPicture1;
    private TextureRect _bgLayerPicture2;
    private TextureRect _starLayerPicture1;
    private TextureRect _starLayerPicture2;
    private KinematicBody2D _corePlayer;
    public override void _Ready()
    {
        _bgLayerPicture1 = GetNode<TextureRect>(bgLayer1Picture1Path);
        _bgLayerPicture2 = GetNode<TextureRect>(bgLayer1Picture2Path);

        _starLayerPicture1 = GetNode<TextureRect>(bgLayer2Picture1Path);
        _starLayerPicture2 = GetNode<TextureRect>(bgLayer2Picture2Path);

        _corePlayer = GetNode<KinematicBody2D>(corePlayerPath);

    }

    private List<string> BgTextureGroup = new List<string>() { "res://art/backgroud/bg-starfiled-1.png", "res://art/backgroud/Nebula Aqua-Pink.png", "res://art/backgroud/Nebula Blue.png", "res://art/backgroud/Nebula Red.png" };
    private List<string> StarTextureGroup = new List<string>() { "res://art/backgroud/Stars Small_1.png", "res://art/backgroud/Stars Small_2.png", "res://art/backgroud/Stars-Big_1_1_PC.png", "res://art/backgroud/Stars-Big_1_2_PC.png" };

    public int randomA = 99;
    public int randomB = 99;
    public void init()
    {
        if(randomA == 99)
        {
            randomA = (int)(GD.Randf() * (BgTextureGroup.Count - 0.01));
        }
        string bgSource = BgTextureGroup[randomA];
        _bgLayerPicture1.Texture = GD.Load<Texture>(bgSource);
        _bgLayerPicture2.Texture = GD.Load<Texture>(bgSource);

        if (randomB == 99)
        {
            randomB = (int)(GD.Randf() * (StarTextureGroup.Count - 0.01));
        }
        string starSource = StarTextureGroup[randomB];
        _starLayerPicture1.Texture = GD.Load<Texture>(starSource);
        _starLayerPicture2.Texture = GD.Load<Texture>(starSource);

        _bgLayerPicture1.RectPosition = new Vector2(-2098f, -2098f);
        _bgLayerPicture2.RectPosition = new Vector2(_bgLayerPicture1.RectPosition.x, _bgLayerPicture1.RectPosition.y - _bgLayerPicture1.RectSize.y);

        _starLayerPicture1.RectPosition = _bgLayerPicture1.RectPosition;
        _starLayerPicture2.RectPosition = _bgLayerPicture2.RectPosition;
    }

    public void hideShip()
    {
        _corePlayer.Visible = false;
    }

    private float _speed = 1f;

    public override void _Process(float delta)
    {
        _bgLayerPicture1.RectPosition = new Vector2(_bgLayerPicture1.RectPosition.x, _bgLayerPicture1.RectPosition.y + _speed);
        _bgLayerPicture2.RectPosition = new Vector2(_bgLayerPicture2.RectPosition.x, _bgLayerPicture2.RectPosition.y + _speed);

        if (_bgLayerPicture1.RectPosition.y > _bgLayerPicture2.RectPosition.y)
        {
            if (_bgLayerPicture1.RectPosition.y > _bgLayerPicture1.RectSize.y/2)
            {
                _bgLayerPicture1.RectPosition = new Vector2(_bgLayerPicture1.RectPosition.x, _bgLayerPicture2.RectPosition.y - _bgLayerPicture1.RectSize.y);

            }
        }else
        {
            if (_bgLayerPicture2.RectPosition.y > _bgLayerPicture1.RectSize.y/2)
            {
                _bgLayerPicture2.RectPosition = new Vector2(_bgLayerPicture2.RectPosition.x, _bgLayerPicture1.RectPosition.y - _bgLayerPicture2.RectSize.y);
            }
        }

        _starLayerPicture1.RectPosition = new Vector2(_starLayerPicture1.RectPosition.x, _starLayerPicture1.RectPosition.y + _speed/2);
        _starLayerPicture2.RectPosition = new Vector2(_starLayerPicture2.RectPosition.x, _starLayerPicture2.RectPosition.y + _speed/2);

        if (_starLayerPicture1.RectPosition.y > _starLayerPicture2.RectPosition.y)
        {
            if (_starLayerPicture1.RectPosition.y > _starLayerPicture1.RectSize.y / 2)
            {
                _starLayerPicture1.RectPosition = new Vector2(_starLayerPicture1.RectPosition.x, _starLayerPicture2.RectPosition.y - _starLayerPicture1.RectSize.y);

            }
        }
        else
        {
            if (_starLayerPicture2.RectPosition.y > _starLayerPicture1.RectSize.y / 2)
            {
                _starLayerPicture2.RectPosition = new Vector2(_starLayerPicture2.RectPosition.x, _starLayerPicture1.RectPosition.y - _starLayerPicture2.RectSize.y);
            }
        }






    }
}
