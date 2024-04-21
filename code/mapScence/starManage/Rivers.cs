using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;


[Tool]
public class Rivers : Planet
{
    //是否预览
    [Export]
    public bool run = false;
    [Export]
    public NodePath RiverLandPath;
    [Export]
    public NodePath RiverCloudPath;

    private ColorRect _riverLand;
    private ColorRect _riverCloud;
    private ShaderMaterial _riverLandMaterial;
    private ShaderMaterial _riverCloudMaterial;

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
      //  _riverLandMaterial.SetShaderParam("viewing_pos", roataion);
       // _riverCloudMaterial.SetShaderParam("viewing_pos", roataion);



    }


    public override void _Ready()
    {
        _riverLand = (ColorRect)GetNode(RiverLandPath);
        _riverCloud = (ColorRect)GetNode(RiverCloudPath);

        _riverLandMaterial = (ShaderMaterial)_riverLand.Material;
        _riverCloudMaterial = (ShaderMaterial)_riverCloud.Material;

        set_pixels(pixels);
        set_size(planetSize);
        set_dither(planetDither);
        set_light(lightpoint);
        _set_viewing_offset(Vector2.Zero);
    }

    public override void _EnterTree()
    {
        if (Engine.EditorHint)
            _Ready();
    }


    public override void set_pixels(float _amount)
    {
        _riverLandMaterial.SetShaderParam("pixels", _amount);
        _riverCloudMaterial.SetShaderParam("pixels", _amount);

    }

    public override void set_size(Vector2 size)
    {
        _riverLand.RectSize = size;
        _riverCloud.RectSize = size;
        //GD.Print(size);
    }


    public override void set_light(Vector2 pos)
    {
        _riverLandMaterial.SetShaderParam("light_origin", pos);
        _riverCloudMaterial.SetShaderParam("light_origin", pos);
    }

    public override void set_seed(float number)
    {
        var converted_seed = number % 1000 / 100.0;
        _riverLandMaterial.SetShaderParam("seed", converted_seed);
        _riverCloudMaterial.SetShaderParam("seed", converted_seed);
        _riverCloudMaterial.SetShaderParam("cloud_cover", (float)GD.RandRange(0.35f, 0.6f));
    }

    public override void set_rotate(float number)
    {
        _riverLandMaterial.SetShaderParam("rotation", number);
        _riverCloudMaterial.SetShaderParam("rotation", number);
    }

    public override void update_time(float t)
    {
        //base.update_time(t);
        _riverCloudMaterial.SetShaderParam("time", t * get_multiplier(_riverCloudMaterial) * 0.01f);
        _riverLandMaterial.SetShaderParam("time", t * get_multiplier(_riverLandMaterial) * 0.02f);

    }

    public override void set_custom_time(float t)
    {
        _riverCloudMaterial.SetShaderParam("time", t * get_multiplier(_riverCloudMaterial) * 0.5f);
        _riverLandMaterial.SetShaderParam("time", t * (1.0f / get_multiplier(_riverLandMaterial)));

    }

    public override void set_dither(bool d)
    {
        _riverLandMaterial.SetShaderParam("should_dither", d);
    }

    public override bool get_dither()
    {
        return (bool)_riverLandMaterial.GetShaderParam("should_dither");
    }


    private List<string> color_vars1 = new List<string> { "col1", "col2", "col3", "col4", "river_col", "river_col_dark" };
    private List<string> color_vars2 = new List<string> { "base_color", "outline_color", "shadow_base_color", "shadow_outline_color" };

    public override List<Color> get_colors()
    {
        List<Color> colors = new List<Color>();
        colors.AddRange(_get_colors_from_vars(_riverLandMaterial, color_vars1));
        colors.AddRange(_get_colors_from_vars(_riverCloudMaterial, color_vars2));
        return colors;
    }

    public override void set_colors(List<Color> colors)
    {
        List<Color> colors1 = new List<Color> { colors[0], colors[5], colors[1] };
        List<Color> colors2 = new List<Color> { colors[6], colors[9], colors[1] };
        _set_colors_from_vars(_riverLandMaterial, color_vars1, colors1);
        _set_colors_from_vars(_riverCloudMaterial, color_vars2, colors2);
    }


    public override void randomize_colors()
    {
        List<Color> seed_colors = _generate_new_colorscheme(GD.Randi() % 2 + 3, (float)GD.RandRange(0.7f, 1.0f), (float)GD.RandRange(0.45f, 0.55f));

        List<Color> land_colors = new List<Color>();
        List<Color> river_colors = new List<Color>();
        List<Color> cloud_colors = new List<Color>();

        for(int i = 0;i < 4;i++)
        {
            var new_col = seed_colors[0].Darkened(i / 4.0f);
            land_colors.Append(Color.FromHsv(new_col.h + (0.2f * (i / 4.0f)), new_col.s, new_col.v));
        }

        for(int i = 0;i < 2;i++)
        {
            var new_col = seed_colors[1].Darkened(i / 2.0f);
            river_colors.Append(Color.FromHsv(new_col.h + (0.2f * (i / 2.0f)), new_col.s, new_col.v));
        }

        for (int i = 0; i < 4; i++)
        {
            var new_col = seed_colors[2].Lightened((1.0f - (i / 4.0f)) * 0.8f);
            cloud_colors.Append(Color.FromHsv(new_col.h + (0.2f* (i / 4.0f)), new_col.s, new_col.v));
        }

        List<Color> newColors = new List<Color>();
        newColors.AddRange(land_colors);
        newColors.AddRange(river_colors);
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
