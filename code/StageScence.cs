using Godot;
using System;
using System.Collections.Generic;

public class StageScence : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.


    [Export]
    public NodePath fightAreaPath;
    [Export]
    public NodePath fightAreaInPath;

    //限定区域
    private Area2D _fightArea;
    private CollisionShape2D _fightAreaIn;

    public Vector2 _fightAreaArea;
    public override void _Ready()
    {
        base._Ready();
        showSize = this.GetViewport().Size + new Vector2(1000, 1000);
        _fightArea = (Area2D)GetNode(fightAreaPath);
        _fightAreaIn = (CollisionShape2D)GetNode(fightAreaInPath);
        _fightArea.Connect("body_entered", this, "_on_Area2D_body_entered");
        _fightArea.Connect("body_exited", this, "_on_Area2D_body_exited");

        _fightArea.Connect("area_entered", this, "_on_Area2D_area_entered");
        _fightArea.Connect("area_exited", this, "_on_Area2D_area_exited");

        RectangleShape2D area = (RectangleShape2D)_fightAreaIn.Shape;
        _fightAreaArea = area.Extents;
       // GD.Print("_fightAreaArea" + _fightAreaArea);
    }



    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);

    }

    private  void _on_Area2D_body_entered(Godot.Object area)
    {
        if (area.HasMethod("enterFightArea"))
        {
            area.Call("enterFightArea");            
        }
    }
    private void _on_Area2D_body_exited(Godot.Object area)
    {
        if (area.HasMethod("exitedFightArea"))
        {
            area.Call("exitedFightArea");
        }
    }
    private void _on_Area2D_area_entered(Area2D area)
    {
        if (area.HasMethod("enterFightArea"))
        {
            area.Call("enterFightArea");
        }
    }
    private void _on_Area2D_area_exited(Area2D area)
    {
        if (area.HasMethod("exitedFightArea"))
        {
            area.Call("exitedFightArea");
        }
    }


    #region 动态地图读取 暂时不启用

    private Vector2 showSize;
    public void loadSrouce(string srcPath)
    {
        GD.Print("loadSrouce " + srcPath);

        string filename = GlobalConstant.getFileName(srcPath,1);


        commonData.LoadData(filename, this);
    }

    private List<commonData> showDatas = new List<commonData>();

    public void updateMap(Vector2 playerPositon)
    {
        //GD.Print("playerPositon " + playerPositon);

        playerPositon -= showSize / 2;
        //commonData.commonMapDatas
        if(showDatas.Count > 0)
        {
            for (int i =0;i< showDatas.Count;i++ )
            {
                if (GlobalConstant.isOutArea(showDatas[i].prop_Position, showDatas[i].prop_Size, playerPositon, showSize))
                {                   
                    if(IsInstanceValid(showDatas[i].objectRe))
                        showDatas[i].objectRe.QueueFree();
                    showDatas.Remove(showDatas[i]);
                }

            }
        }
        if(commonData.commonMapDatas.Count > 0)
        {
            for (int i = 0; i < commonData.commonMapDatas.Count; i++)
            {
                if (!GlobalConstant.isOutArea(commonData.commonMapDatas[i].prop_Position, commonData.commonMapDatas[i].prop_Size, playerPositon, showSize))
                {
                    if (!showDatas.Contains(commonData.commonMapDatas[i]))
                    {
                        showDatas.Add(commonData.commonMapDatas[i]);
                        commonData.LoadData(commonData.commonMapDatas[i], this);
                    }

                }

            }
        }

        
    }

    #endregion








}
