using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

public class baseEnemy : KinematicBody2D
{
    //怪物通用节点
    [Export]
    public NodePath CollisionShape2DPath;
    [Export]
    public NodePath CollisionPolygon2DPath;
    [Export]
    public NodePath EngineFirePath;
    [Export]
    public NodePath BodyPath;
    [Export]
    public NodePath AnimationPlayerWeakupPath;
    [Export]
    public NodePath DestoryPath;
    [Export]
    public NodePath attackPath;
    [Export]
    public bool attackControlPath;//什么控制射击

    [Export]
    public NodePath[] ShootPositionPath;

    [Export]
    public NodePath showPath;



    private CollisionPolygon2D _collisionPolygon2D;
    private CollisionShape2D _collisionShape2D;
    private AnimatedSprite _engineFire;
    private Node2D _body;
    private AnimationPlayer _animationPlayerWeakup;
    private AnimatedSprite _destory;
    private AnimatedSprite _attack;
    private bool _attackControl = false;
    private VisibilityNotifier2D _show;//屏幕外删除


    public Array<Node2D> _shootPosition = new Array<Node2D>();
    //管理怪物销毁

    public Vector2 _dir = new Vector2(0, 0); //怪物的位置和玩家位置之间的向量
    public EnemyScence _enemyScenceParents;//父类节点

    protected Timer _visibilityTimer; //销毁计时器
    protected float _visibilityTime = 3f;
    protected bool _isExitedFightArea = false;
    protected float _speed = 100f;

    protected bool _alive = true;

    [Export]
    public bool _isBoss;
    protected bool isBoss
    {
        get
        {
            if (_isBoss == true)
            {
                _visibilityTime = 1000f;
                return true;
            }
            else
                return false;
        }

    }
    public override void _Ready()
    {
        _show = (VisibilityNotifier2D)GetNode(showPath);

        if (ShootPositionPath.Length > 0)
        {
            //GD.Print("ShootPositionPath.Length           " + (Node2D)GetNode(ShootPositionPath[0]));
            for(int i =0; i < ShootPositionPath.Length;i++)
            {
                _shootPosition.Add((Node2D)GetNode(ShootPositionPath[i]));
            }
            
        }
        if (AnimationPlayerWeakupPath != null)
        {
            _animationPlayerWeakup = (AnimationPlayer)GetNode(AnimationPlayerWeakupPath);
        }
        if (DestoryPath != null)
        {
            _destory = (AnimatedSprite)GetNode(DestoryPath);
        }
        _body = (Node2D)GetNode(BodyPath);
        _engineFire = (AnimatedSprite)GetNode(EngineFirePath);
        if(CollisionShape2DPath != null)
        {
            _collisionShape2D = (CollisionShape2D)GetNode(CollisionShape2DPath);
        }
       if(CollisionPolygon2DPath != null)
        {
            _collisionPolygon2D = (CollisionPolygon2D)GetNode(CollisionPolygon2DPath);
        }
       if(attackPath != null)
        {
            _attack = (AnimatedSprite)GetNode(attackPath);
        }

        _attackControl = attackControlPath;
        _alive = true;

        _visibilityTimer = new Timer();
        AddChild(_visibilityTimer);
        _visibilityTimer.OneShot = false;
        _visibilityTimer.Connect("timeout", this, "CheckOutScreen");
        _visibilityTimer.Autostart = false;
        //_visibilityTimer.Start(_visibilityTime);       
        _animationPlayerWeakup.Play("weakup");
        _animationPlayerWeakup.Connect("animation_finished", this, "animation_finishedFuction");

        _show.Connect("screen_exited", this, "screen_exited_delete");
    }

    public bool isFinished = false;
    private bool isAttack = false;



    private void screen_exited_delete()
    {
        if(isBoss == false)
        {
            if (IsInstanceValid(_enemyScenceParents))
                _enemyScenceParents.queueFreeAll();

        }
            

    }


    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
            


        if (_attackControl)
        {
            if(isAttack)
            {
                if (attackPath != null)
                {
                    if (_attack.Modulate.a >= 1)
                        return;
                    float newAlph = _attack.Modulate.a;
                    newAlph = newAlph + 0.05f;
                    newAlph = newAlph > 1f ? 1f : newAlph;
                    _attack.Modulate = new Color(1f, 1f, 1f, newAlph);
                   // GD.Print("_attack.Modulate.a" + _attack.Modulate.a);
                }

            }else
            {
                if (attackPath != null)
                {
                    if (_attack.Modulate.a <= 0.1)
                        return;
                    float newAlph = _attack.Modulate.a;
                    newAlph = newAlph - 0.05f;
                    newAlph = newAlph > 1f ? 1f : newAlph;
                    _attack.Modulate = new Color(1f, 1f, 1f, newAlph);
                }
            }

        }

            //_velocity = _velocity.Bounce(collisionInfo.Normal);



    }
    private void animation_finishedFuction(String anim_name)
    {
        if (anim_name != "weakup")
            return;
        isFinished = true;
        if (attackPath != null)
        {
            if(!_attackControl)
            {
                _attack.Modulate = new Color(1f, 1f, 1f, 1f);

            }
        }
            
    }



    public void attack()
    {
        if(attackPath != null)
        {
            isAttack = true;
            _attack.Play();
        }
    }

    public void endAttack()
    {
        if (attackPath != null)
        {
            isAttack = false;
            _attack.Stop();
            _attack.Frame = 0;
        }
    }


    protected void CheckOutScreen()
    {
        if (_isExitedFightArea)
        {
              _enemyScenceParents.queueFreeAll();           
        }

    }




    public void exitedFightArea()
    {
        //GD.Print("enmey exitedFightArea" + this);
        if (_visibilityTimer.IsInsideTree())
        {
            
            _visibilityTimer.Start(_visibilityTime);
            
        }
        _isExitedFightArea = true;

    }
    public void enterFightArea()
    {
        
        // GD.Print("enmey enterFightArea" + this);
        if (_visibilityTimer.IsInsideTree())
        {
            _visibilityTimer.Stop();
            
        }
        _isExitedFightArea = false;
    }







    public void setBodyParents(EnemyScence parents)
    {
        _enemyScenceParents = parents;
    }

    public void setDirToPlayer(Vector2 dir)
    {
        _dir = dir;
    }

    public float distanceToPlayer()
    {
        return _dir.Length();
    }

    public Vector2 distanceToPlayerV()
    {
        return _dir;
    }

        


    public void enemyReciveDamge(float value)
    {
        _enemyScenceParents.on_healthchange((int)value, (Enemy_normal)this);
    }


    public void destroy()
    {
        if(IsInstanceValid(_destory))
        {
            _alive = false;
            if (_animationPlayerWeakup.IsPlaying())
            {
                _animationPlayerWeakup.Stop(false);
            }
            _body.Modulate = new Color(1f, 1f, 1f, 0f);
            _engineFire.Modulate = new Color(1f, 1f, 1f, 0f);
            _destory.Modulate = new Color(1f, 1f, 1f, 1f);
            if (IsInstanceValid(_attack))
            {
                _attack.Modulate = new Color(1f, 1f, 1f, 0f);
            }
            if(IsInstanceValid(_collisionShape2D))
            {
                _collisionShape2D.QueueFree();
            }
            if(IsInstanceValid(_collisionPolygon2D))
            {
                _collisionPolygon2D.QueueFree();
            }
            _destory.Play("destroy");
            if(!_destory.IsConnected("animation_finished", _enemyScenceParents, "queueFreeAll"))
                _destory.Connect("animation_finished", _enemyScenceParents, "queueFreeAll");

        }
    }


    
}


