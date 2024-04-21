using Godot;
using System;
using System.Collections.Generic;

public class player_bullet : baseBullet
{
    [Export]
    public NodePath AreaDamageArea;
    [Export]
    public NodePath BoomEffectAnimateNodePath;
    [Export]
    public NodePath AnimaterdSpiritNodePath;

    private Area2D _areaDamageArea;
    private AnimatedSprite _boomEffectAnimate;
    private AnimatedSprite _animaterdSpirit;
    private List<Godot.Object> _areaDamageList = new List<Godot.Object>();
    public override void _Ready()
    {
        base._Ready();
        if(AnimaterdSpiritNodePath != null)
        {
            _animaterdSpirit = GetNode<AnimatedSprite>(AnimaterdSpiritNodePath);
        }      
        if (AreaDamageArea != null)
        {
            GD.Print("!AreaDamageArea");
            _areaDamageArea = GetNode<Area2D>(AreaDamageArea);
            _areaDamageArea.Connect("body_entered", this, "_areaDamageListAdd");
            _areaDamageArea.Connect("body_exited", this, "_areaDamageListRemove");
            _boomEffectAnimate = GetNode<AnimatedSprite>(BoomEffectAnimateNodePath);
            _boomEffectAnimate.Connect("animation_finished", this, "_BulletqueueFree");
            // _boomEffectAnimate.Visible = true;
        }


    }

    public void _areaDamageListAdd(Godot.Object area)
    {
        _areaDamageList.Add(area);
    }

    public void _areaDamageListRemove(Godot.Object area)
    {
        if(_areaDamageList.Contains(area))
        {
            _areaDamageList.Remove(area);
        }
    }
    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
    public override void _on_Node2D_body_entered(Godot.Object area)
    {
        base._on_Node2D_body_entered(area);
        if (area.HasMethod("enemyReciveDamge")&& (bool)area.Get("_alive"))
        {
           
            switch (_TeckMode)
            {
                case bulletTrackMode.bulletTrackMode_DesTroyNo_Other1:
                case bulletTrackMode.bulletTrackMode_DesTroyNo:
                    {
                        area.Call("enemyReciveDamge", damage);//触碰到的伤害
                    }
                    break;
                case bulletTrackMode.bulletTrackMode_AreaDamgae:
                    {
                        if (IsInstanceValid(_areaDamageArea))
                        {
                            if (_areaDamageList.Count > 0)
                            {
                                _boomEffectAnimate.Visible = true;
                                _boomEffectAnimate.Play();
                                _isVisible = false;
                                _animaterdSpirit.Visible = false;
                                //GD.Print("_areaDamageList.Count" + _areaDamageList.Count);
                                foreach (Godot.Object i in _areaDamageList)
                                {
                                    i.Call("enemyReciveDamge", damage);//触碰到的伤害
                                }
                            }

                        }
                    }
                    break;
                default:
                    {
                        area.Call("enemyReciveDamge", damage);//触碰到的伤害
                        //GD.Print("area.Call(\"enemyReciveDamge\", damage)");
                        _BulletqueueFree();
                    }
                    break;
            }
            
        }
    }

    public void _BulletqueueFree()
    {

        _bulletScenceParents.QueueFree();
    }


    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);

    }




}
