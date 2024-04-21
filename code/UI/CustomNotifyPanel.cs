using Godot;
using System;

public class CustomNotifyPanel : Control
{

    [Export]
    public NodePath LabelPath;

    [Export]
    public NodePath TimerPath;

    private Label _label;
    private Timer _time;
    public override void _Ready()
    {
        _label= GetNode<Label>(LabelPath);
        _time = GetNode<Timer>(TimerPath);
        _time.Connect("timeout", this, "_on_Timer_timeout");
        this.Modulate = new Color(1f, 1f, 1f, 0);
    }

    public void initWithWord(string a)
    {
        this.Modulate = new Color(1f, 1f, 1f, 1f);
        isVisible = false;
        _label.Text = a;
        _time.Start(1.0f);
    }

    private bool isVisible = false;
    private float isVisibleTime = 0.1f;
    private void _on_Timer_timeout()
    {
        isVisible = true;
    }

    public override void _PhysicsProcess(float delta)
    {


        if (isVisible|| this.Modulate.a != 0)
        {
            isVisibleTime = isVisibleTime - delta;
            if (isVisibleTime < 0)
            {
                isVisibleTime = 0.1f;
                float a = this.Modulate.a;
                a = a - 0.1f;
                this.Modulate = new Color(1f, 1f, 1f, a);

            }
        }        
    }

}
