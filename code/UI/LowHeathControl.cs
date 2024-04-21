using Godot;
using System;

public class LowHeathControl : Control
{

    [Export]
    public NodePath leftColorRectPath;
    [Export]
    public NodePath rightColorRectPath;
    [Export]
    public NodePath upColorRectPath;
    [Export]
    public NodePath downColorRectPath;


    private ColorRect _leftColorRect;
    private ColorRect _rightColorRect;
    private ColorRect _upColorRect;
    private ColorRect _downColorRect;

    public override void _Ready()
    {
        _leftColorRect = (ColorRect)GetNode(leftColorRectPath);
        _rightColorRect = (ColorRect)GetNode(rightColorRectPath);
        _upColorRect = (ColorRect)GetNode(upColorRectPath);
        _downColorRect = (ColorRect)GetNode(downColorRectPath);
    }

    private Color beginColor = new Color(0, 0, 0, 0);
    private Color lowHealthColor = new Color(220f / 255f, 99f / 255f, 144f / 255f, 184f / 255f);
    private Color VerylowHealthColor = new Color(146f / 255f, 19f / 255f, 43f / 255f, 169f / 255f);
    private bool isActive = false;


    public void init()
    {

        _leftColorRect.Color = beginColor;
        _rightColorRect.Color = beginColor;
        _upColorRect.Color = beginColor;
        _downColorRect.Color = beginColor;
    }

    public void lowHeath()
    {
        _leftColorRect.Color = lowHealthColor;
        _rightColorRect.Color = lowHealthColor;
        _upColorRect.Color = lowHealthColor;
        _downColorRect.Color = lowHealthColor;
        if(!isActive)
            isActive = true;

    }

    public void veryLowHealth()
    {
        _leftColorRect.Color = VerylowHealthColor;
        _rightColorRect.Color = VerylowHealthColor;
        _upColorRect.Color = VerylowHealthColor;
        _downColorRect.Color = VerylowHealthColor;
        if (!isActive)
            isActive = true;
    }

    private bool _isUp = false;

    public override void _PhysicsProcess(float delta)
    {
        if(isActive)
        {
            if(_isUp)
            {
                float newAlph = _leftColorRect.Modulate.a;
                newAlph = newAlph - 0.02f;
                newAlph = newAlph <= 0 ? 0 : newAlph;
                _leftColorRect.Modulate = new Color(1f, 1f, 1f, newAlph);
                _rightColorRect.Modulate = new Color(1f, 1f, 1f, newAlph);
                _upColorRect.Modulate = new Color(1f, 1f, 1f, newAlph);
                _downColorRect.Modulate = new Color(1f, 1f, 1f, newAlph);
                if (newAlph == 0)
                    _isUp = false;
            }else
            {
                float newAlph = _leftColorRect.Modulate.a;
                newAlph = newAlph + 0.02f;
                newAlph = newAlph >= 1f ? 1f : newAlph;
                _leftColorRect.Modulate = new Color(1f, 1f, 1f, newAlph);
                _rightColorRect.Modulate = new Color(1f, 1f, 1f, newAlph);
                _upColorRect.Modulate = new Color(1f, 1f, 1f, newAlph);
                _downColorRect.Modulate = new Color(1f, 1f, 1f, newAlph);
                if (newAlph == 1f)
                    _isUp = true;
            }

        }

    }

}
