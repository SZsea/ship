using Godot;
using System;
using System.Diagnostics;

public class baseBullet : Area2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.


    protected BulletScence _bulletScenceParents;//父类节点

    public float damage;

    

    public void setBodyParents(BulletScence parents)
    {
        _bulletScenceParents = parents;
    }



    public enum bulletTrackMode
    {
        bulletTrackMode_None, //向目标点直线运动
        bulletTrackMode_Enemy1,//敌人的boss发的子弹1 子弹轨迹1 曲线轨道1
        bulletTrackMode_Enemy2,//敌人的boss发的子弹2 子弹轨迹2 曲线轨道2
        bulletTrackMode_track,//跟踪子弹 子弹轨迹跟着敌方运动 暂时有实现问题
        bulletTrackMode_DesTroyNo,//无法摧毁的子弹 直线运动
        bulletTrackMode_DesTroyNo_Other1,//无法摧毁的子弹 直线运动 速度更快
        bulletTrackMode_AreaDamgae,//子弹命中后造成范围伤害 向目标点直线运动 速度较慢
        bulletTrackMode_None_Other1,//向目标点直线运动 速度更快
        bulletTrackMode_Accelerate_slowly,//先慢速度射出去 然后突然加速
        bulletTrackMode_Accelerate_slowly_Other1,//先慢速度射出去 然后突然加速
        bulletTrackMode_customize

    }


    public void setBulletTrackMode(Vector2 target, float speed, int mode, int number)
    {
        targetPosition = target;
        _uniformSpeed = speed;
        _TeckMode = bulletTrackMode.bulletTrackMode_None;
        if (mode == 20001 || mode == 10002)
        {
            if (number % 2 == 0)
                _TeckMode = bulletTrackMode.bulletTrackMode_Enemy1;
            else
                _TeckMode = bulletTrackMode.bulletTrackMode_Enemy2;

        }

        if (mode == 20010)
            _TeckMode = bulletTrackMode.bulletTrackMode_AreaDamgae;
        if (mode == 20030)
            _TeckMode = bulletTrackMode.bulletTrackMode_DesTroyNo;
        if (mode == 20040)
            _TeckMode = bulletTrackMode.bulletTrackMode_None_Other1;
        if (mode == 20050)
            _TeckMode = bulletTrackMode.bulletTrackMode_DesTroyNo_Other1;
        if (mode == 20060)
            _TeckMode = bulletTrackMode.bulletTrackMode_None;
        if (mode == 20070)
            _TeckMode = bulletTrackMode.bulletTrackMode_DesTroyNo;
        if (mode == 20080)
            _TeckMode = bulletTrackMode.bulletTrackMode_None;
        if (mode == 20090)
            _TeckMode = bulletTrackMode.bulletTrackMode_None;
        if (mode == 20100)
            _TeckMode = bulletTrackMode.bulletTrackMode_None;
        if (mode == 20110)
            _TeckMode = bulletTrackMode.bulletTrackMode_DesTroyNo_Other1;
        if (mode == 20120)
            _TeckMode = bulletTrackMode.bulletTrackMode_DesTroyNo_Other1;
        if (mode == 20130)
            _TeckMode = bulletTrackMode.bulletTrackMode_AreaDamgae;
        if (mode == 20140)
            _TeckMode = bulletTrackMode.bulletTrackMode_None;
        if (mode == 20150)
            _TeckMode = bulletTrackMode.bulletTrackMode_Accelerate_slowly;
        if (mode == 20160)
            _TeckMode = bulletTrackMode.bulletTrackMode_Accelerate_slowly_Other1;


    }


    private Vector2 _targetPosition = Vector2.Zero;
    private Vector2 _direction;
    protected bulletTrackMode _TeckMode;
    protected float _uniformSpeed;

    public Vector2 targetPosition
    {
        set
        {
            _targetPosition = value;
            _direction = _targetPosition - Position;
            LookAt(_targetPosition);
        }
        get
        {
            return _targetPosition;
        }
    }

    private Timer _visibilityTimer; //销毁计时器
    private float _visibilityTime = 5f;
    private bool _isExitedFightArea = false;
    public float lifeTime = 8f;
    public bool _isVisible = true;
    public override void _Ready()
    {
        Connect("body_entered", this, "_on_Node2D_body_entered");

        _visibilityTimer = new Timer();
        AddChild(_visibilityTimer);
        _visibilityTimer.OneShot = false;
        _visibilityTimer.Connect("timeout", this, "CheckOutScreen");
        _visibilityTimer.Autostart = false;
        _visibilityTimer.Start(_visibilityTime);

    }

    public virtual void _on_Node2D_body_entered(Godot.Object area)
    {
        if (GlobalConstant.BUllELTTESTMODE)
        {
            GD.Print("_on_Node2D_body_entered" + this);
        }


    }

    protected void CheckOutScreen()
    {
        if(_isExitedFightArea)
        {
            _bulletScenceParents.QueueFree();
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

    //贝塞尔曲线
    private Vector2 _QuadraticBezierP0;
    private Vector2 _QuadraticBezierP1;
    private Vector2 _QuadraticBezierP2;
    private float _time = -1f;
    private float _bulletTime = 0.3f;
    public override void _PhysicsProcess(float delta)
    {

        lifeTime -= delta;
        if(lifeTime < 0)
            _bulletScenceParents.QueueFree();

        switch (_TeckMode)
        {
            case bulletTrackMode.bulletTrackMode_AreaDamgae:
                {
                    if(_isVisible)
                        Position += _direction.Normalized() * 350f * delta;
                }
                break;
            case bulletTrackMode.bulletTrackMode_None:
                {

                    Position += _direction.Normalized() * 450f * delta;
                }
                break;
            case bulletTrackMode.bulletTrackMode_DesTroyNo:
                {
                    
                    Position += _direction.Normalized() * 500f * delta;
                }
                break;
            case bulletTrackMode.bulletTrackMode_DesTroyNo_Other1:
                {
                    Position += _direction.Normalized() * 800f * delta;
                }
                break;
            case bulletTrackMode.bulletTrackMode_track:
                {
                    Position += _direction.Normalized() * 600f * delta;

                }
                break;

            case bulletTrackMode.bulletTrackMode_None_Other1:
                {
                    Position += _direction.Normalized() * 750f * delta;
                }
                break;
            case bulletTrackMode.bulletTrackMode_Accelerate_slowly:
                {
                    _bulletTime = _bulletTime - delta;
                    if (_bulletTime > 0)
                    {
                        Position += _direction.Normalized() * 300f * delta;
                    }
                    else
                    {
                        Position += _direction.Normalized() * 1000f * delta;
                    }
                    
                }
                break;
            case bulletTrackMode.bulletTrackMode_Accelerate_slowly_Other1:
                {
                    _bulletTime = _bulletTime - delta;
                    if (_bulletTime > 0)
                    {
                        Position += _direction.Normalized() * 100f * delta;
                    }
                    else
                    {
                        Position += _direction.Normalized() * 800f * delta;
                    }
                }
                break;
            case bulletTrackMode.bulletTrackMode_Enemy1:
                {
                    if(_QuadraticBezierP0 == Vector2.Zero)
                        _QuadraticBezierP0 = Position;
                    if (_QuadraticBezierP2 == Vector2.Zero)
                        _QuadraticBezierP2 = _targetPosition;
                    if (_QuadraticBezierP1 == Vector2.Zero)
                    {
                        _QuadraticBezierP1 = (_targetPosition - Position) * 0.7f + Position;
                        Vector2 newVec = (_targetPosition - Position) * 0.7f + Position;
                        double angel = (newVec - Position).Angle() * 180f/Math.PI;                       
                        //GD.Print("angel---------===  "  + angel );
                        float distance = 500f;
                        if ( angel > 90 && angel <=180 )
                        {
                            angel -= 90;
                            angel = 90 - angel;
                            _QuadraticBezierP1.x = _QuadraticBezierP1.x - distance * (float)Math.Sin(angel / 180f * Math.PI);
                            _QuadraticBezierP1.y = _QuadraticBezierP1.y - distance * (float)Math.Cos(angel / 180f * Math.PI);
                            //GD.Print("22222222222----------" + _QuadraticBezierP1 + "+++++++++++" + angel);
                        }
                        else if(angel > 0 && angel <= 90)
                        {
                            angel = 90 - angel;
                            _QuadraticBezierP1.x = _QuadraticBezierP1.x - distance * (float)Math.Cos(angel / 180f * Math.PI);
                            _QuadraticBezierP1.y = _QuadraticBezierP1.y + distance * (float)Math.Sin(angel / 180f * Math.PI);
                           // GD.Print("11111111----------" + _QuadraticBezierP1 + "+++++++++++" + angel);
                        }
                        else if (angel > -90 && angel <= 0)
                        {
                            angel = -angel;
                          
                            _QuadraticBezierP1.x = _QuadraticBezierP1.x + distance * (float)Math.Sin(angel / 180f * Math.PI);
                            _QuadraticBezierP1.y = _QuadraticBezierP1.y + distance * (float)Math.Cos(angel / 180f * Math.PI);
                           // GD.Print("3333333333----------" + _QuadraticBezierP1 + "+++++++++++" + angel);
                        }
                        else if (angel > -180 && angel <= -90)
                        {
                            angel += 90;
                            angel = -angel;

                            _QuadraticBezierP1.x = _QuadraticBezierP1.x + distance * (float)Math.Cos(angel / 180f * Math.PI);
                            _QuadraticBezierP1.y = _QuadraticBezierP1.y - distance * (float)Math.Sin(angel / 180f * Math.PI);
                            //GD.Print("4444444444----------" + _QuadraticBezierP1 + "+++++++++++" + angel);
                        }

                    }

                  
                    if (_time < 0)
                        _time = 0;
                    _time += delta / 3f;

                    Vector2 q0 = _QuadraticBezierP0.LinearInterpolate(_QuadraticBezierP1, _time);
                    Vector2 q1 = _QuadraticBezierP1.LinearInterpolate(_QuadraticBezierP2, _time);
                    Position = q0.LinearInterpolate(q1, _time);
                    LookAt(q0);


                }
                break;
            case bulletTrackMode.bulletTrackMode_Enemy2:
                {
                    if (_QuadraticBezierP0 == Vector2.Zero)
                        _QuadraticBezierP0 = Position;
                    if (_QuadraticBezierP2 == Vector2.Zero)
                        _QuadraticBezierP2 = _targetPosition;
                    if (_QuadraticBezierP1 == Vector2.Zero)
                    {
                        _QuadraticBezierP1 = (_targetPosition - Position) * 0.7f + Position;
                        Vector2 newVec = (_targetPosition - Position) * 0.7f + Position;
                        double angel = (newVec - Position).Angle() * 180f / Math.PI;

                        float distance = 500f;
                        if (angel > 90 && angel <= 180)
                        {
                            angel -= 90;
                            
                            _QuadraticBezierP1.x = _QuadraticBezierP1.x + distance * (float)Math.Cos(angel / 180 * Math.PI);
                            _QuadraticBezierP1.y = _QuadraticBezierP1.y + distance * (float)Math.Sin(angel / 180 * Math.PI);
                        }
                        else if (angel > 0 && angel <= 90)
                        {
                            _QuadraticBezierP1.x = _QuadraticBezierP1.x + distance * (float)Math.Sin(angel / 180 * Math.PI);
                            _QuadraticBezierP1.y = _QuadraticBezierP1.y - distance * (float)Math.Cos(angel / 180 * Math.PI);
                        }
                        else if (angel > -90 && angel <= 0)
                        {
                            angel = -angel;
                            angel = 90 - angel;
                            _QuadraticBezierP1.x = _QuadraticBezierP1.x - distance * (float)Math.Cos(angel / 180 * Math.PI);
                            _QuadraticBezierP1.y = _QuadraticBezierP1.y - distance * (float)Math.Sin(angel / 180 * Math.PI);
                        }
                        else if (angel > -180 && angel <= -90)
                        {
                            angel += 90;
                            angel = -angel;
                            angel = 90 - angel;
                            _QuadraticBezierP1.x = _QuadraticBezierP1.x - distance * (float)Math.Sin(angel / 180 * Math.PI);
                            _QuadraticBezierP1.y = _QuadraticBezierP1.y + distance * (float)Math.Cos(angel / 180 * Math.PI);
                        }

                    }

                    if (_time < 0)
                        _time = 0;
                    _time += delta / 3f;
                    Vector2 q0 = _QuadraticBezierP0.LinearInterpolate(_QuadraticBezierP1, _time);
                    Vector2 q1 = _QuadraticBezierP1.LinearInterpolate(_QuadraticBezierP2, _time);
                    Position = q0.LinearInterpolate(q1, _time);
                    LookAt(q0);


                }
                break;
            case bulletTrackMode.bulletTrackMode_customize:
                {

                }
                break;


        }

    }



}
