using Godot;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;


[Tool]
public class GasPlanet : Planet
{
    //是否预览
    [Export]
    public bool run = false;


    [Export]
    public NodePath CloudPath;
    [Export]
    public NodePath Cloud2Path;


    private ColorRect _cloud;
    private ColorRect _cloud2;


    private ShaderMaterial _cloudMaterial;
    private ShaderMaterial _cloud2Material;

    //噪点 选否会增大噪点
    [Export]
    public bool starKind = true;
    [Export]
    public Vector2 lightpoint = new Vector2(0.712f, 0.396f);

    public override void _Ready()
    {
        _cloud = (ColorRect)GetNode(CloudPath);
        _cloud2 = (ColorRect)GetNode(Cloud2Path);

        _cloudMaterial = (ShaderMaterial)_cloud.Material;
        _cloud2Material = (ShaderMaterial)_cloud2.Material;

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
        _cloudMaterial.SetShaderParam("pixels", _amount);
        _cloud2Material.SetShaderParam("pixels", _amount);

    }

    public override void set_size(Vector2 size)
    {
        _cloud.RectSize = size;
        _cloud2.RectSize = size;
    }


    public override void set_light(Vector2 pos)
    {
        _cloudMaterial.SetShaderParam("light_origin", pos);
        _cloud2Material.SetShaderParam("light_origin", pos);

    }


    public override void set_seed(float number)
    {
        var converted_seed = number % 1000.0f / 100.0f;
        _cloudMaterial.SetShaderParam("seed", converted_seed);
        _cloud2Material.SetShaderParam("seed", converted_seed);
        _cloud2Material.SetShaderParam("cloud_cover", GD.RandRange(0.28f,0.5f));
    }


    public override void set_rotate(float number)
    {
        _cloudMaterial.SetShaderParam("rotation", number);
        _cloud2Material.SetShaderParam("rotation", number);
    }


    public override void update_time(float t)
    {
        _cloudMaterial.SetShaderParam("time", t * get_multiplier(_cloudMaterial) * 0.005f);
        _cloud2Material.SetShaderParam("time", t * get_multiplier(_cloud2Material) * 0.005f);
    }

    public override void set_custom_time(float t)
    {
        _cloudMaterial.SetShaderParam("time", t * get_multiplier(_cloudMaterial) );
        _cloud2Material.SetShaderParam("time", t * get_multiplier(_cloud2Material));
    }

    private List<string> color_vars1 = new List<string> { "base_color", "outline_color", "shadow_base_color", "shadow_outline_color" };
    private List<string> color_vars2 = new List<string> { "base_color", "outline_color", "shadow_base_color", "shadow_outline_color" };

    public override List<Color> get_colors()
    {
        List<Color> newColor = new List<Color>();
        newColor.AddRange(_get_colors_from_vars(_cloudMaterial, color_vars1));
        newColor.AddRange(_get_colors_from_vars(_cloud2Material, color_vars2));

        return newColor;
    }


    public override void set_colors(List<Color> colors)
    {
        List<Color> colors1 = new List<Color> { colors[0], colors[3], colors[1] };
        List<Color> colors2 = new List<Color> { colors[4], colors[7], colors[1] };


        _set_colors_from_vars(_cloudMaterial, color_vars1, colors1);
        _set_colors_from_vars(_cloud2Material, color_vars2, colors2);
    }


    public override void randomize_colors()
    {
        List<Color> seed_colors = _generate_new_colorscheme(8 + GD.Randi() % 4f, (float)GD.RandRange(0.3f, 0.8f), 1.0f);

        List<Color> cols1 = new List<Color>();
        List<Color> cols2 = new List<Color>();

        for (int i = 0; i < 4; i++)
        {
            Color new_col = seed_colors[i].Darkened(i / 6.0f).Darkened(0.7f);           
            cols1.Add(new_col);
        }
        for (int i = 0; i < 4; i++)
        {
            Color new_col = seed_colors[i + 4].Darkened(i / 4.0f);
            new_col = new_col.Lightened((1.0f - (i / 4.0f)) * 0.5f);
            cols2.Add(new_col);
        }
        List<Color> newColors = new List<Color>();
        newColors.AddRange(cols1);
        newColors.AddRange(cols2);

        set_colors(newColors);
    }


    public override void _Process(float delta)
    {
        if (run)
        {
            base._Process(delta);
        }

    }




}
