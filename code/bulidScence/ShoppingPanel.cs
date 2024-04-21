using Godot;
using System;
using static GameData;

public class ShoppingPanel : Panel
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.

    [Export]
    public NodePath buyButtonPath;
    [Export]
    public NodePath buildNodePath;
    [Export]
    public NodePath pricePath;
    [Export]
    public NodePath bgPath;


    private Button _buyButton;
    private baseBulidPart _bulidPart;
    private Control _buildNode;
    private Label _priceLabel;
    private ColorRect _bg;
    public override void _Ready()
    {
        _buyButton = (Button)GetNode(buyButtonPath);
        _buildNode = (Control)GetNode(buildNodePath);
        _priceLabel = (Label)GetNode(pricePath);
        _bg = (ColorRect)GetNode(bgPath);
        _buyButton.Connect("pressed", this,"_buySomeThing");
    }

    public ShipBulidScence parantsScence;
    private void _buySomeThing()
    {
        if(parantsScence.addGoods(_goodIndex))
        {
            _buyButton.Disabled = true;
        }
    }

    private int _goodIndex = 0;

    public void updateCommodity(int index, language languageNow)
    {
        if (languageNow == language.english)
            _buyButton.Text = "buy";
        if (languageNow == language.chinese)
            _buyButton.Text = "购买";

        _buyButton.Disabled = false;
        _goodIndex = index;
        if (_buildNode.GetChildCount() > 0)
        {
            foreach (Node2D a in _buildNode.GetChildren())
            {
                _bulidPart = null;
                a.QueueFree();
            }
        }
        foreach (var item in cfg.playerCoreCfg.GetAllList())
        {
            if(index == item.id)
            {
                if (item.quality == 1)
                    _bg.Color = Godot.Color.ColorN("whitesmoke");
                if (item.quality == 2)
                    _bg.Color = Godot.Color.ColorN("lightskyblue");
                if (item.quality == 3)
                    _bg.Color = Godot.Color.ColorN("purple");
                if (item.quality == 4)
                    _bg.Color = Godot.Color.ColorN("orange");
                var player1 = GD.Load<PackedScene>(item.path);
                _bulidPart = (baseBulidPart)player1.Instance();
                _bulidPart.Scale = new Godot.Vector2(1.3f,1.3f);
                _buildNode.AddChild(_bulidPart);
                _priceLabel.Text = item.price.ToString();
                
            }
        }
    }
}
