using Godot;
using System;
using System.Collections.Generic;
using System.Linq;


[Tool]
public class NoAtmosphere : Planet
{
    //是否预览
    [Export]
    public bool run = false;
    [Export]
    public NodePath PlanetUnderPath;
    [Export]
    public NodePath CratersPath;

    private ColorRect _planetUnder;
    private ColorRect _craters;
    private ShaderMaterial _planetUnderMaterial;
    private ShaderMaterial _cratersMaterial;


    //噪点 选否会增大噪点
    [Export]
    public bool starKind = true;
    [Export]
    public Vector2 lightpoint = new Vector2(0.712f, 0.396f);


    public override void _set_viewing_offset(Vector2 pos)
    {

        Vector2 roataion = pos;

        float maxAngle = 1.1f;
        if (pos.Length() > new Vector2(maxAngle, maxAngle).Length())
        {
            return;
        }

        if (roataion.x > maxAngle)
        {
            roataion.x = maxAngle;
        }
        if (roataion.y > maxAngle)
        {
            roataion.y = maxAngle;
        }
        if (roataion.x < -maxAngle)
        {
            roataion.x = -maxAngle;
        }
        if (roataion.y < -maxAngle)
        {
            roataion.y = -maxAngle;
        }
        // GD.Print("_set_viewing_offset" + roataion);
       // _planetUnderMaterial.SetShaderParam("viewing_pos", roataion);
      //  _cratersMaterial.SetShaderParam("viewing_pos", roataion);



    }


    public override void _Ready()
    {
        _planetUnder = (ColorRect)GetNode(PlanetUnderPath);
        _craters = (ColorRect)GetNode(CratersPath);

        _planetUnderMaterial = (ShaderMaterial)_planetUnder.Material;
        _cratersMaterial = (ShaderMaterial)_craters.Material;

        set_pixels(pixels);
        set_size(planetSize);
        set_dither(planetDither);
        set_light(lightpoint);
        // _set_viewing_offset(Vector2.Zero);
    }

    public override void _EnterTree()
    {
        if (Engine.EditorHint)
            _Ready();
    }

    public override void set_pixels(float _amount)
    {
        _planetUnderMaterial.SetShaderParam("pixels", _amount);
        _cratersMaterial.SetShaderParam("pixels", _amount);

    }

    public override void set_size(Vector2 size)
    {
        _planetUnder.RectSize = size;
        _craters.RectSize = size;
    }


    public override void set_light(Vector2 pos)
    {
        _planetUnderMaterial.SetShaderParam("light_origin", pos);
        _cratersMaterial.SetShaderParam("light_origin", pos);
    }

    public override void set_seed(float number)
    {
        var converted_seed = number % 1000f/ 100.0f;
        _planetUnderMaterial.SetShaderParam("seed", converted_seed);
        _cratersMaterial.SetShaderParam("seed", converted_seed);

    }


    public override void set_rotate(float number)
    {
        _planetUnderMaterial.SetShaderParam("rotation", number);
        _cratersMaterial.SetShaderParam("rotation", number);
    }


    public override void update_time(float t)
    {
        _planetUnderMaterial.SetShaderParam("time", t * get_multiplier(_planetUnderMaterial) * 0.01f);
        _cratersMaterial.SetShaderParam("time", t * get_multiplier(_cratersMaterial) * 0.02f);

    }

    public override void set_custom_time(float number)
    {
        _planetUnderMaterial.SetShaderParam("time", number * get_multiplier(_planetUnderMaterial));
        _cratersMaterial.SetShaderParam("time", number * get_multiplier(_cratersMaterial));
    }

    public override void set_dither(bool d)
    {
        _planetUnderMaterial.SetShaderParam("should_dither", d);

    }

    public override bool get_dither()
    {
        return (bool)_planetUnderMaterial.GetShaderParam("should_dither");
    }


    private List<string> color_vars1 = new List<string> { "color1", "color2", "color3" };
    private List<string> color_vars2 = new List<string> { "color1", "color2" };


    public override List<Color> get_colors()
    {
        List<Color> colors = new List<Color>();
        colors.AddRange(_get_colors_from_vars(_planetUnderMaterial, color_vars1));
        colors.AddRange(_get_colors_from_vars(_cratersMaterial, color_vars2));
        return colors;
    }

    public override void set_colors(List<Color> colors)
    {
        List<Color> colors1 = new List<Color> { colors[0], colors[2], colors[1] };
        List<Color> colors2 = new List<Color> { colors[3], colors[4], colors[1] };
        _set_colors_from_vars(_planetUnderMaterial, color_vars1, colors1);
        _set_colors_from_vars(_cratersMaterial, color_vars2, colors2);
    }

    public override void randomize_colors()
    {
        List<Color> seed_colors = _generate_new_colorscheme(3.0f + GD.Randi() % 2.0f,(float)GD.RandRange(0.3f, 0.6f), 0.7f);

        List<Color> cols = new List<Color>();

        for(int i =0;i< 3;i++)
        {
            var new_col = seed_colors[i].Darkened(i / 3.0f);
            new_col = new_col.Lightened((1.0f - (i / 3.0f)) * 0.2f);
            cols.Append(new_col);
        }

        List<Color> newColors = new List<Color>();
        newColors.AddRange(cols);
        newColors.Add(cols[1]);
        newColors.Add(cols[2]);


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
