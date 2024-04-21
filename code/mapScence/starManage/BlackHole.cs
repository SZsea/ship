using Godot;
using System;
using System.Collections.Generic;


[Tool]
public class BlackHole : Planet
{

    //是否预览
    [Export]
    public bool run = false;


    [Export]
    public NodePath BlackHolePath;
    [Export]
    public NodePath DiskPath;

    private ColorRect _blackHole;
    private ColorRect _disk;

    private ShaderMaterial _blackHoleMaterial;
    private ShaderMaterial _diskMaterial;

    //噪点 选否会增大噪点
    [Export]
    public bool starKind = true;
    [Export]
    public Vector2 lightpoint = new Vector2(0.712f, 0.396f);


    public override void _Ready()
    {
        _blackHole = (ColorRect)GetNode(BlackHolePath);
        _disk = (ColorRect)GetNode(DiskPath);

        _blackHoleMaterial = (ShaderMaterial)_blackHole.Material;
        _diskMaterial = (ShaderMaterial)_disk.Material;

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
        _blackHoleMaterial.SetShaderParam("pixels", _amount);
        _diskMaterial.SetShaderParam("pixels", _amount * 3.0f);
    }

    public override void set_size(Vector2 size)
    {
        _blackHole.RectSize = size;
        _disk.RectPosition = -size;
        _disk.RectSize = size * 3.0f;
    }

    public override void set_light(Vector2 pos)
    {
        
    }

    public override void set_seed(float number)
    {
        var converted_seed = number % 1000f / 100.0f;
        _diskMaterial.SetShaderParam("seed", converted_seed);
    }

    public override void set_rotate(float number)
    {
        _diskMaterial.SetShaderParam("rotation", number + 0.7f);
    }

    public override void update_time(float t)
    {
        _diskMaterial.SetShaderParam("time", t * 314.15f * 0.004f);
    }

    public override void set_custom_time(float t)
    {
        _diskMaterial.SetShaderParam("time", t * 314.15f *(float)_diskMaterial.GetShaderParam("time_speed") * 0.5f);
    }

    public override void set_dither(bool d)
    {
        _diskMaterial.SetShaderParam("should_dither", d);
    }


    public override bool get_dither()
    {
        return (bool)_diskMaterial.GetShaderParam("should_dither");
    }

    public override List<Color> get_colors()
    {
        List<Color> newColor = new List<Color>();
        Color colors = (Color)_diskMaterial.GetShaderParam("black_color");
        newColor.Add(colors);
        newColor.AddRange(_get_colors_from_gradient(_diskMaterial, "colorscheme"));
        newColor.AddRange(_get_colors_from_gradient(_diskMaterial, "colorscheme"));
        return newColor;
    }


    public override void set_colors(List<Color> colors)
    {
        List<Color> colors1 = new List<Color> { colors[1], colors[2] };
        List<Color> colors2 = new List<Color> { colors[3], colors[4], colors[5], colors[6], colors[7] };

        _blackHoleMaterial.SetShaderParam("black_color", colors[0]);
        _set_colors_from_gradient(_blackHoleMaterial, "colorscheme", colors1);
        _set_colors_from_gradient(_diskMaterial, "colorscheme", colors2);

    }

    public override void randomize_colors()
    {
        List<Color> seed_colors = _generate_new_colorscheme(5.0f + GD.Randi() % 2.0f, (float)GD.RandRange(0.3f, 0.5f), 2.0f);

        List<Color> cols1 = new List<Color>();

        for (int i = 0; i < 5; i++)
        {
            Color new_col = seed_colors[i].Darkened((i / 5.0f) * 0.7f);
            new_col = new_col.Lightened((1.0f - (i / 5.0f)) * 0.9f);
            cols1.Add(new_col);
        }

        List<Color> cols2 = new List<Color>
        {
            new Color("272736"),
            cols1[0],
            cols1[3]
        };
        cols2.AddRange(cols1);
        set_colors(cols2);
    }

    public override void _Process(float delta)
    {
        if (run)
        {
            base._Process(delta);
        }

    }
}

