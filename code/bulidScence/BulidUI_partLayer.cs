using cfg;
using Godot;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

public class BulidUI_partLayer : ScrollContainer
{

    [Export]
    public NodePath GridContainerPath;


    private GridContainer _mygridContainer;





    public override void _Ready()
    {
        _mygridContainer = GetNode<GridContainer>(GridContainerPath);
        MouseFilter = MouseFilterEnum.Ignore;
        //this.RectMinSize = new Vector2(250, 250);
        //_data.Add(bulidShipBulidUI_health, 0);
        
    }

    private List<BulidUI_customPanel> _panelList = new List<BulidUI_customPanel>();
    private Dictionary<string, string> _data = new Dictionary<string, string>();
    public void updateWithData(baseBulidPart data)
    {
        _data.Clear();
        if (_mygridContainer.GetChildCount() > 0)
        {
            _panelList.Clear();
            foreach (BulidUI_customPanel child in _mygridContainer.GetChildren())
            {
                child.QueueFree();
            }

        }
        if (data == null)
            return;

        cfg.playerCoreCfg cfg_ship = data.shipData();

        foreach (var Cfg_item in cfg_ship.param)
        {
            if (!_data.ContainsKey(Cfg_item.Key))
                _data.Add(Cfg_item.Key, Cfg_item.Value);
        }


        foreach (var item in _data)
        {

            foreach (var cfg_A in cfg.shipBuildAttributesCfg.GetAllList())
            {
                if (item.Key.ToInt() == cfg_A.id && cfg_A.allShow)
                {
                    var panel = GD.Load<PackedScene>("res://scence/UI/BulidUI_customPanel.tscn");
                    var panel1 = (BulidUI_customPanel)panel.Instance();
                    //panel1.RectMinSize = new Vector2(250, 50);
                    
                    _mygridContainer.AddChild(panel1);
                    _panelList.Add(panel1);
                    panel1.resetRect(new Vector2(250, 50));
                    panel1.updateData(cfg_A.ch, item.Value);
                    
                    //GD.Print(cfg.ch + "   " + item.Value);
                }
            }
        }
    }



}
