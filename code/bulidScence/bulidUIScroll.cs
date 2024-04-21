using cfg;
using Godot;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

public class bulidUIScroll : ScrollContainer
{

    [Export]
    public NodePath GridContainerPath;


    private GridContainer _mygridContainer;





    public override void _Ready()
    {
        _mygridContainer = GetNode<GridContainer>(GridContainerPath);
        MouseFilter = MouseFilterEnum.Ignore;
        //_data.Add(bulidShipBulidUI_health, 0);
    }

    private List<BulidUI_customPanel> _panelList = new List<BulidUI_customPanel>();
    private Dictionary<string, string> _data = new Dictionary<string, string>();

    private const string PARTPOWER = "10000001";
    private const string COREPOWER = "10000002";

    public void updateWithData(baseBulidPart data, GameData.language thislanguage)
    {


        _data.Clear();
        if(_mygridContainer.GetChildCount() > 0)
        {
            _panelList.Clear();
            foreach (BulidUI_customPanel child in _mygridContainer.GetChildren())
            {
                child.QueueFree();
            }
            
        }
        if (data == null)
            return;

        List<baseBulidPart> dataList = data.setList();
        _data.Add(PARTPOWER, (-data.shipData().partPower).ToString());
        _data.Add(COREPOWER, data.shipData().corePower.ToString());
        foreach (var item in dataList)
        {
            cfg.playerCoreCfg cfg = item.shipData();

            foreach (var Cfg_item in cfg.param)
            {
                if(_data.ContainsKey(Cfg_item.Key))
                {
                    _data[Cfg_item.Key] = (_data[Cfg_item.Key].ToFloat() + Cfg_item.Value.ToFloat()).ToString();
                }else
                {
                    _data.Add(Cfg_item.Key, Cfg_item.Value);
                }
                
            }
        }


        foreach (var item in _data)
        {
            
            foreach (var cfg in cfg.shipBuildAttributesCfg.GetAllList())
            {
                if (item.Key.ToInt() == cfg.id && cfg.allShow)
                {                   
                    var panel = GD.Load<PackedScene>("res://scence/UI/BulidUI_customPanel.tscn");
                    var panel1 = (BulidUI_customPanel)panel.Instance();
                    panel1.RectMinSize = new Vector2( this.RectSize.x,100);
                    _mygridContainer.AddChild(panel1);
                    _panelList.Add(panel1);
                    if(thislanguage == GameData.language.english)
                    {
                        if (item.Key.ToInt() == 1)
                        {
                            panel1.updateData(cfg.eng, _data[COREPOWER] + "/" + _data[PARTPOWER]);
                            //GD.Print(cfg.ch + "   "  + _data[COREPOWER] + "   " + _data[PARTPOWER]);
                        }
                        else if (item.Key.ToInt() == 4)
                        {
                            panel1.updateData(cfg.eng, (item.Value.ToFloat() + GlobalConstant.SHIPBASESPEED).ToString());
                        }
                        else
                        {
                            panel1.updateData(cfg.eng, item.Value);
                        }
                    }
                    else
                    {
                        if (item.Key.ToInt() == 1)
                        {
                            panel1.updateData(cfg.ch, _data[COREPOWER] + "/" + _data[PARTPOWER]);
                            //GD.Print(cfg.ch + "   "  + _data[COREPOWER] + "   " + _data[PARTPOWER]);
                        }
                        else if (item.Key.ToInt() == 4)
                        {
                            panel1.updateData(cfg.ch, (item.Value.ToFloat() + GlobalConstant.SHIPBASESPEED).ToString());
                        }
                        else
                        {
                            panel1.updateData(cfg.ch, item.Value);
                        }
                    }

                    
                   
                }
            }
        }
    }

    public void updateWithData(cfg.playerCoreCfg data, GameData.language thislanguage)
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

        _data.Add(PARTPOWER, (-data.partPower).ToString());
        _data.Add(COREPOWER, data.corePower.ToString());

        foreach (var Cfg_item in data.param)
        {
            if (_data.ContainsKey(Cfg_item.Key))
            {
                _data[Cfg_item.Key] = (_data[Cfg_item.Key].ToFloat() + Cfg_item.Value.ToFloat()).ToString();
            }
            else
            {
                _data.Add(Cfg_item.Key, Cfg_item.Value);
            }

        }

        foreach (var item in _data)
        {

            foreach (var cfg in cfg.shipBuildAttributesCfg.GetAllList())
            {
                if (item.Key.ToInt() == cfg.id && cfg.allShow)
                {
                    var panel = GD.Load<PackedScene>("res://scence/UI/BulidUI_customPanel.tscn");
                    var panel1 = (BulidUI_customPanel)panel.Instance();
                    panel1.RectMinSize = new Vector2(this.RectSize.x, 100);
                    _mygridContainer.AddChild(panel1);
                    _panelList.Add(panel1);

                    if (thislanguage == GameData.language.english)
                    {
                        if (item.Key.ToInt() == 1)
                        {
                            panel1.updateData(cfg.eng, _data[COREPOWER] + "/" + _data[PARTPOWER]);
                            //GD.Print(cfg.ch + "   "  + _data[COREPOWER] + "   " + _data[PARTPOWER]);
                        }
                        else if (item.Key.ToInt() == 4)
                        {
                            panel1.updateData(cfg.eng, (item.Value.ToFloat() + GlobalConstant.SHIPBASESPEED).ToString());
                        }
                        else
                        {
                            panel1.updateData(cfg.eng, item.Value);
                        }
                    }else
                    {
                        if (item.Key.ToInt() == 1)
                        {
                            panel1.updateData(cfg.ch, _data[COREPOWER] + "/" + _data[PARTPOWER]);
                            //GD.Print(cfg.ch + "   "  + _data[COREPOWER] + "   " + _data[PARTPOWER]);
                        }
                        else if (item.Key.ToInt() == 4)
                        {
                            panel1.updateData(cfg.ch, (item.Value.ToFloat() + GlobalConstant.SHIPBASESPEED).ToString());
                        }
                        else
                        {
                            panel1.updateData(cfg.ch, item.Value);
                        }
                    }



                }
            }
        }
    }

}
