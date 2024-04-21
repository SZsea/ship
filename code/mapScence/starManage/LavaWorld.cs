using Godot;
using System;
using System.Collections.Generic;

[Tool]
public class LavaWorld : Planet
{
    //颜色
    [Export]
    public List<Color> LavaWorldColors = new List<Color> { new Color("8f4d57"), new Color("52333f"), new Color("3d2936"), new Color("52333f"), new Color("3d2936"), new Color("ff8933"), new Color("e64539"), new Color("ad2f45") };


    [Export]
    public Vector2 lightpoint = new Vector2(0.712f, 0.396f);
    //是否预览
    [Export]
    public bool run = false;

    [Export]
    public NodePath LavaWorldPlanetUnderPath;
    [Export]
    public NodePath LavaWorldCratersPath;
    [Export]
    public NodePath LavaWorldLavaRiversPath;


    private ColorRect _lavaWorldPlanetUnder;
    private ColorRect _lavaWorldCraters;
    private ColorRect _lavaWorldLavaRivers;
    private ShaderMaterial _lavaWorldPlanetUnderMaterial;
    private ShaderMaterial _lavaWorldCratersMaterial;
    private ShaderMaterial _lavaWorldLavaRiversMaterial;
    public override void _Ready()
    {
        _lavaWorldPlanetUnder = (ColorRect)GetNode(LavaWorldPlanetUnderPath);
        _lavaWorldCraters = (ColorRect)GetNode(LavaWorldCratersPath);
        _lavaWorldLavaRivers = (ColorRect)GetNode(LavaWorldLavaRiversPath);
        _lavaWorldPlanetUnderMaterial = (ShaderMaterial)_lavaWorldPlanetUnder.Material;
        _lavaWorldCratersMaterial = (ShaderMaterial)_lavaWorldCraters.Material;
        _lavaWorldLavaRiversMaterial = (ShaderMaterial)_lavaWorldLavaRivers.Material;
        set_pixels(pixels);
        set_size(planetSize);
        set_dither(planetDither);
        set_light(lightpoint);
        set_colors(LavaWorldColors);
    }


    public override void _EnterTree()
    {
        if (Engine.EditorHint)
            _Ready();
    }

    public override void set_size(Vector2 size)
    {
        _lavaWorldPlanetUnder.RectSize = size;
        _lavaWorldCraters.RectSize = size;
        _lavaWorldLavaRivers.RectSize = size;
    }

    public override void set_pixels(float _amount)
    {
        _lavaWorldPlanetUnderMaterial.SetShaderParam("pixels", _amount);
        _lavaWorldCratersMaterial.SetShaderParam("pixels", _amount);
        _lavaWorldLavaRiversMaterial.SetShaderParam("pixels", _amount);

        _lavaWorldPlanetUnder.RectSize = new Vector2(_amount, _amount);
        _lavaWorldCraters.RectSize = new Vector2(_amount, _amount);
        _lavaWorldLavaRivers.RectSize = new Vector2(_amount, _amount);
    }

    public override void set_light(Vector2 pos)
    {
        _lavaWorldPlanetUnderMaterial.SetShaderParam("light_origin", pos);
        _lavaWorldCratersMaterial.SetShaderParam("light_origin", pos);
        _lavaWorldLavaRiversMaterial.SetShaderParam("light_origin", pos);
    }

    public override void set_seed(float sd)
    {
        var converted_seed = sd % 1000f / 100.0f;
        _lavaWorldPlanetUnderMaterial.SetShaderParam("seed", converted_seed);
        _lavaWorldCratersMaterial.SetShaderParam("seed", converted_seed);
        _lavaWorldLavaRiversMaterial.SetShaderParam("seed", converted_seed);
    }


    public override void set_rotate(float r)
    {

        _lavaWorldPlanetUnderMaterial.SetShaderParam("rotation", r);
        _lavaWorldCratersMaterial.SetShaderParam("rotation", r);
        _lavaWorldLavaRiversMaterial.SetShaderParam("rotation", r);
    }

    public override void update_time(float t)
    {
        _lavaWorldPlanetUnderMaterial.SetShaderParam("time", t * get_multiplier(_lavaWorldPlanetUnderMaterial) * 0.02f);
        _lavaWorldCratersMaterial.SetShaderParam("time", t * get_multiplier(_lavaWorldCratersMaterial) * 0.02f);
        _lavaWorldLavaRiversMaterial.SetShaderParam("time", t * get_multiplier(_lavaWorldLavaRiversMaterial) * 0.02f);
    }


    public override void set_custom_time(float t)
    {
        _lavaWorldPlanetUnderMaterial.SetShaderParam("time", t * get_multiplier(_lavaWorldPlanetUnderMaterial));
        _lavaWorldCratersMaterial.SetShaderParam("time", t * get_multiplier(_lavaWorldCratersMaterial));
        _lavaWorldLavaRiversMaterial.SetShaderParam("time", t * get_multiplier(_lavaWorldLavaRiversMaterial));
    }


    public override void set_dither(bool d)
    {
        _lavaWorldPlanetUnderMaterial.SetShaderParam("should_dither", d);
    }

    public override bool get_dither()
    {
        return (bool)_lavaWorldPlanetUnderMaterial.GetShaderParam("should_dither");
    }

    private List<string> color_vars1 = new List<string> { "color1", "color2", "color3" };
    private List<string> color_vars2 = new List<string> { "color1", "color2" };
    private List<string> color_vars3 = new List<string> { "color1", "color2", "color3" };


    public override List<Color> get_colors()
    {
        List<Color> colors = new List<Color>();
        colors.AddRange(_get_colors_from_vars(_lavaWorldPlanetUnderMaterial, color_vars1));
        colors.AddRange(_get_colors_from_vars(_lavaWorldCratersMaterial, color_vars2));
        colors.AddRange(_get_colors_from_vars(_lavaWorldLavaRiversMaterial, color_vars3));
        return colors;
    }

    public override void set_colors(List<Color> colors)
    {
        List<Color> colors1 = new List<Color> { colors[0], colors[1], colors[2] };
        List<Color> colors2 = new List<Color> { colors[3], colors[4] };
        List<Color> colors3 = new List<Color> { colors[5], colors[6], colors[7] };
        _set_colors_from_vars(_lavaWorldPlanetUnderMaterial, color_vars1, colors1);
        _set_colors_from_vars(_lavaWorldCratersMaterial, color_vars2, colors2);
        _set_colors_from_vars(_lavaWorldLavaRiversMaterial, color_vars3, colors3);
    }

    public override void randomize_colors()
    {
        var seed_colors = _generate_new_colorscheme(GD.Randi() % 3 + 2, (float)GD.RandRange(0.6f, 1.0f), (float)GD.RandRange(0.7f, 0.8f));
        List<Color> land_colors = new List<Color>();
        List<Color> lava_colors = new List<Color>();
        for (int i = 0; i < 3; i++)
        {
            var new_col = seed_colors[0].Darkened(i / 3.0f);
            land_colors.Add(Color.FromHsv((new_col.h + (0.2f * (i / 4.0f))), new_col.s, new_col.v));
        }
        for (int i = 0; i < 3; i++)
        {
            var new_col = seed_colors[1].Darkened(i / 3.0f);
            lava_colors.Add(Color.FromHsv((new_col.h + (0.2f * (i / 3.0f))), new_col.s, new_col.v));
        }


        List<Color> newColors = new List<Color>();
        newColors.AddRange(land_colors);
        newColors.Add(land_colors[1]);
        newColors.Add(land_colors[2]);
        newColors.AddRange(lava_colors);
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
