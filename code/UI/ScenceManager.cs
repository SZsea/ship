using Godot;
using System;
using System.Linq;

public class ScenceManager : CanvasLayer
{

    [Export]
    public NodePath ColorRectPath;


    private ColorRect _ColorRect;


    private ShaderMaterial _ColorRectMaterial;


    public override void _Ready()
    {
        _ColorRect = (ColorRect)GetNode(ColorRectPath);

        _ColorRectMaterial = (ShaderMaterial)_ColorRect.Material;
        _ColorRectMaterial.SetShaderParam("dissolve_amount", 0f);

    }
    public enum ScenceChangdeMode
    {
        Circle,
        Curtains,
        Diagonal,
        Horizontal,
        Radial,
        Scribbles,
        Squares,
        Vertical

    }

    private Texture givenTexture(ScenceChangdeMode mode)
    {
        StreamTexture texture = new StreamTexture();
        switch(mode)
        {
            case ScenceChangdeMode.Circle:
                {
                    texture = GD.Load<StreamTexture>("res://art/theme/circle.png");
                    return (texture);
                }
            case ScenceChangdeMode.Curtains:
                {
                    texture = GD.Load<StreamTexture>("res://art/theme/curtains.png");
                    return (texture);
                }
            case ScenceChangdeMode.Diagonal:
                {
                    texture = GD.Load<StreamTexture>("res://art/theme/diagonal.png");
                    return (texture);

                }
            case ScenceChangdeMode.Horizontal:
                {
                    texture = GD.Load<StreamTexture>("res://art/theme/horizontal.png");
                    return (texture);

                }
            case ScenceChangdeMode.Radial:
                {
                    texture = GD.Load<StreamTexture>("res://art/theme/radial.png");
                    return (texture);
                }
            case ScenceChangdeMode.Scribbles:
                {
                    texture = GD.Load<StreamTexture>("res://art/theme/scribbles.png");
                    return (texture);
                }
            case ScenceChangdeMode.Squares:
                {
                    texture = GD.Load<StreamTexture>("res://art/theme/squares.png");
                    return (texture);
                }
            case ScenceChangdeMode.Vertical:
                {
                    texture = GD.Load<StreamTexture>("res://art/theme/vertical.png");
                    return (texture);
                }

        }
        return null;
    }


    private Color[] colorRanger = new Color[] { Godot.Color.ColorN("black") };

    public void initWith()
    {
        int i = (int)(GD.Randf() * 1.99f)  + 5  ;
        ScenceChangdeMode newmode = (ScenceChangdeMode)i;
        _ColorRectMaterial.SetShaderParam("dissolve_texture", givenTexture(newmode));

        //int j = (int)(GD.Randf() * 1.99f);
        //bool fade = i > 1 ? true : false;
        //_ColorRectMaterial.SetShaderParam("fade", fade);

        int f = (int)(GD.Randf() * 1.99f);
        bool inverted = i > 1 ? true : false;
        _ColorRectMaterial.SetShaderParam("inverted", inverted);

        int c = (int)(GD.Randf() * (colorRanger.Count() - 0.01f));
        //GD.Print("22222222" + c);
        _ColorRectMaterial.SetShaderParam("fade_color", colorRanger[c]);

        start();

    }
    private bool _start = false;
    private float _time = 1f;

    public override void _Process(float delta)
    {
        if(_start && _time >= 0)
        {
            _ColorRectMaterial.SetShaderParam("dissolve_amount",_time);
            _time = _time - 0.04f;
            //GD.Print(_time);
        }

        if(_start && _time < 0)
        {
            over();
        }
        
    }

    private void start()
    {
        _start = true;
        _time = 1f;
    }

    private void over()
    {
        _ColorRectMaterial.SetShaderParam("dissolve_amount", 0f);
    }

}
