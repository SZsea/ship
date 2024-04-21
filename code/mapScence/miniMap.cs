using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class miniMap : ColorRect
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _player = new AnimatedSprite();
        this.AddChild(_player);
        _player.Scale =  new Vector2(1,1);
        _player.Position = this.RectSize / 2;
        SpriteFrames spriteFrames = GD.Load<SpriteFrames>("res://art/spriteFrame/star1.tres");
        _player.Frames = spriteFrames;
        _player.Animation = spriteFrames.GetAnimationNames()[0];
        _player.Playing = true;
    }

    private AnimatedSprite _player;
    private List<EnemyScence> _enemyScenceLsit = new List<EnemyScence>();
    public void updateMap(List<EnemyScence> mapdata)
    {
        if (mapdata.Count < 1)
            return;

        if (_enemyScenceLsit.Count > 0)
        {
            for (int i = 0; i < _enemyScenceLsit.Count; i++)
            {
                if (!IsInstanceValid(_enemyScenceLsit[i]))
                    _enemyScenceLsit.RemoveAt(i);
            }
        }

        for (int i =0;i < mapdata.Count;i++)
        {
            var positon = _player.Position - mapdata[i].DistanceToPlayerV() / GetViewport().Size * this.RectSize  ;

            if (_enemyScenceLsit.Count > 0 && _enemyScenceLsit.Contains(mapdata[i]))
            {
                if (IsInstanceValid(mapdata[i].objectRe))
                {
                    AnimatedSprite enmey = (AnimatedSprite)mapdata[i].objectRe;
                    enmey.Position = positon;
                    if(GlobalConstant.isOutArea(positon,new Vector2(1,1),Vector2.Zero, this.RectSize))
                    {
                        enmey.QueueFree();
                        _enemyScenceLsit.Remove(mapdata[i]);
                    }
                }

            }
            else
            {
                _enemyScenceLsit.Add(mapdata[i]);
                AnimatedSprite enmey = new AnimatedSprite();
                this.AddChild(enmey);
                enmey.Scale = new Vector2(1, 1);
                enmey.Position = positon;
                SpriteFrames spriteFrames = GD.Load<SpriteFrames>("res://art/spriteFrame/star1.tres");
                enmey.Frames = spriteFrames;
                enmey.Animation = spriteFrames.GetAnimationNames()[0];
                enmey.Playing = true;
                mapdata[i].objectRe = enmey;
                if (GlobalConstant.isOutArea(positon, new Vector2(1, 1), Vector2.Zero, this.RectSize))
                {
                    enmey.QueueFree();
                    _enemyScenceLsit.Remove(mapdata[i]);
                }
            }

        }



    }

}
