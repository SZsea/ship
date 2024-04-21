using Godot;
using Godot.Collections;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using static baseDerivedPart;

public  class baseCoreEngine : baseBulidPart
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.

    [Export]
    public NodePath engine1_on_animatePath;
    [Export]
    public NodePath engine2_on_animatePath;
    [Export]
    public NodePath engine3_on_animatePath;
    [Export]
    public NodePath engine4_on_animatePath;
    [Export]
    public NodePath engine5_on_animatePath;
    [Export]
    public NodePath engine6_on_animatePath;
    [Export]
    public NodePath engine1_pic_DamagedPath;
    [Export]
    public NodePath engine1_pic_SlightDamagedPath;
    [Export]
    public NodePath engine1_pic_VeryDamagedPath;

    [Export]
    public NodePath engine_effect1Path;
    [Export]
    public NodePath engine_effect2Path;


    [Export]
    public NodePath engine7_on_animatePath;

    [Export]
    public NodePath weapon_baseweaponPath;

    public AnimatedSprite weapon_baseweapon
    {
        get
        {
            if (weapon_baseweaponPath != null)
                return (AnimatedSprite)GetNode(weapon_baseweaponPath);
            else
                return null;
        }
    }

    [Export]
    public NodePath[] ShootPositionPath;

    public Array<Node2D> _shootPosition = new Array<Node2D>();




    public AnimatedSprite engine7_on_animate
    {
        get
        {
            if (engine7_on_animatePath != null)
                return (AnimatedSprite)GetNode(engine7_on_animatePath);
            else
                return null;
        }
    }
    [Export]
    public NodePath engine8_on_animatePath;

    public AnimatedSprite engine8_on_animate
    {
        get
        {
            if (engine8_on_animatePath != null)
                return (AnimatedSprite)GetNode(engine8_on_animatePath);
            else
                return null;
        }
    }
    [Export]
    public NodePath engine9_on_animatePath;

    public AnimatedSprite engine9_on_animate
    {
        get
        {
            if (engine9_on_animatePath != null)
                return (AnimatedSprite)GetNode(engine9_on_animatePath);
            else
                return null;
        }
    }
    [Export]
    public NodePath engine10_on_animatePath;

    public AnimatedSprite engine10_on_animate
    {
        get
        {
            if (engine10_on_animatePath != null)
                return (AnimatedSprite)GetNode(engine10_on_animatePath);
            else
                return null;
        }
    }
    [Export]
    public NodePath engine11_on_animatePath;

    public AnimatedSprite engine11_on_animate
    {
        get
        {
            if (engine11_on_animatePath != null)
                return (AnimatedSprite)GetNode(engine11_on_animatePath);
            else
                return null;
        }
    }
    [Export]
    public NodePath engine12_on_animatePath;
    public AnimatedSprite engine12_on_animate
    {
        get
        {
            if (engine12_on_animatePath != null)
                return (AnimatedSprite)GetNode(engine12_on_animatePath);
            else
                return null;
        }
    }

    public Particles2D engine_effect1
    {
        get
        {
            if (engine_effect1Path != null)
                return (Particles2D)GetNode(engine_effect1Path);
            else
                return null;
        }
    }
    public Particles2D engine_effect2
    {
        get
        {
            if (engine_effect2Path != null)
                return (Particles2D)GetNode(engine_effect2Path);
            else
                return null;
        }
    }


    public AnimatedSprite engine1_on_animate
    {
        get
        {
            if (engine1_on_animatePath != null)
                return (AnimatedSprite)GetNode(engine1_on_animatePath);
            else
                return null;
        }
    }
    public AnimatedSprite engine2_on_animate
    {
        get
        {
            if (engine2_on_animatePath != null)
                return (AnimatedSprite)GetNode(engine2_on_animatePath);
            else
                return null;
        }
    }
    public AnimatedSprite engine3_on_animate
    {
        get
        {
            if (engine3_on_animatePath != null)
                return (AnimatedSprite)GetNode(engine3_on_animatePath);
            else
                return null;
        }
    }
    public AnimatedSprite engine4_on_animate
    {
        get
        {
            if (engine4_on_animatePath != null)
                return (AnimatedSprite)GetNode(engine4_on_animatePath);
            else
                return null;
        }
    }
    public AnimatedSprite engine5_on_animate
    {
        get
        {
            if (engine5_on_animatePath != null)
                return (AnimatedSprite)GetNode(engine5_on_animatePath);
            else
                return null;
        }
    }
    public AnimatedSprite engine6_on_animate
    {
        get
        {
            if (engine6_on_animatePath != null)
                return (AnimatedSprite)GetNode(engine6_on_animatePath);
            else
                return null;
        }
    }
    public Sprite engine1_pic_Damaged
    {
        get
        {
            if (engine1_pic_DamagedPath != null)
                return (Sprite)GetNode(engine1_pic_DamagedPath);
            else
                return null;
        }
    }
    public Sprite engine1_pic_SlightDamaged
    {
        get
        {
            if (engine1_pic_SlightDamagedPath != null)
                return (Sprite)GetNode(engine1_pic_SlightDamagedPath);
            else
                return null;
        }
    }
    public Sprite engine1_pic_VeryDamaged
    {
        get
        {
            if (engine1_pic_VeryDamagedPath != null)
                return (Sprite)GetNode(engine1_pic_VeryDamagedPath);
            else
                return null;
        }
    }

    private float _reloadTime = 2f;//重装填时间
    private float _pseudo_timer = 0; //计时
    protected virtual void shoot()
    {
        weapon_baseweapon.Play("fire");

    }


    

    public override void _Ready()
    {
        base._Ready();


    }


    public override void playerReciveDamge(float value)
    {
        base.playerReciveDamge(value);
    }

    private float _alphChange = 0.1f;
    public override void _PhysicsProcess(float delta)
    {
        //_velocity = _velocity.Bounce(collisionInfo.Normal);
        

        base._PhysicsProcess(delta);
        if (ShootPositionPath != null && ShootPositionPath.Length > 0 && _shootPosition.Count == 0)
        {
            //GD.Print("ShootPositionPath.Length" + ShootPositionPath.Length);
            for (int i = 0; i < ShootPositionPath.Length; i++)
            {
                _shootPosition.Add((Node2D)GetNode(ShootPositionPath[i]));
            }
        }


        if (_isExitedFightArea)
        {
            _isExitedTime += delta;
            if(_isExitedTime > 1f)
            {
                playerReciveDamge(_exitedFightAreaDamge);
                if (_exitedFightAreaDamge < 10f)
                    _exitedFightAreaDamge++;
                _isExitedTime = 0f;
            }
        }
        if (IsInstanceValid(engine1_on_animate)&& engine1_on_animate.Playing&& engine1_on_animate.Modulate.a < 1f)
        {
            float newAlph = engine1_on_animate.Modulate.a;
            newAlph = newAlph + _alphChange;
            newAlph = newAlph > 1f ? 1f : newAlph;
            if (IsInstanceValid(engine7_on_animate))
                engine7_on_animate.Modulate = new Color(1f, 1f, 1f, newAlph);            
            engine1_on_animate.Modulate = new Color(1f, 1f, 1f, newAlph);
        }
        if (IsInstanceValid(engine2_on_animate) && engine2_on_animate.Playing && engine2_on_animate.Modulate.a < 1f)
        {
            float newAlph = engine2_on_animate.Modulate.a;
            newAlph = newAlph + _alphChange;
            newAlph = newAlph > 1f ? 1f : newAlph;
            if (IsInstanceValid(engine8_on_animate))
                engine8_on_animate.Modulate = new Color(1f, 1f, 1f, newAlph);
            engine2_on_animate.Modulate = new Color(1f, 1f, 1f, newAlph);
        }
        if (IsInstanceValid(engine3_on_animate) && engine3_on_animate.Playing && engine3_on_animate.Modulate.a < 1f)
        {
            float newAlph = engine3_on_animate.Modulate.a;
            
            newAlph = newAlph + _alphChange;
            newAlph = newAlph > 1f ? 1f : newAlph;
            if (IsInstanceValid(engine9_on_animate))
                engine9_on_animate.Modulate = new Color(1f, 1f, 1f, newAlph);
            engine3_on_animate.Modulate = new Color(1f, 1f, 1f, newAlph);

        }
        if (IsInstanceValid(engine4_on_animate) && engine4_on_animate.Playing && engine4_on_animate.Modulate.a < 1f)
        {
            float newAlph = engine4_on_animate.Modulate.a;
            newAlph = newAlph + _alphChange;
            newAlph = newAlph > 1f ? 1f : newAlph;
            //GD.Print("engine4_on_animate.Modulate" + engine4_on_animate.Modulate);
            if (IsInstanceValid(engine10_on_animate))
                engine10_on_animate.Modulate = new Color(1f, 1f, 1f, newAlph);
            engine4_on_animate.Modulate = new Color(1f, 1f, 1f, newAlph);
        }
        if (IsInstanceValid(engine5_on_animate) && engine5_on_animate.Playing && engine5_on_animate.Modulate.a < 1f)
        {
            float newAlph = engine5_on_animate.Modulate.a;
            newAlph = newAlph + _alphChange;
            newAlph = newAlph > 1f ? 1f : newAlph;
            if (IsInstanceValid(engine11_on_animate))
                engine11_on_animate.Modulate = new Color(1f, 1f, 1f, newAlph);
            engine5_on_animate.Modulate = new Color(1f, 1f, 1f, newAlph);
        }
        if (IsInstanceValid(engine6_on_animate) && engine6_on_animate.Playing && engine6_on_animate.Modulate.a < 1f)
        {
            float newAlph = engine6_on_animate.Modulate.a;
            newAlph = newAlph + _alphChange;
            newAlph = newAlph > 1f ? 1f : newAlph;
            if (IsInstanceValid(engine12_on_animate))
                engine12_on_animate.Modulate = new Color(1f, 1f, 1f, newAlph);
            engine6_on_animate.Modulate = new Color(1f, 1f, 1f, newAlph);
        }


        if (IsInstanceValid(_playerScenceParents) && ShootPositionPath != null)
        {
            if (_playerScenceParents.nearEnemyToPlayerPos.Count == 0)
                return;
            if ((_playerScenceParents.nearEnemyToPlayerPos[0] - this.GlobalPosition).Length() > _shipBulidData.aimDistance)
            {
                //GD.Print("_playerScenceParents.nearEnemyToPlayerPos - weaponShootPos.GlobalPosition).Length() " + (_playerScenceParents.nearEnemyToPlayerPos - weaponShootPos.GlobalPosition).Length());
                // GD.Print("_shipBulidData.aimDistance " + _shipBulidData.aimDistance);
                weapon_baseweapon.Stop();
                return;
            }

            _reloadTime = _shipBulidData.reloadTime;
            _pseudo_timer += delta;
            if (_pseudo_timer > _reloadTime )
            {
                shoot();
                shoot();
                _pseudo_timer = 0;

            }
        }
    }



    private bool _isExitedFightArea = false;
    private float _exitedFightAreaDamge= 1f;
    private float _isExitedTime = 0f;

    public override void exitedFightArea()
    {
        base.exitedFightArea();
        _isExitedFightArea = true;
        //需要主界面做出反应
        GD.Print("Player+++++++++++++GlobalPosition:" + this.GlobalPosition);
    }
    public override void enterFightArea()
    {
        base.enterFightArea();
        _isExitedFightArea = false;
        _exitedFightAreaDamge = 1f;
        _isExitedTime = 0f;
        //需要主界面做出反应
    }

    public override void interface1ChannelOn()
    {
        base.interface1ChannelOn();
        if (!IsInstanceValid(engine1_on_animate))
            return;
        engine1_on_animate.Modulate = new Color(1f, 1f, 1f, 0f);
        engine1_on_animate.Play();
        if (!IsInstanceValid(engine7_on_animate))
            return;
        engine7_on_animate.Modulate = new Color(1f, 1f, 1f, 0f);
        engine7_on_animate.Play();



    }
    public override void interface2ChannelOn()
    {
        base.interface2ChannelOn();
        if (!IsInstanceValid(engine2_on_animate))
            return;
        engine2_on_animate.Modulate = new Color(1f, 1f, 1f, 0f);
        engine2_on_animate.Play();
        if (!IsInstanceValid(engine8_on_animate))
            return;
        engine8_on_animate.Modulate = new Color(1f, 1f, 1f, 0f);
        engine8_on_animate.Play();


    }
    public override void interface3ChannelOn()
    {
        base.interface3ChannelOn();
        if (!IsInstanceValid(engine3_on_animate))
            return;

        engine3_on_animate.Modulate = new Color(1f, 1f, 1f, 0f);
        engine3_on_animate.Play();
        if (!IsInstanceValid(engine9_on_animate))
            return;
        engine9_on_animate.Modulate = new Color(1f, 1f, 1f, 0f);
        engine9_on_animate.Play();

    }
    public override void interface4ChannelOn()
    {
        base.interface4ChannelOn();
        if (!IsInstanceValid(engine4_on_animate))
            return;
      //  GD.Print("engine4_on_animate.Modulate before" + engine3_on_animate.Modulate);
        engine4_on_animate.Modulate = new Color(1f, 1f, 1f, 0f);
       // GD.Print("engine4_on_animate.Modulate after" + engine3_on_animate.Modulate);
        engine4_on_animate.Play();
        if (!IsInstanceValid(engine10_on_animate))
            return;
        engine10_on_animate.Modulate = new Color(1f, 1f, 1f, 0f);
        engine10_on_animate.Play();

    }
    public override void interface5ChannelOn()
    {
        base.interface5ChannelOn();
        if (!IsInstanceValid(engine5_on_animate))
            return;
        engine5_on_animate.Modulate = new Color(1f, 1f, 1f, 0f);
        engine5_on_animate.Play();
        if (!IsInstanceValid(engine11_on_animate))
            return;
        engine11_on_animate.Modulate = new Color(1f, 1f, 1f, 0f);
        engine11_on_animate.Play();

    }
    public override void interface6ChannelOn()
    {
        base.interface6ChannelOn();
        if (!IsInstanceValid(engine6_on_animate))
            return;
        engine6_on_animate.Modulate = new Color(1f, 1f, 1f, 0f);
        engine6_on_animate.Play();
        if (!IsInstanceValid(engine12_on_animate))
            return;
        engine12_on_animate.Modulate = new Color(1f, 1f, 1f, 0f);
        engine12_on_animate.Play();

    }
    public override void interface1ChannelOff()
    {
        base.interface1ChannelOff();
        if (!IsInstanceValid(engine1_on_animate))
            return;
        engine1_on_animate.Modulate = new Color(1f, 1f, 1f, 0f);
        engine1_on_animate.Stop();
        if (!IsInstanceValid(engine7_on_animate))
            return;
        engine7_on_animate.Modulate = new Color(1f, 1f, 1f, 0f);
        engine7_on_animate.Stop();
    }
    public override void interface2ChannelOff()
    {
        base.interface2ChannelOff();
        if (!IsInstanceValid(engine2_on_animate))
            return;
        engine2_on_animate.Modulate = new Color(1f, 1f, 1f, 0f);
        engine2_on_animate.Stop();
        if (!IsInstanceValid(engine8_on_animate))
            return;
        engine8_on_animate.Modulate = new Color(1f, 1f, 1f, 0f);
        engine8_on_animate.Stop();
    }
    public override void interface3ChannelOff()
    {
        base.interface3ChannelOff();
        if (!IsInstanceValid(engine3_on_animate))
            return;
        engine3_on_animate.Modulate = new Color(1f, 1f, 1f, 0f);
        engine3_on_animate.Stop();
        if (!IsInstanceValid(engine9_on_animate))
            return;
        engine9_on_animate.Modulate = new Color(1f, 1f, 1f, 0f);
        engine9_on_animate.Stop();
    }
    public override void interface4ChannelOff()
    {
        base.interface4ChannelOff();
        if (!IsInstanceValid(engine4_on_animate))
            return;
        engine4_on_animate.Modulate = new Color(1f, 1f, 1f, 0f);
        engine4_on_animate.Stop();
        if (!IsInstanceValid(engine10_on_animate))
            return;
        engine10_on_animate.Modulate = new Color(1f, 1f, 1f, 0f);
        engine10_on_animate.Stop();
    }
    public override void interface5ChannelOff()
    {
        base.interface5ChannelOn();
        if (!IsInstanceValid(engine6_on_animate))
            return;
        engine5_on_animate.Modulate = new Color(1f, 1f, 1f, 0f);
        engine5_on_animate.Stop();
        if (!IsInstanceValid(engine11_on_animate))
            return;
        engine11_on_animate.Modulate = new Color(1f, 1f, 1f, 0f);
        engine11_on_animate.Stop();
    }
    public override void interface6ChannelOff()
    {
        base.interface6ChannelOff();
        if (!IsInstanceValid(engine6_on_animate))
            return;
        engine6_on_animate.Modulate = new Color(1f, 1f, 1f, 0f);
        engine6_on_animate.Stop();
        if (!IsInstanceValid(engine12_on_animate))
            return;
        engine12_on_animate.Modulate = new Color(1f, 1f, 1f, 0f);
        engine12_on_animate.Stop();
    }

    public void damgeAnimate(float per)
    {

    }

    public override void engineON()
    {
        if (IsInstanceValid(engine_effect1))
            engine_effect1.Emitting = true;
        if (IsInstanceValid(engine_effect2))
            engine_effect2.Emitting = true;
        if (IsInstanceValid(_interfacePositionBuild1))
            _interfacePositionBuild1.engineON();
        if (IsInstanceValid(_interfacePositionBuild2))
            _interfacePositionBuild2.engineON();
        if (IsInstanceValid(_interfacePositionBuild3))
            _interfacePositionBuild3.engineON();
        if (IsInstanceValid(_interfacePositionBuild4))
            _interfacePositionBuild4.engineON();
        if (IsInstanceValid(_interfacePositionBuild5))
            _interfacePositionBuild5.engineON();

    }

    public override void updateTradFromHealth(float per = 1f)
    {
        base.updateTradFromHealth(per);
        
        if (per < _damagedHealthPer)
        {

            if (per < _slightDamagedHealthPer)
            {
                if (IsInstanceValid(engine1_pic_Damaged))
                    engine1_pic_Damaged.Modulate = new Color(1f, 1f, 1f, 1f);


                if (per < _veryDamagedHealthPer)
                {
                    if (IsInstanceValid(engine1_pic_SlightDamaged))
                        engine1_pic_SlightDamaged.Modulate = new Color(1f, 1f, 1f, 1f);

                    float alph = 1f - (per) / (_veryDamagedHealthPer);
                    if (IsInstanceValid(engine1_pic_VeryDamaged))
                        engine1_pic_VeryDamaged.Modulate = new Color(1f, 1f, 1f, alph);

                }
                else
                {
                    float alph = 1f - (per - _veryDamagedHealthPer) / (_slightDamagedHealthPer - _veryDamagedHealthPer);
                    if (IsInstanceValid(engine1_pic_SlightDamaged))
                        engine1_pic_SlightDamaged.Modulate = new Color(1f, 1f, 1f, alph);

                }
            }
            else
            {
                float alph = 1f - (per - _slightDamagedHealthPer) / (_damagedHealthPer - _slightDamagedHealthPer);
                if (IsInstanceValid(engine1_pic_Damaged))
                    engine1_pic_Damaged.Modulate = new Color(1f, 1f, 1f, alph);

            }
        }


        if (IsInstanceValid(_interfacePositionBuild1))
            _interfacePositionBuild1.updateTradFromHealth(per);
        if (IsInstanceValid(_interfacePositionBuild2))
            _interfacePositionBuild2.updateTradFromHealth(per);
        if (IsInstanceValid(_interfacePositionBuild3))
            _interfacePositionBuild3.updateTradFromHealth(per);
        if (IsInstanceValid(_interfacePositionBuild4))
            _interfacePositionBuild4.updateTradFromHealth(per);
        if (IsInstanceValid(_interfacePositionBuild5))
            _interfacePositionBuild5.updateTradFromHealth(per);
        if (IsInstanceValid(_interfacePositionBuild6))
            _interfacePositionBuild6.updateTradFromHealth(per);
    }


}
