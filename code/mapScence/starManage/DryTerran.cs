using Godot;
using System;
using System.Collections.Generic;


[Tool]
public class DryTerran : Planet
{
    //是否预览
    [Export]
    public bool run = false;


    [Export]
    public NodePath LandPath;

    private ColorRect _land;

    private ShaderMaterial _LandMaterial;


    //噪点 选否会增大噪点
    [Export]
    public bool starKind = true;
    [Export]
    public Vector2 lightpoint = new Vector2(0.712f, 0.396f);
    public override void _Ready()
    {
        _land = (ColorRect)GetNode(LandPath);

        _LandMaterial = (ShaderMaterial)_land.Material;

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
        _LandMaterial.SetShaderParam("pixels", _amount);
    }

    public override void set_size(Vector2 size)
    {
        _land.RectSize = size;
    }

    public override void set_light(Vector2 pos)
    {
        _LandMaterial.SetShaderParam("light_origin", pos);
    }

    public override void set_seed(float number)
    {
        var converted_seed = number % 1000f / 100.0f;
        _LandMaterial.SetShaderParam("seed", converted_seed);
    }

    public override void set_rotate(float number)
    {
        _LandMaterial.SetShaderParam("rotation", number);
    }

    public override void update_time(float t)
    {
        _LandMaterial.SetShaderParam("time", t * get_multiplier(_LandMaterial) * 0.02);
    }

    public override void set_custom_time(float t)
    {
        _LandMaterial.SetShaderParam("time", t * get_multiplier(_LandMaterial));
    }


    public override void set_dither(bool d)
    {
        _LandMaterial.SetShaderParam("should_dither", d);
    }

    public override bool get_dither()
    {
        return (bool)_LandMaterial.GetShaderParam("should_dither");
    }

    public override List<Color> get_colors()
    {
        List<Color> newColor = new List<Color>();
        newColor.AddRange(_get_colors_from_gradient(_LandMaterial, "colors"));
        return newColor;
    }


    public override void set_colors(List<Color> colors)
    {
        _set_colors_from_gradient(_LandMaterial, "colors", colors);
    }

    public override void randomize_colors()
    {
        List<Color> seed_colors = _generate_new_colorscheme(5.0f + GD.Randi() % 3.0f, (float)GD.RandRange(0.3f, 0.65f), 1.0f);

        List<Color> cols1 = new List<Color>();

        for (int i = 0; i < 6; i++)
        {
            Color new_col = seed_colors[i].Darkened(i / 5.0f);
            new_col = new_col.Lightened((1.0f - (i / 5.0f)) * 0.2f);
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
