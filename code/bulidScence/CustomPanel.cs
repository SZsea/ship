using Godot;
using System;
using System.Drawing;
using static GameData;

public class CustomPanel : PanelContainer
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    private bool _isSelected = false;

    public int sequenceId;
    public Node2D chidNode;
    public Button updateButton;
    public int gold;
    public ColorRect backPanel;


    public language _languageNow = language.chinese;
    public override void _Ready()
    {

    }

    public void update(int i)
    {
        //背景
        backPanel = new ColorRect();
        backPanel.RectSize = this.RectSize;

        if(i == 1)
            backPanel.Color = Godot.Color.ColorN("whitesmoke");
        if(i == 2)
            backPanel.Color = Godot.Color.ColorN("lightskyblue");
        if(i == 3)
            backPanel.Color = Godot.Color.ColorN("purple");
        if(i == 4)
            backPanel.Color = Godot.Color.ColorN("orange");

        backPanel.MouseFilter = MouseFilterEnum.Ignore;
        AddChild(backPanel);
        //下方的按钮
        updateButton = new Button();
        AddChild(updateButton);
        updateButton.RectMinSize = new Vector2(100f, 40f);
        updateButton.RectPosition = new Vector2(50f, 50f);
        updateButton.SizeFlagsHorizontal = 8;
        updateButton.SizeFlagsVertical = 8;
        var dynamic_font = new DynamicFont();
        dynamic_font.FontData = GD.Load<DynamicFontData>("res://art/font/BoutiqueBitmap9x9_1.9.ttf");
        dynamic_font.Size = 12;
        updateButton.AddFontOverride("font", dynamic_font);
        //中英文互换
        updateButton.Text = "升级";
    }

    public void updateGold(int g)
    {
        gold = g;
        if(_languageNow == language.chinese)
            updateButton.Text = "升级 " + gold + "金币";
        if (_languageNow == language.english)
            updateButton.Text = "update " + gold + "gold";


    }


    public void updatePriceButton()
    {
        //中英文互换
        updateButton.Disabled = true;
        if (_languageNow == language.chinese)
            updateButton.Text = "已满级";
        if (_languageNow == language.english)
            updateButton.Text = "FullLevel";
    }

    public bool isSelected
    {
        get
        {
            return _isSelected;
        }
        set
        {
            _isSelected = value;
            if(_isSelected)
            {
                Modulate = Godot.Color.ColorN("gray");
            }else
            {
                Modulate = Godot.Color.ColorN("white");
            }
        }
    }
}
