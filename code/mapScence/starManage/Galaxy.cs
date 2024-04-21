using Godot;
using System;
using System.Collections.Generic;


[Tool]
public class Galaxy : Planet
{
    //是否预览
    [Export]
    public bool run = false;


    [Export]
    public NodePath GalaxyPath;

    private ColorRect _galaxy;

    private ShaderMaterial _galaxyMaterial;


    //噪点 选否会增大噪点
    [Export]
    public bool starKind = true;
    [Export]
    public Vector2 lightpoint = new Vector2(0.712f, 0.396f);


    public override void _Ready()
    {
        _galaxy = (ColorRect)GetNode(GalaxyPath);

        _galaxyMaterial = (ShaderMaterial)_galaxy.Material;

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
        _galaxyMaterial.SetShaderParam("pixels", _amount);

    }

    public override void set_size(Vector2 size)
    {
        _galaxy.RectSize = size;
    }

    public override void set_light(Vector2 pos)
    {
        
    }

    public override void set_seed(float number)
    {
        float converted_seed = number % 1000f / 100.0f;
        _galaxyMaterial.SetShaderParam("seed", converted_seed);
    }

    public override void set_rotate(float number)
    {
        _galaxyMaterial.SetShaderParam("rotation", number);
    }

    public override void update_time(float t)
    {
        _galaxyMaterial.SetShaderParam("time", t * get_multiplier(_galaxyMaterial) * 0.04);
    }

    public override void set_custom_time(float t)
    {
        _galaxyMaterial.SetShaderParam("time", t * Math.PI * 2 * (float)_galaxyMaterial.GetShaderParam("time_speed"));
    }


    public override void set_dither(bool d)
    {
        _galaxyMaterial.SetShaderParam("should_dither", d);
    }


    public override bool get_dither()
    {
        return (bool)_galaxyMaterial.GetShaderParam("should_dither");
    }


    public override List<Color> get_colors()
    {
        List<Color> newColor = new List<Color>();
        newColor.AddRange(_get_colors_from_gradient(_galaxyMaterial, "colorscheme"));
        return newColor;
    }

    public override void set_colors(List<Color> colors)
    {
        _set_colors_from_gradient(_galaxyMaterial, "colorscheme", colors);
    }


    public override void randomize_colors()
    {
        List<Color> seed_colors = _generate_new_colorscheme(6.0f,(float)GD.RandRange(0.5f,0.8f),1.4f);

        List<Color> cols1 = new List<Color>();

        for (int i = 0; i < 6; i++)
        {
            Color new_col = seed_colors[i].Darkened(i / 7.0f);
            new_col = new_col.Lightened((1.0f - (i / 6.0f)) * 0.6f);
            cols1.Add(new_col);
        }

        set_colors(cols1);
    }


    public override void _Process(float delta)
    {
        if (run)
        {
            base._Process(delta);
        }

    }
}
