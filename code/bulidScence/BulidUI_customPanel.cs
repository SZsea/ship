using Godot;
using System;

public class BulidUI_customPanel : Panel
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.

    [Export]
    public NodePath LabelText1Path;
    [Export]
    public NodePath LabelText2Path;

    private Label _labelText1;
    private Label _labelText2;

    public override void _Ready()
    {
        _labelText1 = GetNode<Label>(LabelText1Path);
        _labelText2 = GetNode<Label>(LabelText2Path);
    }



    public void updateData(string name, string value)
    {
        _labelText1.Text = name;
        _labelText2.Text = value;
    }

    public void resetRect(Vector2 newVect)
    {
        this.RectMinSize = newVect;
        _labelText1.RectSize = new Vector2(this.RectMinSize.x / 2, this.RectMinSize.y);
        _labelText2.RectSize = new Vector2(this.RectMinSize.x / 2, this.RectMinSize.y);
        _labelText1.RectPosition = new Vector2(0, 0);
        _labelText2.MarginLeft = -_labelText1.RectSize.x;

    }
}
