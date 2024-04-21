using Godot;
using System;
using System.Collections.Generic;


[Tool]
public class Asteroids : Planet
{

    //是否预览
    [Export]
    public bool run = false;


    [Export]
    public NodePath AsteroidPath;

    private ColorRect _asteroid;

    private ShaderMaterial _asteroidMaterial;


    //噪点 选否会增大噪点
    [Export]
    public bool starKind = true;
    [Export]
    public Vector2 lightpoint = new Vector2(0.712f, 0.396f);



    public override void _Ready()
    {
        _asteroid = (ColorRect)GetNode(AsteroidPath);

        _asteroidMaterial = (ShaderMaterial)_asteroid.Material;

        set_pixels(pixels);
        set_size(planetSize);
        set_dither(planetDither);
        set_light(lightpoint);
    }

    public override void _EnterTree()
    {
        if (Engine.EditorHint)
            _Ready();
    }


    public override void set_pixels(float _amount)
    {
        _asteroidMaterial.SetShaderParam("pixels", _amount);
    }

    public override void set_size(Vector2 size)
    {
        _asteroid.RectSize = size;
    }

    public override void set_light(Vector2 pos)
    {
        _asteroidMaterial.SetShaderParam("light_origin", pos);
    }

    public override void set_seed(float number)
    {
        var converted_seed = number % 1000f / 100.0f;
        _asteroidMaterial.SetShaderParam("seed", converted_seed);
    }


    public override void set_rotate(float number)
    {
        _asteroidMaterial.SetShaderParam("rotation", number);
    }


    public override void update_time(float t)
    {
        
    }


    public override void set_custom_time(float t)
    {
        _asteroidMaterial.SetShaderParam("rotation", t * Mathf.Pi * 2.0f);
    }

    public override void set_dither(bool d)
    {
        _asteroidMaterial.SetShaderParam("should_dither", d);
    }


    public override bool get_dither()
    {
        return (bool)_asteroidMaterial.GetShaderParam("should_dither");
    }

    private List<string> color_vars1 = new List<string> { "color1", "color2", "color3" };

    public override List<Color> get_colors()
    {
        return _get_colors_from_vars(_asteroidMaterial, color_vars1);
    }

    public override void set_colors(List<Color> colors)
    {
        _set_colors_from_vars(_asteroidMaterial, color_vars1, colors);
    }

    public override void randomize_colors()
    {
        var seed_colors = _generate_new_colorscheme(3.0f + GD.Randi() % 2.0f, (float)GD.RandRange(0.3f, 0.6f), 0.7f);
        List<Color> cols = new List<Color>();

        for (int i = 0; i < 3; i++)
        {
            Color new_col = seed_colors[i].Darkened(i / 3.0f);
            new_col = new_col.Lightened((1.0f - (i / 3.0f)) * 0.2f);
            cols.Add(new_col);
        }


        set_colors(cols);
    }
    public override void _Process(float delta)
    {
        if (run)
        {
            base._Process(delta);
        }

    }
}
