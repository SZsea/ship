using Godot;
using System;
using System.Collections.Generic;

[Tool]
public class GasPlanetLayers : Planet
{
    //是否预览
    [Export]
    public bool run = false;


    [Export]
    public NodePath GasLayersPath;
    [Export]
    public NodePath RingPath;


    private ColorRect _gasLayers;
    private ColorRect _ring;


    private ShaderMaterial _gasLayersMaterial;
    private ShaderMaterial _ringMaterial;


    //噪点 选否会增大噪点
    [Export]
    public bool starKind = true;
    [Export]
    public Vector2 lightpoint = new Vector2(0.712f, 0.396f);

    public override void _Ready()
    {
        _gasLayers = (ColorRect)GetNode(GasLayersPath);
        _ring = (ColorRect)GetNode(RingPath);

        _gasLayersMaterial = (ShaderMaterial)_gasLayers.Material;
        _ringMaterial = (ShaderMaterial)_ring.Material;

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
        _gasLayersMaterial.SetShaderParam("pixels", _amount);
        _ringMaterial.SetShaderParam("pixels", _amount * 3f );

    }

    public override void set_size(Vector2 size)
    {
        _gasLayers.RectSize = size;
        _ring.RectSize = size * 3f;
        _ring.RectPosition = -size;
    }


    public override void set_light(Vector2 pos)
    {
        _gasLayersMaterial.SetShaderParam("light_origin", pos);
        _ringMaterial.SetShaderParam("light_origin", pos);
    }


    public override void set_seed(float number)
    {
        var converted_seed = number % 1000.0f / 100.0f;
        _gasLayersMaterial.SetShaderParam("seed", converted_seed);
        _ringMaterial.SetShaderParam("seed", converted_seed);
    }


    public override void set_rotate(float number)
    {
        _gasLayersMaterial.SetShaderParam("rotation", number);
        _ringMaterial.SetShaderParam("rotation", number + 0.7f);
    }

    public override void update_time(float t)
    {
        _gasLayersMaterial.SetShaderParam("time", t * get_multiplier(_gasLayersMaterial) * 0.004f);
        _ringMaterial.SetShaderParam("time", t * 314.15f * 0.004f);
    }

    public override void set_custom_time(float t)
    {
        _gasLayersMaterial.SetShaderParam("time", t * get_multiplier(_gasLayersMaterial));
        _ringMaterial.SetShaderParam("time", t * 314.15f * (float)_ringMaterial.GetShaderParam("time_speed") * 0.5f);
    }

    public override void set_dither(bool d)
    {
        _gasLayersMaterial.SetShaderParam("should_dither", d);
    }

    public override bool get_dither()
    {
        return (bool)_gasLayersMaterial.GetShaderParam("should_dither");
    }

    public override List<Color> get_colors()
    {
        List<Color> newColor = new List<Color>();
        newColor.AddRange(_get_colors_from_gradient(_gasLayersMaterial, "colorscheme"));
        newColor.AddRange(_get_colors_from_gradient(_ringMaterial, "dark_colorscheme"));

        return newColor;
    }

    public override void set_colors(List<Color> colors)
    {
        List<Color> colors1 = new List<Color> { colors[0], colors[2], colors[1] };
        List<Color> colors2 = new List<Color> { colors[3], colors[5], colors[1] };


        _set_colors_from_gradient(_gasLayersMaterial, "colorscheme", colors1);
        _set_colors_from_gradient(_ringMaterial, "colorscheme", colors1);


        _set_colors_from_gradient(_gasLayersMaterial, "dark_colorscheme", colors2);
        _set_colors_from_gradient(_ringMaterial, "dark_colorscheme", colors2);

    }


    public override void randomize_colors()
    {
        List<Color> seed_colors = _generate_new_colorscheme(6 + GD.Randi() % 4f,(float)GD.RandRange(0.3,0.55),1.4f);

        List<Color> cols = new List<Color>();

        for (int i =0;i< 6;i++)
        {
            Color new_col = seed_colors[i].Darkened(i / 7.0f);
            new_col = new_col.Lightened((1.0f - (i / 6.0f)) * 0.3f);
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
