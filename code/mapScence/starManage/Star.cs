using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

[Tool]
public class Star : Planet
{

    //颜色
    [Export]
    public Color[] gradientColor1 = new Color[] { new Color("f5ffe8"), new Color("ffd832"), new Color("ff823b"), new Color("7c191a") };
    [Export]
    public Color[] gradientColor2 = new Color[] { new Color("f5ffe8"), new Color("77d6c1"), new Color("1c92a7"), new Color("033e5e") };
    //颜色种类
    [Export]
    public bool starKind = true;
    //是否预览
    [Export]
    public bool run = false;

    [Export]
    public NodePath StarBackgroundPath;
    [Export]
    public NodePath StarPath;
    [Export]
    public NodePath StarFlaresPath;


    private ColorRect _starBackground;
    private ColorRect _star;
    private ColorRect _starFlares;
    private ShaderMaterial _starBackgroundMaterial;
    private ShaderMaterial _starMaterial;
    private ShaderMaterial _starFlaresMaterial;

    private Gradient _starcolor1 = new Gradient();
    private Gradient _starcolor2 = new Gradient();
    private Gradient _starflarecolor1 = new Gradient();
    private Gradient _starflarecolor2 = new Gradient();


    public override void _Ready()
    {
        _starBackground = (ColorRect)GetNode(StarBackgroundPath);
        _star = (ColorRect)GetNode(StarPath);
        _starFlares = (ColorRect)GetNode(StarFlaresPath);
        _starBackgroundMaterial = (ShaderMaterial)_starBackground.Material;
        _starMaterial = (ShaderMaterial)_star.Material;
        _starFlaresMaterial = (ShaderMaterial)_starFlares.Material;

        _starcolor1.Offsets = new float[] { 0f, 0.33f, 0.66f, 1.0f };
        _starcolor2.Offsets = new float[] { 0f, 0.33f, 0.66f, 1.0f };
        _starflarecolor1.Offsets = new float[] { 0f, 1.0f };
        _starflarecolor2.Offsets = new float[] { 0f, 1.0f };

        _starcolor1.Colors = gradientColor1;
        _starcolor2.Colors = gradientColor2;

        _starflarecolor1.Colors = new Color[] { gradientColor1[1], gradientColor1[0] };
        _starflarecolor2.Colors = new Color[] { gradientColor2[1], gradientColor2[0] };
        set_pixels(pixels);
        set_size(planetSize);
        set_dither(planetDither);
        _set_colors(starKind);
        //randomize_colors();
    }

    public override void _EnterTree()
    {
        if (Engine.EditorHint)
            _Ready();
    }

    public override void set_size(Vector2 size)
    {
        _starBackground.RectSize = size * 2;
        _starFlares.RectSize = size * 2;
        _star.RectSize = size;
        _starBackground.RectPosition = new Vector2(-size.x, -size.y) * 0.5f;
        _starFlares.RectPosition = new Vector2(-size.x, -size.y) * 0.5f;
    }

    public override void set_pixels(float amount)
    {

        _starBackgroundMaterial.SetShaderParam("pixels", amount * relative_scale);
        _starMaterial.SetShaderParam("pixels", amount);
        _starFlaresMaterial.SetShaderParam("pixels", amount * relative_scale);

        _starBackground.RectSize = new Vector2(amount, amount) * relative_scale * 2;
        _starFlares.RectSize = new Vector2(amount, amount) * relative_scale * 2;
        _star.RectSize = new Vector2(amount, amount);

        _starBackground.RectPosition = new Vector2(-amount, -amount) * 0.5f;
        _starFlares.RectPosition = new Vector2(-amount, -amount) * 0.5f;
    }

    public override void set_light(Vector2 _pos)
    {

    }


    public override void set_seed(float sd)
    {
        var converted_seed = sd % 1000f / 100.0f;

        _starBackgroundMaterial.SetShaderParam("seed", converted_seed);
        _starMaterial.SetShaderParam("seed", converted_seed);
        _starFlaresMaterial.SetShaderParam("seed", converted_seed);

    }

    public void _set_colors(bool starKind)//this is just a little extra function to show some different possible stars
    {
        if(starKind)
        {
            GradientTexture gradientTexture1 = (GradientTexture)_starMaterial.GetShaderParam("colorramp");
            gradientTexture1.Gradient = _starcolor1;
            GradientTexture gradientTexture2 = (GradientTexture)_starFlaresMaterial.GetShaderParam("colorramp");
            gradientTexture2.Gradient = _starflarecolor1;

        }else
        {
            GradientTexture gradientTexture1 = (GradientTexture)_starMaterial.GetShaderParam("colorramp");
            gradientTexture1.Gradient = _starcolor2;
            GradientTexture gradientTexture2 = (GradientTexture)_starFlaresMaterial.GetShaderParam("colorramp");
            gradientTexture2.Gradient = _starflarecolor2;
        }
        
    }

    public override void set_rotate(float r)
    {
        _starBackgroundMaterial.SetShaderParam("rotation", r);
        _starMaterial.SetShaderParam("rotation", r);
        _starFlaresMaterial.SetShaderParam("rotation", r);
    }

    public override void update_time(float t)
    {
        //base.update_time(t);
        _starBackgroundMaterial.SetShaderParam("time", t * get_multiplier(_starBackgroundMaterial) * 0.01f);
        _starMaterial.SetShaderParam("time", t * get_multiplier(_starMaterial) * 0.005f);
        _starFlaresMaterial.SetShaderParam("time", t * get_multiplier(_starFlaresMaterial) * 0.015f);
        
    }

    public override void set_custom_time(float t)
    {
        _starBackgroundMaterial.SetShaderParam("time", t * get_multiplier(_starBackgroundMaterial));
        _starMaterial.SetShaderParam("time", t * ( 1.0f / get_multiplier(_starMaterial)));
        _starFlaresMaterial.SetShaderParam("time", t * get_multiplier(_starFlaresMaterial));
    }


    public override void set_dither(bool d)
    {
        _starMaterial.SetShaderParam("should_dither", d);
        _starFlaresMaterial.SetShaderParam("should_dither", d);
    }

    public override bool get_dither()
    {
        return (bool)_starMaterial.GetShaderParam("should_dither");
    }

    public override List<Color> get_colors()
    {
        List<Color> colors = new List<Color>();
        colors.AddRange(_get_colors_from_vars(_starBackgroundMaterial,new List<string>{ "color"}));
        colors.AddRange(_get_colors_from_gradient(_starMaterial, "colorramp"));
        colors.AddRange(_get_colors_from_gradient(_starFlaresMaterial, "colorramp"));
        return colors;
    }


    public override void set_colors(List<Color> colors)
    {
        List<Color> cols1 = new List<Color>();
        cols1.Add(colors[1]);
        cols1.Add(colors[2]);
        cols1.Add(colors[3]);
        cols1.Add(colors[4]);
        List<Color> cols2 = new List<Color>();
        cols2.Add(colors[5]);
        cols2.Add(colors[6]);
        
        _starBackgroundMaterial.SetShaderParam("color", colors[0]);
        _set_colors_from_gradient(_starMaterial, "colorramp", cols1);
        _set_colors_from_gradient(_starFlaresMaterial, "colorramp", cols2);
    }


    public override void randomize_colors()
    {
        var seed_colors = _generate_new_colorscheme(4f, (float)GD.RandRange(0.2, 0.4), 2.0f);
       // GD.Print("seed_colors" + seed_colors.Count);
        List<Color> cols = new List<Color>();
        for(int i = 0;i< 4;i++)
        {
            var new_col = seed_colors[i].Darkened((i / 4.0f) * 0.9f);
            new_col = new_col.Lightened((1.0f - (i / 4.0f)) * 0.8f);
            cols.Add(new_col);
        }
        cols[0] = cols[0].Lightened(0.8f);
        List<Color> newcols = new List<Color>();
        newcols.Add(cols[0]);
        newcols.AddRange(cols);
        newcols.Add(cols[0]);
        newcols.Add(cols[1]);
        GD.Print("newcols" + newcols.Count);
        set_colors(newcols);


    }

    public override void _Process(float delta)
    {
        if(run)
        {
            base._Process(delta);
        }
        
    }


}
