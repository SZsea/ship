using Godot;
using System;
using System.Collections.Generic;


[Tool]
public class IceWorld : Planet
{


    //是否预览
    [Export]
    public bool run = false;
    [Export]
    public NodePath PlanetUnderPath;
    [Export]
    public NodePath LakesPath;
    [Export]
    public NodePath CloudsPath;

    private ColorRect _planetUnder;
    private ColorRect _lakes;
    private ColorRect _clouds;
    private ShaderMaterial _planetUnderMaterial;
    private ShaderMaterial _lakesMaterial;
    private ShaderMaterial _cloudsMaterial;

    //噪点 选否会增大噪点
    [Export]
    public bool starKind = true;
    [Export]
    public Vector2 lightpoint = new Vector2(0.712f, 0.396f);

    public override void _Ready()
    {
        _planetUnder = (ColorRect)GetNode(PlanetUnderPath);
        _lakes = (ColorRect)GetNode(LakesPath);
        _clouds = (ColorRect)GetNode(CloudsPath);

        _planetUnderMaterial = (ShaderMaterial)_planetUnder.Material;
        _lakesMaterial = (ShaderMaterial)_lakes.Material;
        _cloudsMaterial = (ShaderMaterial)_clouds.Material;

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
        _planetUnderMaterial.SetShaderParam("pixels", _amount);
        _lakesMaterial.SetShaderParam("pixels", _amount);
        _cloudsMaterial.SetShaderParam("pixels", _amount);

    }

    public override void set_size(Vector2 size)
    {
        _planetUnder.RectSize = size;
        _lakes.RectSize = size;
        _clouds.RectSize = size;
    }


    public override void set_light(Vector2 pos)
    {
        _planetUnderMaterial.SetShaderParam("light_origin", pos);
        _lakesMaterial.SetShaderParam("light_origin", pos);
        _cloudsMaterial.SetShaderParam("light_origin", pos);
    }


    public override void set_seed(float number)
    {
        var converted_seed = number % 1000.0f / 100.0f;
        _planetUnderMaterial.SetShaderParam("seed", converted_seed);
        _lakesMaterial.SetShaderParam("seed", converted_seed);
        _cloudsMaterial.SetShaderParam("seed", converted_seed);
    }

    public override void set_rotate(float number)
    {
        _planetUnderMaterial.SetShaderParam("roration", number);
        _lakesMaterial.SetShaderParam("roration", number);
        _cloudsMaterial.SetShaderParam("roration", number);
    }

    public override void update_time(float t)
    {
        _planetUnderMaterial.SetShaderParam("time", t * get_multiplier(_planetUnderMaterial) *0.02f);
        _lakesMaterial.SetShaderParam("time", t * get_multiplier(_lakesMaterial) * 0.02f);
        _cloudsMaterial.SetShaderParam("time", t * get_multiplier(_cloudsMaterial) * 0.01f);
    }

    public override void set_custom_time(float number)
    {
        _planetUnderMaterial.SetShaderParam("time", number * get_multiplier(_planetUnderMaterial));
        _lakesMaterial.SetShaderParam("time", number * get_multiplier(_lakesMaterial));
        _cloudsMaterial.SetShaderParam("time", number * get_multiplier(_cloudsMaterial));
    }


    public override void set_dither(bool d)
    {
        _planetUnderMaterial.SetShaderParam("should_dither",d);
    }

    public override bool get_dither()
    {
        return (bool)_planetUnderMaterial.GetShaderParam("should_dither");
    }

    private List<string> color_vars1 = new List<string> { "color1", "color2", "color3" };
    private List<string> color_vars2 = new List<string> { "color1", "color2", "color3" };
    private List<string> color_vars3 = new List<string> { "base_color", "outline_color", "shadow_base_color", "shadow_outline_color" };

    public override List<Color> get_colors()
    {
        List<Color> newColor = new List<Color>();
        newColor.AddRange(_get_colors_from_vars(_planetUnderMaterial, color_vars1));
        newColor.AddRange(_get_colors_from_vars(_lakesMaterial, color_vars2));
        newColor.AddRange(_get_colors_from_vars(_cloudsMaterial, color_vars3));
        return newColor;
    }


    public override void set_colors(List<Color> colors)
    {
        List<Color> colors1 = new List<Color> { colors[0], colors[2], colors[1] };
        List<Color> colors2 = new List<Color> { colors[3], colors[5], colors[1] };
        List<Color> colors3 = new List<Color> { colors[6], colors[9], colors[1] };

        _set_colors_from_vars(_planetUnderMaterial, color_vars1, colors1);
        _set_colors_from_vars(_lakesMaterial, color_vars2, colors2);
        _set_colors_from_vars(_cloudsMaterial, color_vars3, colors3);
    }


    public override void randomize_colors()
    {
        var seed_colors = _generate_new_colorscheme(GD.Randi() % 2 + 3, (float)GD.RandRange(0.7f, 1.0f), (float)GD.RandRange(0.45f, 0.55f));
        List<Color> land_colors = new List<Color>();
        List<Color> lake_colors = new List<Color>();
        List<Color> cloud_colors = new List<Color>();
        for (int i = 0; i < 3; i++)
        {
            var new_col = seed_colors[0].Darkened(i / 3.0f);
            land_colors.Add(Color.FromHsv((new_col.h + (0.2f * (i / 4.0f))), new_col.s, new_col.v));
        }
        for (int i = 0; i < 3; i++)
        {
            var new_col = seed_colors[1].Darkened(i / 3.0f);
            lake_colors.Add(Color.FromHsv((new_col.h + (0.2f * (i / 3.0f))), new_col.s, new_col.v));
        }
        for (int i = 0; i < 4; i++)
        {
            var new_col = seed_colors[2].Lightened((1.0f - (i / 4.0f)) * 0.8f);
            cloud_colors.Add(Color.FromHsv((new_col.h + (0.2f * (i / 4.0f))), new_col.s, new_col.v));
        }

        List<Color> newColors = new List<Color>();
        newColors.AddRange(land_colors);
        newColors.AddRange(lake_colors);
        newColors.AddRange(cloud_colors);
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
