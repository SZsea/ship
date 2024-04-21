using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;

public class bulidScenceScroll : ScrollContainer
{




    [Export]
    public NodePath GridContainerPath;


    private GridContainer _mygridContainer;


    public ShipBulidScence fatherNode;
    public GameData.language language;

    public override void _Ready()
    {
        _mygridContainer = GetNode<GridContainer>(GridContainerPath);
        MouseFilter = MouseFilterEnum.Ignore;
        
    }

    private List<CustomPanel> panelList = new List<CustomPanel>();
    public void initWithData(List<string> data, List<cfg.playerCoreCfg.shipPartDataType> dataType, List<int> sequence, List<int> bulidId, List<int> updateId,List<int> gold,List<int> qualitys)
    {
        if(panelList.Count > 0)
        {
            panelList.Clear();
            if (_mygridContainer.GetChildCount() > 0)
            {
                foreach (Node a in _mygridContainer.GetChildren())
                {
                    a.QueueFree();
                }
            }
        }

        for(int i = 0;i< data.Count;i ++)
        {

            CustomPanel panel = new CustomPanel();
            if (i == 0)
            {
                _firstPanel = panel;
            }
            
            panel.update(qualitys[i]);
            panel.sequenceId = sequence[i];
            panel.RectMinSize = new Vector2(100f, 140f);
            //panel.RectSize = new Vector2(100f, 140f);
            var paneldataLoad = GD.Load<PackedScene>(data[i]);
            var paneldata = (Node2D)paneldataLoad.Instance();
            paneldata.Position = new Vector2(57f, 50f);
            panel.AddChild(paneldata);
            panel.chidNode = paneldata;
            _mygridContainer.AddChild(panel);
            panel._languageNow = language;
            panel.updateGold(gold[i]);
            if (updateId[i] == 0)
            {
                panel.updatePriceButton();
            }
            panelList.Add(panel);
            panel.Connect("gui_input", this, "_on_PanelContainer_gui_input",new Godot.Collections.Array(panel, data[i], dataType[i], bulidId[i]));

            panel.updateButton.Connect("pressed", this, "_on_Button_pressed", new Godot.Collections.Array(panel, bulidId[i]));
        }
    }
    public CustomPanel _firstPanel;

    private bool dragging = false;
    private Node2D paneldata;//可移动的图标

    private void _on_Button_pressed(CustomPanel panel,int bulidId)
    {
        if (!fatherNode.updateitem(panel.gold))
            return;
        foreach (var item in cfg.playerCoreCfg.GetAllList())
        {
            if (bulidId == item.id)
            {
                //GD.Print("zhaodao" + item.id + "  " + bulidId);
                if(item.updateId != 0)
                {
                    bulidId = item.updateId;
                    ///GD.Print("item.updateId" + bulidId);
                    foreach (var itemCfg in cfg.playerCoreCfg.GetAllList())
                    {
                        if (bulidId == itemCfg.id)
                        {
                            var paneldataLoad = GD.Load<PackedScene>(itemCfg.path);
                            var paneldataNew = (Node2D)paneldataLoad.Instance();
                            paneldataNew.Position = new Vector2(50f, 50f);                          
                            panel.RemoveChild(panel.chidNode);
                            panel.AddChild(paneldataNew);
                            panel.chidNode = null;
                            panel.chidNode = paneldataNew;
                            fatherNode.updateDate(panel.sequenceId, bulidId) ;
                        }
                    }
                    break;
                }
            }
        }

        GD.Print(bulidId);
        
    }


    //删除
    public void deleteData(int id)
    {
        for(int i =0;i< panelList.Count;i++)
        {
            if (panelList[i].sequenceId == id)
            {
                _mygridContainer.RemoveChild(panelList[i]);
                
                panelList[i].QueueFree();
                panelList.Remove(panelList[i]);
            }
        }
    }


    public void updateData(List<baseBulidPart> playerdata)
    {
        foreach (var item in panelList)
        {
            foreach (var itemS in playerdata)
            {
                if(itemS.onlyId == item.sequenceId)
                {
                    item.isSelected = true;
                    itemS.ConnectPanel = item;
                }
            }
        }
    }
    public void updateData(baseBulidPart playerdata)
    {
        foreach (var item in panelList)
        {
            if (playerdata.onlyId == item.sequenceId)
            {
                item.isSelected = true;
                playerdata.ConnectPanel = item;
            }
        }
    }





    private void _on_PanelContainer_gui_input(InputEvent @event, CustomPanel panel, string path, cfg.playerCoreCfg.shipPartDataType pathType,int buildId)
    {
        
        if (@event is InputEventMouseButton mouseEvent && (ButtonList)mouseEvent.ButtonIndex == ButtonList.Left && !panel.isSelected)
        {
            if (!dragging && mouseEvent.Pressed)
            {
                dragging = true;
                var paneldataLoad = GD.Load<PackedScene>(path);
                paneldata = (Node2D)paneldataLoad.Instance();
                GetParent().AddChild(paneldata);
                paneldata.GlobalPosition = GetGlobalMousePosition();
                //GD.Print("dragging");
                //panel.isSelected = true;

            }
            // Stop dragging if the button is released.
            if (dragging && !mouseEvent.Pressed)
            {
               // GD.Print("released");
                dragging = false;
                paneldata.QueueFree();
                GetParent().CallDeferred("newItem", path, panel, pathType, buildId);
            }

        }
        else
        {
            if (@event is InputEventMouseMotion motionEvent && dragging && !panel.isSelected)
            {
                
                paneldata.GlobalPosition = GetGlobalMousePosition();
                //GD.Print(paneldata.GlobalPosition);
            }
        }
    }
}
