using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public class baseDerivedPart : baseBulidPart
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.



    [Export]
    public NodePath engine1_on_animatePath;
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
    [Export]
    public NodePath engine2_on_animatePath;
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
    [Export]
    public NodePath engine3_on_animatePath;
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
    [Export]
    public NodePath engine4_on_animatePath;
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
    [Export]
    public NodePath engine5_on_animatePath;
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
    [Export]
    public NodePath engine6_on_animatePath;
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
    [Export]
    public NodePath engine_pic_DamagedPath;
    public Sprite engine1_pic_Damaged
    {
        get
        {
            if (engine_pic_DamagedPath != null)
                return (Sprite)GetNode(engine_pic_DamagedPath);
            else
                return null;
        }
    }
    [Export]
    public NodePath engine_pic_SlightDamagedPath;
    public Sprite engine1_pic_SlightDamaged
    {
        get
        {
            if (engine_pic_SlightDamagedPath != null)
                return (Sprite)GetNode(engine_pic_SlightDamagedPath);
            else
                return null;
        }
    }
    [Export]
    public NodePath engine_pic_VeryDamagedPath;
    public Sprite engine1_pic_VeryDamaged
    {
        get
        {
            if (engine_pic_VeryDamagedPath != null)
                return (Sprite)GetNode(engine_pic_VeryDamagedPath);
            else
                return null;
        }
    }
    [Export]
    public NodePath weaponAnimationPath;
    public AnimationPlayer weaponAnimation
    {
        get
        {
            if (weaponAnimationPath != null)
                return (AnimationPlayer)GetNode(weaponAnimationPath);
            else
                return null;
        }
    }

    [Export]
    public NodePath[] ShootPositionPath;

    public Array<Node2D> _shootPosition = new Array<Node2D>();


    [Export]
    public NodePath weaponPath;
    public Node2D weapon
    {
        get
        {
            if (weaponPath != null)
                return (Node2D)GetNode(weaponPath);
            else
                return null;
        }
    }

    [Export]
    public NodePath[] weaponArrayPath;

    public Array<Node2D> _weaponArray = new Array<Node2D>();




    [Export]
    public NodePath weapon_pic_DamagedPath;
    public Sprite weapon_pic_Damaged
    {
        get
        {
            if (weapon_pic_DamagedPath != null)
                return (Sprite)GetNode(weapon_pic_DamagedPath);
            else
                return null;
        }
    }
    [Export]
    public NodePath weapon_pic_SlightDamagedPath;
    public Sprite weapon_pic_SlightDamaged
    {
        get
        {
            if (weapon_pic_SlightDamagedPath != null)
                return (Sprite)GetNode(weapon_pic_SlightDamagedPath);
            else
                return null;
        }
    }
    [Export]
    public NodePath weapon_pic_VeryDamagedPath;
    public Sprite weapon_pic_VeryDamaged
    {
        get
        {
            if (weapon_pic_VeryDamagedPath != null)
                return (Sprite)GetNode(weapon_pic_VeryDamagedPath);
            else
                return null;
        }
    }

    [Export]
    public int weapon_target_number = 1;//武器目标瞄准数

    [Export]
    public weaponShootMode weapon_auto_target = weaponShootMode.weaponShootMode_None;//瞄准模式

    public enum weaponShootMode
    {
        weaponShootMode_None, //自动瞄准
        weaponShootMode_Line, //向一个方向直线射击
        weaponShootMode_SpreedLine,//弧形射击方向

    }


    public override void _Ready()
    {
        base._Ready();
    }





    public float _reloadTime = 2f;//重装填时间
    public float _pseudo_timer = 0; //计时


    public bool _fisrtShoot = true; //第一次射击

    private float _alphChange = 0.1f;
    public override void _PhysicsProcess(float delta)
    {
        if (ShootPositionPath != null && ShootPositionPath.Length > 0 && _shootPosition.Count == 0)
        {
            //GD.Print("ShootPositionPath.Length" + ShootPositionPath.Length);
            for (int i = 0; i < ShootPositionPath.Length; i++)
            {
                _shootPosition.Add((Node2D)GetNode(ShootPositionPath[i]));
            }
        }
        if (weaponArrayPath != null && weaponArrayPath.Length > 0 && _weaponArray.Count == 0)
        {
            //GD.Print("ShootPositionPath.Length" + ShootPositionPath.Length);
            for (int i = 0; i < weaponArrayPath.Length; i++)
            {
                _weaponArray.Add((Node2D)GetNode(weaponArrayPath[i]));
            }
        }


        if (IsInstanceValid(engine1_on_animate) && engine1_on_animate.Playing && engine1_on_animate.Modulate.a < 1f)
        {
            float newAlph = engine1_on_animate.Modulate.a;
            newAlph = newAlph + _alphChange;
            newAlph = newAlph > 1f ? 1f : newAlph;
            engine1_on_animate.Modulate = new Color(1f, 1f, 1f, newAlph);
        }
        if (IsInstanceValid(engine2_on_animate) && engine2_on_animate.Playing && engine2_on_animate.Modulate.a < 1f)
        {
            float newAlph = engine2_on_animate.Modulate.a;
            newAlph = newAlph + _alphChange;
            newAlph = newAlph > 1f ? 1f : newAlph;
            engine2_on_animate.Modulate = new Color(1f, 1f, 1f, newAlph);
        }
        if (IsInstanceValid(engine3_on_animate) && engine3_on_animate.Playing && engine3_on_animate.Modulate.a < 1f)
        {
            float newAlph = engine3_on_animate.Modulate.a;

            newAlph = newAlph + _alphChange;
            newAlph = newAlph > 1f ? 1f : newAlph;
            engine3_on_animate.Modulate = new Color(1f, 1f, 1f, newAlph);

        }
        if (IsInstanceValid(engine4_on_animate) && engine4_on_animate.Playing && engine4_on_animate.Modulate.a < 1f)
        {
            float newAlph = engine4_on_animate.Modulate.a;
            newAlph = newAlph + _alphChange;
            newAlph = newAlph > 1f ? 1f : newAlph;
            //GD.Print("engine4_on_animate.Modulate" + engine4_on_animate.Modulate);
            engine4_on_animate.Modulate = new Color(1f, 1f, 1f, newAlph);
        }
        if (IsInstanceValid(engine5_on_animate) && engine5_on_animate.Playing && engine5_on_animate.Modulate.a < 1f)
        {
            float newAlph = engine5_on_animate.Modulate.a;
            newAlph = newAlph + _alphChange;
            newAlph = newAlph > 1f ? 1f : newAlph;
            engine5_on_animate.Modulate = new Color(1f, 1f, 1f, newAlph);
        }
        if (IsInstanceValid(engine6_on_animate) && engine6_on_animate.Playing && engine6_on_animate.Modulate.a < 1f)
        {
            float newAlph = engine6_on_animate.Modulate.a;
            newAlph = newAlph + _alphChange;
            newAlph = newAlph > 1f ? 1f : newAlph;
            engine6_on_animate.Modulate = new Color(1f, 1f, 1f, newAlph);
        }

        if (IsInstanceValid(_playerScenceParents))
        {
            if (_playerScenceParents.nearEnemyToPlayerPos.Count == 0)
                return;

            if ((_playerScenceParents.nearEnemyToPlayerPos[0] - this.GlobalPosition).Length() > _shipBulidData.aimDistance)
            {
               // GD.Print("_playerScenceParents.nearEnemyToPlayerPos - weaponShootPos.GlobalPosition).Length() " + ((_playerScenceParents.nearEnemyToPlayerPos[0] - weapon.GlobalPosition).Length()));
               // GD.Print("_shipBulidData.aimDistance " + _shipBulidData.aimDistance);
                return;
            }

            _reloadTime = _shipBulidData.reloadTime;
            //GD.Print(_shootmode);
            _pseudo_timer += delta;
            
            if (_pseudo_timer > _reloadTime|| _fisrtShoot == true)
            {
                //GD.Print("_pseudo_timer" + _pseudo_timer);
                if (IsInstanceValid(weapon))
                {
                    if (weapon_auto_target == weaponShootMode.weaponShootMode_None)
                    {
                        weapon.LookAt((_playerScenceParents.nearEnemyToPlayerPos[0] - weapon.GlobalPosition).Rotated((float)Math.PI / 2) + weapon.GlobalPosition);
                    }
                    else
                    {
                        //weapon.LookAt((_playerScenceParents.nearEnemyToPlayerPos[0] - weapon.GlobalPosition).Rotated((float)Math.PI / 2) + weapon.GlobalPosition);
                    }
                    
                    
                }
                if (_weaponArray.Count > 0)
                {
                    for (int i = 0; i < _weaponArray.Count; i++)
                    {
                        _weaponArray[i].LookAt((_playerScenceParents.nearEnemyToPlayerPos[0] - _weaponArray[i].GlobalPosition).Rotated((float)Math.PI / 2) + _weaponArray[i].GlobalPosition);
                    }
                    
                }
                shoot();
                _fisrtShoot = false;
                _pseudo_timer = 0;
            }
        }
    }

    public virtual void shoot()
    {
     //   GD.Print("weaponAnimation.Play(\"fire\");");
        if (!weaponAnimation.IsPlaying())
        {
            weaponAnimation.Play("fire");

        }
        

        if (!weaponAnimation.IsConnected("animation_finished", this, "animation_finishedFuction"))
            weaponAnimation.Connect("animation_finished", this, "animation_finishedFuction");

    }





    private void animation_finishedFuction(String anim_name)
    {
        if(anim_name == "fire")
        {
            weaponAnimation.Play("idle");
            //GD.Print("weaponAnimation.Stop(true);" + weaponAnimation.CurrentAnimationPosition);
        }

        
    }


    public override void playerReciveDamge(float value)
    {
        base.playerReciveDamge(value);
    }

    public override void exitedFightArea()
    {
        base.exitedFightArea();
    }

    public override void interface1ChannelOn()
    {
        base.interface1ChannelOn();
        if (!IsInstanceValid(engine1_on_animate))
            return;
        engine1_on_animate.Modulate = new Color(1f, 1f, 1f, 0f);
        engine1_on_animate.Play();



    }
    public override void interface2ChannelOn()
    {
        base.interface2ChannelOn();
        if (!IsInstanceValid(engine2_on_animate))
            return;
        engine2_on_animate.Modulate = new Color(1f, 1f, 1f, 0f);
        engine2_on_animate.Play();


    }
    public override void interface3ChannelOn()
    {
        base.interface3ChannelOn();
        if (!IsInstanceValid(engine3_on_animate))
            return;

        engine3_on_animate.Modulate = new Color(1f, 1f, 1f, 0f);
        engine3_on_animate.Play();

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

    }
    public override void interface5ChannelOn()
    {
        base.interface5ChannelOn();
        if (!IsInstanceValid(engine5_on_animate))
            return;
        engine5_on_animate.Modulate = new Color(1f, 1f, 1f, 0f);
        engine5_on_animate.Play();

    }
    public override void interface6ChannelOn()
    {
        base.interface6ChannelOn();
        if (!IsInstanceValid(engine6_on_animate))
            return;
        engine6_on_animate.Modulate = new Color(1f, 1f, 1f, 0f);
        engine6_on_animate.Play();

    }
    public override void interface1ChannelOff()
    {
        base.interface1ChannelOff();
        if (!IsInstanceValid(engine1_on_animate))
            return;
        engine1_on_animate.Modulate = new Color(1f, 1f, 1f, 0f);
        engine1_on_animate.Stop();
    }
    public override void interface2ChannelOff()
    {
        base.interface2ChannelOff();
        if (!IsInstanceValid(engine2_on_animate))
            return;
        engine2_on_animate.Modulate = new Color(1f, 1f, 1f, 0f);
        engine2_on_animate.Stop();
    }
    public override void interface3ChannelOff()
    {
        base.interface3ChannelOff();
        if (!IsInstanceValid(engine3_on_animate))
            return;
        engine3_on_animate.Modulate = new Color(1f, 1f, 1f, 0f);
        engine3_on_animate.Stop();
    }
    public override void interface4ChannelOff()
    {
        base.interface4ChannelOff();
        if (!IsInstanceValid(engine4_on_animate))
            return;
        engine4_on_animate.Modulate = new Color(1f, 1f, 1f, 0f);
        engine4_on_animate.Stop();
    }
    public override void interface5ChannelOff()
    {
        base.interface5ChannelOn();
        if (!IsInstanceValid(engine6_on_animate))
            return;
        engine5_on_animate.Modulate = new Color(1f, 1f, 1f, 0f);
        engine5_on_animate.Stop();
    }
    public override void interface6ChannelOff()
    {
        base.interface6ChannelOff();
        if (!IsInstanceValid(engine6_on_animate))
            return;
        engine6_on_animate.Modulate = new Color(1f, 1f, 1f, 0f);
        engine6_on_animate.Stop();
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
                if (IsInstanceValid(weapon_pic_Damaged))
                    weapon_pic_Damaged.Modulate = new Color(1f, 1f, 1f, 1f);

                if (per < _veryDamagedHealthPer)
                {
                    if (IsInstanceValid(engine1_pic_SlightDamaged))
                        engine1_pic_SlightDamaged.Modulate = new Color(1f, 1f, 1f, 1f);
                    if (IsInstanceValid(weapon_pic_SlightDamaged))
                        weapon_pic_SlightDamaged.Modulate = new Color(1f, 1f, 1f, 1f);
                    float alph = 1f - (per) / (_veryDamagedHealthPer);
                    if (IsInstanceValid(engine1_pic_VeryDamaged))
                        engine1_pic_VeryDamaged.Modulate = new Color(1f, 1f, 1f, alph);
                    if (IsInstanceValid(weapon_pic_VeryDamaged))
                        weapon_pic_VeryDamaged.Modulate = new Color(1f, 1f, 1f, alph);
                }
                else
                {
                    float alph = 1f - (per - _veryDamagedHealthPer) / (_slightDamagedHealthPer - _veryDamagedHealthPer);
                    if (IsInstanceValid(engine1_pic_SlightDamaged))
                        engine1_pic_SlightDamaged.Modulate = new Color(1f, 1f, 1f, alph);
                    if (IsInstanceValid(weapon_pic_SlightDamaged))
                        weapon_pic_SlightDamaged.Modulate = new Color(1f, 1f, 1f, alph);
                }
            }
            else
            {
                float alph = 1f - (per - _slightDamagedHealthPer) / (_damagedHealthPer - _slightDamagedHealthPer);
                if (IsInstanceValid(engine1_pic_Damaged))
                    engine1_pic_Damaged.Modulate = new Color(1f, 1f, 1f, alph);
                if (IsInstanceValid(weapon_pic_Damaged))
                    weapon_pic_Damaged.Modulate = new Color(1f, 1f, 1f, alph);
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
