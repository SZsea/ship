using Godot;
using System;

public class playerControl : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.

    private float _speed;
    public enum operateMode
    {
        keybord = 0,
        joypads = 1,
    }

    public operateMode operateModeUsing;

    public float speed
    {
        get
        {
            return _speed;
        }
        set
        {
            _speed = value;
        }
    }


    private PlayerScence _player;
    public override void _Ready()
    {
        _player = GetParent<PlayerScence>();
        operateModeUsing = operateMode.keybord;
    }


    public float degreesChange = 4f;
    //操作定义

    public override void _Process(float delta)
    {
        base._Process(delta);
        switch(operateModeUsing)
        {
            case operateMode.keybord:
                {
                    Vector2 velocity = Vector2.Zero;
                    //if ((GetGlobalMousePosition() - _player.Position).Length() > 20f)
                    //{
                    //    _player.LookAt((GetGlobalMousePosition() - _player.Position).Rotated((float)Math.PI / 2) + _player.Position);
                    //}
                    if (Input.IsActionPressed("character_up"))
                    {
                        //GD.Print("_player.Rotation" + _player.GlobalRotationDegrees);
                        if ((int)_player.GlobalRotationDegrees != 0)
                        {
                            if(_player.GlobalRotationDegrees > 0)
                            {
                                _player.GlobalRotationDegrees = _player.GlobalRotationDegrees - degreesChange;
                            }else
                            {
                                _player.GlobalRotationDegrees = _player.GlobalRotationDegrees + degreesChange;
                            }
                        }                           
                        //velocity += new Vector2(0, -speed).Rotated(_player.Rotation);

                    }
                    if (Input.IsActionPressed("character_down"))
                    {
                        //GD.Print("_player.Rotation" + _player.GlobalRotationDegrees);
                        if ((int)Mathf.Abs(_player.GlobalRotationDegrees) != 180 )
                        {
                            if (_player.GlobalRotationDegrees > 0)
                            {
                                _player.GlobalRotationDegrees = _player.GlobalRotationDegrees + degreesChange;
                            }
                            else
                            {
                                _player.GlobalRotationDegrees = _player.GlobalRotationDegrees - degreesChange;
                            }
                        }
                        //velocity += new Vector2(0, speed).Rotated(_player.Rotation);

                    }
                    if (Input.IsActionPressed("character_left"))
                    {
                        //GD.Print("_player.Rotation" + _player.GlobalRotationDegrees);
                        if ((int)_player.GlobalRotationDegrees != -90)
                        {
                            if (Mathf.Abs(_player.GlobalRotationDegrees) > 90)
                            {
                                _player.GlobalRotationDegrees = _player.GlobalRotationDegrees + degreesChange;
                            }
                            else
                            {
                                _player.GlobalRotationDegrees = _player.GlobalRotationDegrees - degreesChange;
                            }
                        }
                        // velocity += new Vector2(-speed, 0).Rotated(_player.Rotation);

                    }
                    if (Input.IsActionPressed("character_right"))
                    {
                        //GD.Print("_player.Rotation" + _player.GlobalRotationDegrees);
                        if ((int)_player.GlobalRotationDegrees != 90)
                        {
                            if (Mathf.Abs(_player.GlobalRotationDegrees) > 90)
                            {
                                _player.GlobalRotationDegrees = _player.GlobalRotationDegrees - degreesChange;
                            }
                            else
                            {
                                _player.GlobalRotationDegrees = _player.GlobalRotationDegrees + degreesChange;
                            }
                        }
                        //velocity += new Vector2(speed, 0).Rotated(_player.Rotation);

                    }


                    velocity += new Vector2(0, -speed).Rotated(_player.Rotation);
                    _player.Position += velocity * delta;
                }
                break;
            case operateMode.joypads:
                {
                    Vector2 velocity = Vector2.Zero;
                    if (Input.IsActionPressed("character_up"))
                    {
                        velocity += new Vector2(0, -speed);

                    }
                    if (Input.IsActionPressed("character_down"))
                    {
                        velocity += new Vector2(0, speed);

                    }
                    if (Input.IsActionPressed("character_left"))
                    {
                        velocity += new Vector2(-speed, 0);

                    }
                    if (Input.IsActionPressed("character_right"))
                    {
                        velocity += new Vector2(speed, 0);

                    }
                    if (Input.IsActionPressed("character_rotate_up"))
                    {
                        _player.Rotation = _player.Rotation - 5f * delta;
                    }
                    if (Input.IsActionPressed("character_rotate_down"))
                    {
                        _player.Rotation = _player.Rotation + 5f * delta; 
                    }
                    if (Input.IsActionPressed("character_rotate_left"))
                    {
                        _player.Rotation = _player.Rotation - 5f * delta; 
                    }
                    if (Input.IsActionPressed("character_rotate_right"))
                    {
                        _player.Rotation = _player.Rotation + 5f * delta;
                    }
                    _player.Position += velocity * delta;
                }
                break;
        }



       
    }
}
