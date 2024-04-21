using Godot;
using System;
using System.Collections.Generic;

[Tool]
public class LandMasses : Planet
{
    //颜色
    [Export]
    public Color[] waterColors = new Color[] { new Color("92e8c0"), new Color("4fa4b8"), new Color("2c354d") };
    [Export]
    public Color[] landColors = new Color[] { new Color("c8d45d"), new Color("63ab3f"), new Color("2f5753"), new Color("283540") };
    [Export]
    public Color[] cloudColors = new Color[] { new Color("dfe0e8"), new Color("a3a7c2"), new Color("686f99"), new Color("404973") };
    //噪点 选否会增大噪点
    [Export]
    public bool starKind = true;
    [Export]
    public Vector2 lightpoint = new Vector2(0.712f,0.396f);
    //是否预览
    [Export]
    public bool run = false;
    [Export]
    public Vector2 viewingOffset = new Vector2(0f, 0f);


    [Export]
    public NodePath LandMassesWaterPath;
    [Export]
    public NodePath LandMassesLandPath;
    [Export]
    public NodePath LandMassesCloudPath;


    private ColorRect _landMassesWater;
    private ColorRect _landMassesLand;
    private ColorRect _landMassesCloud;
    private ShaderMaterial _landMassesWaterMaterial;
    private ShaderMaterial _landMassesLandMaterial;
    private ShaderMaterial _landMassesCloudMaterial;


    /// <summary>
    /// 表示观测者距离星球的方向和距离 
    // 二维上的方向 以及3维的距离
    // +- 值表示方向 值大小表示距离 反正表示一个大概
    //实际中应该是曲线函数，无限逼近一个值 这里我设定了最大值1.1
    /// </summary>
    /// <param name="pos"></param>
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
        _landMassesWaterMaterial.SetShaderParam("viewing_pos", roataion);
        _landMassesLandMaterial.SetShaderParam("viewing_pos", roataion);
        _landMassesCloudMaterial.SetShaderParam("viewing_pos", roataion);
            


    }


    public override void _Ready()
    {
        _landMassesWater = (ColorRect)GetNode(LandMassesWaterPath);
        _landMassesLand = (ColorRect)GetNode(LandMassesLandPath);
        _landMassesCloud = (ColorRect)GetNode(LandMassesCloudPath);
        _landMassesWaterMaterial = (ShaderMaterial)_landMassesWater.Material;
        _landMassesLandMaterial = (ShaderMaterial)_landMassesLand.Material;
        _landMassesCloudMaterial = (ShaderMaterial)_landMassesCloud.Material;

        set_pixels(pixels);
        set_size(planetSize);
        set_dither(planetDither);
        set_light(lightpoint);
        _set_viewing_offset(Vector2.Zero);

        List<Color> newColors = new List<Color>();
        newColors.AddRange(waterColors);
        newColors.AddRange(landColors);
        newColors.AddRange(cloudColors);
        set_colors(newColors);
    }

    public override void _EnterTree()
    {
        if(Engine.EditorHint)
            _Ready();
    }


    public override void set_size(Vector2 size)
    {
        _landMassesWater.RectSize = size;
        _landMassesLand.RectSize = size;
        _landMassesCloud.RectSize = size;
    }



    public override void set_pixels(float _amount)
    {
        _landMassesWaterMaterial.SetShaderParam("pixels", _amount);
        _landMassesLandMaterial.SetShaderParam("pixels", _amount);
        _landMassesCloudMaterial.SetShaderParam("pixels", _amount);

        _landMassesWater.RectSize = new Vector2(_amount, _amount);
        _landMassesLand.RectSize = new Vector2(_amount, _amount);
        _landMassesCloud.RectSize = new Vector2(_amount, _amount);
    }

    public override void set_light(Vector2 pos)
    {
        _landMassesCloudMaterial.SetShaderParam("light_origin", pos);
        _landMassesLandMaterial.SetShaderParam("light_origin", pos);
        _landMassesWaterMaterial.SetShaderParam("light_origin", pos);
    }

    public override Vector2 _get_light(ShaderMaterial matel)
    {
        return (Vector2)matel.GetShaderParam("light_origin");

    }



    public override void set_seed(float sd)
    {
        var converted_seed = sd % 1000 / 100.0;
        _landMassesCloudMaterial.SetShaderParam("seed", converted_seed);        
        _landMassesWaterMaterial.SetShaderParam("seed", converted_seed);
        _landMassesLandMaterial.SetShaderParam("seed", converted_seed);
        _landMassesCloudMaterial.SetShaderParam("cloud_cover", GD.RandRange(0.35f, 0.6f));
    }


    public override void set_rotate(float r)
    {
        _landMassesCloudMaterial.SetShaderParam("rotation", r);
        _landMassesWaterMaterial.SetShaderParam("rotation", r);
        _landMassesLandMaterial.SetShaderParam("rotation", r);
    }

    public override void update_time(float t)
    {

        _landMassesCloudMaterial.SetShaderParam("time", t * get_multiplier(_landMassesCloudMaterial) * 0.001f);
        _landMassesWaterMaterial.SetShaderParam("time", t * get_multiplier(_landMassesWaterMaterial) * 0.002f);
        _landMassesLandMaterial.SetShaderParam("time", t * get_multiplier(_landMassesLandMaterial) * 0.002f);


       
    }

    public override void set_custom_time(float t)
    {
        _landMassesCloudMaterial.SetShaderParam("time", t * get_multiplier(_landMassesCloudMaterial));
        _landMassesWaterMaterial.SetShaderParam("time", t * get_multiplier(_landMassesWaterMaterial));
        _landMassesLandMaterial.SetShaderParam("time", t * get_multiplier(_landMassesLandMaterial));
    }


    public override void set_dither(bool d)
    {
        _landMassesWaterMaterial.SetShaderParam("should_dither", d);
    }


    public override bool get_dither()
    {
        return (bool)_landMassesWaterMaterial.GetShaderParam("should_dither");
    }

    private List<string> color_vars1 = new List<string> { "color1", "color2", "color3" };
    private List<string> color_vars2 = new List<string> { "col1", "col2", "col3", "col4" };
    private List<string> color_vars3 = new List<string> { "base_color", "outline_color", "shadow_base_color", "shadow_outline_color" };


    public override List<Color> get_colors()
    {
        List<Color> colors = new List<Color>();
        colors.AddRange(_get_colors_from_vars(_landMassesWaterMaterial, color_vars1));
        colors.AddRange(_get_colors_from_vars(_landMassesLandMaterial, color_vars2));
        colors.AddRange(_get_colors_from_vars(_landMassesCloudMaterial, color_vars3));
        return colors;
    }


    public override void set_colors(List<Color> colors)
    {
        List<Color> colors1 = new List<Color> { colors[0], colors[1], colors[2] };
        List<Color> colors2 = new List<Color> { colors[3], colors[4], colors[5], colors[6] };
        List<Color> colors3 = new List<Color> { colors[7], colors[8], colors[9], colors[10] };
        _set_colors_from_vars(_landMassesWaterMaterial, color_vars1, colors1);
        _set_colors_from_vars(_landMassesLandMaterial, color_vars2, colors2);
        _set_colors_from_vars(_landMassesCloudMaterial, color_vars3, colors3);
    }


    public override void randomize_colors()
    {
        var seed_colors = _generate_new_colorscheme(GD.Randi() % 2 + 3, (float)GD.RandRange(0.7f, 1.0f), (float)GD.RandRange(0.45f, 0.55f));
        List<Color> land_colors = new List<Color>();
        List<Color> water_colors = new List<Color>();
        List<Color> cloud_colors = new List<Color>();
        for(int i =0;i< 4;i++)
        {
            var new_col = seed_colors[0].Darkened(i / 4.0f);
            land_colors.Add(Color.FromHsv((new_col.h + (0.2f * (i / 4.0f))), new_col.s,new_col.v));
        }
        for (int i = 0; i < 3; i++)
        {
            var new_col = seed_colors[1].Darkened(i / 5.0f);
            water_colors.Add(Color.FromHsv((new_col.h + (0.1f * (i / 2.0f))), new_col.s, new_col.v));
        }
        for (int i = 0; i < 4; i++)
        {
            var new_col = seed_colors[2].Lightened((1.0f - (i / 4.0f)) * 0.8f);
            cloud_colors.Add(Color.FromHsv((new_col.h + (0.2f * (i / 4.0f))), new_col.s, new_col.v));
        }

        List<Color> newColors = new List<Color>();
        newColors.AddRange(land_colors);
        newColors.AddRange(water_colors);
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
