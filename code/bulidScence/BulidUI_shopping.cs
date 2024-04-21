using Godot;
using System;
using System.Collections.Generic;
using static GameData;
using static Godot.HTTPRequest;

public class BulidUI_shopping : CanvasLayer
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.

    [Export]
    public NodePath closeButtonPath;
    [Export]
    public NodePath refreshButtonPath;
    [Export]
    public NodePath panelPart1Path;
    [Export]
    public NodePath panelPart2Path;
    [Export]
    public NodePath panelPart3Path;
    [Export]
    public NodePath panelPart4Path;
    [Export]
    public NodePath pirceNotePath;

    private Button _closeButton;
    private Button _refreshButton;
    private ShoppingPanel _panelPart1;
    private ShoppingPanel _panelPart2;
    private ShoppingPanel _panelPart3;
    private ShoppingPanel _panelPart4;
    private Label _priceNote;

    private int _levelIndex = 0;
    public ShipBulidScence _shipBulidScence;
    private bool isFirst = true;
    public int refreshTime = 0;
    public int levelIndex
    {
        set
        {
            
            _levelIndex = value;
            _refreshShop(_levelIndex);
            _refreshButton.Connect("pressed", this, "_refreshShop", new Godot.Collections.Array(levelIndex));
            isFirst = false;
        }
        get
        {
            
            return _levelIndex;
        }
    }

    public override void _Ready()
    {
        _closeButton = (Button)GetNode(closeButtonPath);
        _refreshButton = (Button)GetNode(refreshButtonPath);
        _panelPart1 = (ShoppingPanel)GetNode(panelPart1Path);
        _panelPart2 = (ShoppingPanel)GetNode(panelPart2Path);
        _panelPart3 = (ShoppingPanel)GetNode(panelPart3Path);
        _panelPart4 = (ShoppingPanel)GetNode(panelPart4Path);
        _panelPart1.parantsScence = (ShipBulidScence)this.GetParent();
        _panelPart2.parantsScence = (ShipBulidScence)this.GetParent();
        _panelPart3.parantsScence = (ShipBulidScence)this.GetParent();
        _panelPart4.parantsScence = (ShipBulidScence)this.GetParent();
        _closeButton.Connect("pressed", this, "_close");
        _priceNote = (Label)GetNode(pirceNotePath);

        
        //_refreshShop(levelIndex);
    }

    private language _languageNow = language.chinese;
    public language languageNow
    {
        set
        {
            if (value == language.english)
            {
                _closeButton.Text = "close";
                _refreshButton.Text = "refresh";
            }
                
            if (value == language.chinese)
            {
                _closeButton.Text = "关闭";
                _refreshButton.Text = "刷新";
            }
            _languageNow = value;
        }
        get
        {
            return _languageNow;
        }
    }

    public void changePirceGold(int gold)
    {
        if (_languageNow == GameData.language.english)
            _priceNote.Text = gold + " gold";
        if (_languageNow == GameData.language.chinese)
            _priceNote.Text = gold + " 金币";
    }



    public void open()
    {
        this.Show();

    }

    //商店更新
    private void _refreshShop(int index = 0)
    {
        GD.Print("index" + index);
        if (index == 0)
            return;
        if(IsInstanceValid(_shipBulidScence)&& isFirst == false)
        {
            if (!_shipBulidScence.refreshGood())
                return;
            refreshTime++;
        }

        foreach (var item in cfg.stageLevelInfoCfg.GetAllList())
        {
            if(item.id == index)
            {
                List<int> Weights = new List<int>();
                List<int> value = new List<int>();
                foreach (var reward in item.rewardNormal)
                {
                    Weights.Add(reward.Value);
                    value.Add(reward.Key);
                }
                if(item.rewardSpecial.Count > 0)
                {
                    foreach (var reward in item.rewardSpecial)
                    {
                        Weights.Add(reward.Value);
                        value.Add(reward.Key);
                    }
                }
                List<int> reslut = GlobalConstant.randomFromWeight(Weights, value, 4);
                
                _panelPart1.updateCommodity(reslut[0], languageNow);
                _panelPart2.updateCommodity(reslut[1], languageNow);
                _panelPart3.updateCommodity(reslut[2], languageNow);
                _panelPart4.updateCommodity(reslut[3], languageNow);

            }
        }
    }


    private void _close()
    {
        this.Hide();
    }
}
