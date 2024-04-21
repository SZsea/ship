using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

[Tool]
public class Planet : Control
{

    public float time = 1000;
    public bool override_time = false;
    public List<Color> original_colors;


    [Export]
    public float relative_scale = 1.0f;
    [Export]
    public float gui_zoom = 1.0f;

    [Export]
    public float pixels = 100.0f;
    [Export]
    public Vector2 planetSize = new Vector2(200f,200f);
    [Export]
    public bool planetDither = true;



    public override void _Ready()
    {
        
    }

    public virtual void set_size(Vector2 size)
    {

    }


    public virtual void set_pixels(float _amount)
    {

    }

    public virtual void set_light(Vector2 pos)
    {

    }

    public virtual void set_seed(float number)
    {

    }

    public virtual void set_rotate(float number)
    {

    }

    public virtual void update_time(float t)
    {

    }

    public virtual void set_custom_time(float number)
    {

    }

    public float get_multiplier(ShaderMaterial mat)
    {
        float a = (float)mat.GetShaderParam("size") * 2.0f;
        float b = (float)mat.GetShaderParam("time_speed");
        return Mathf.Round(a/b);
    }

    public override void _Process(float delta)
    {
        time += delta;
        if (!override_time)
            update_time(time);
    }


    public virtual void set_dither(bool d)
    {

    }

    public virtual bool get_dither()
    {
        return true;
    }

    public virtual List<Color> get_colors()
    {
        return new List<Color>();
    }

    public virtual void set_colors(List<Color> colors)
    {

    }

    public virtual Color[] _get_colors_from_gradient(ShaderMaterial mat,string grad_var)
    {
        GradientTexture gradientTexture = (GradientTexture)mat.GetShaderParam(grad_var);

        return gradientTexture.Gradient.Colors;
    }

    public virtual void _set_colors_from_gradient(ShaderMaterial mat, string grad_var, List<Color> colors)
    {
        GradientTexture gradientTexture = (GradientTexture)mat.GetShaderParam(grad_var);
        gradientTexture.Gradient.Colors = colors.ToArray();
    }

    public virtual List<Color> _get_colors_from_vars(ShaderMaterial mat, List<string> vars)
    {
        List<Color> colors = new List<Color>();
        for(int i = 0;i < vars.Count; i++ )
        {
            Color color = (Color)mat.GetShaderParam(vars[i]);
            colors.Add(color);
        }
        return colors;
    }

    public virtual void _set_colors_from_vars(ShaderMaterial mat, List<string> vars, List<Color> colors)
    {

        for (int i = 0; i < vars.Count; i++)
        {
            mat.SetShaderParam(vars[i], colors[i]);
        }
    }

    public virtual void randomize_colors()
    {

    }

    //随机产生一个颜色列
    //Using ideas from https://www.iquilezles.org/www/articles/palettes/palettes.htm
    public virtual List<Color> _generate_new_colorscheme(float n_colors, float hue_diff = 0.9f, float saturation = 0.5f)
    {
        List<Color> cols = new List<Color>();
        // var a = Vector3(rand_range(0.0, 0.5), rand_range(0.0, 0.5), rand_range(0.0, 0.5));
        var a = new Vector3(0.5f, 0.5f, 0.5f);
        // var b = Vector3(rand_range(0.1, 0.6), rand_range(0.1, 0.6), rand_range(0.1, 0.6))
        var b = new Vector3(0.5f, 0.5f, 0.5f) * saturation;
        var c = new Vector3((float)GD.RandRange(0.5f, 1.5f), (float)GD.RandRange(0.5f, 1.5f), (float)GD.RandRange(0.5f, 1.5f)) * hue_diff;
        var d = new Vector3((float)GD.RandRange(0, 1.0f), (float)GD.RandRange(0, 1.0f), (float)GD.RandRange(0, 1.0f)) * (float)GD.RandRange(1.0f, 3.0f);

        var n = n_colors - 1.0f;
        n = n > 1f ? n : 1f;
        float[] range = { 0f, n_colors, 1f };
        for(int i =0; i < n_colors; i++)
        {
            var vec3 = new Vector3
            {
                x = a.x + b.x * Mathf.Cos(6.28318f * (c.x * (float)(i / n) + d.x)),
                y = a.y + b.y * Mathf.Cos(6.28318f * (c.y * (float)(i / n) + d.y)),
                z = a.z + b.z * Mathf.Cos(6.28318f * (c.z * (float)(i / n) + d.z))
            };

            cols.Add(new Color(vec3.x, vec3.y, vec3.z));
        }

        return cols;
    }

    public virtual void _set_viewing_offset(Vector2 pos)
    {

    }


    public virtual Vector2 _get_light(ShaderMaterial matel)
    {
        return Vector2.Zero;
    }
}
