using cfg;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class Progress_ScrollContainer : ScrollContainer
{
    [Export]
    public NodePath GridContainerPath;


    private GridContainer _mygridContainer;

    public ProgressScence fatherView;

    public override void _Ready()
    {
        _mygridContainer = GetNode<GridContainer>(GridContainerPath);
        MouseFilter = MouseFilterEnum.Ignore;
    }

    private List<CustomPanel> panelList = new List<CustomPanel>();

    public List<int> _unlockedPacked;
    public void initWithData(List<playerCoreCfg> allpacked,List<int> unlockedPacked)
    {
        //GD.Print("allpacked.Count" + allpacked.Count);
        //GD.Print("unlockedPacked.Count" + unlockedPacked.Count);
        unlockedPacked.Sort();
        for (int i = 0; i < allpacked.Count; i++)
        {
            if (allpacked[i].id % 10 != 0)
                continue;
            CustomPanel panel = new CustomPanel();
            panel.RectMinSize = new Vector2(100f, 100f);
            if (unlockedPacked.Contains(allpacked[i].id))
            {
                
                var paneldataLoad = GD.Load<PackedScene>(allpacked[i].path);
                var paneldata = (Node2D)paneldataLoad.Instance();
                paneldata.Position = panel.RectMinSize / 2;
                panel.AddChild(paneldata);
                panel.Connect("gui_input", this, "_on_PanelContainer_gui_input", new Godot.Collections.Array(unlockedPacked[unlockedPacked.BinarySearch(allpacked[i].id)]));
            }
            _mygridContainer.AddChild(panel);
            panelList.Add(panel);
            

        }
    }


    private void _on_PanelContainer_gui_input(InputEvent @event, int cfg)
    {

        if (@event is InputEventMouseButton mouseEvent && (ButtonList)mouseEvent.ButtonIndex == ButtonList.Left)
        {
            if (mouseEvent.Pressed)
            {
                //GD.Print(cfg);
                fatherView.updateinformation(cfg);
            }


        }
    }
}
