using Godot;
using System;

public class SZBaseNode2D : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.

    private string _mainScencePath = "/root/Main"; //主场景路径
    private string _dataPath = "/root/Main/data"; //数据节点路径
    //private string _changeScencePath = "/root/Main/data"; //数据节点路径
    //private string _viewRootPath = "/root/Main/ViewRoot"; //主场景的场景节点路径

    public override void _Ready()
    {
        base._Ready();
    }

    public MainScence mainRoot()
    {
        return (MainScence)GetNode(_mainScencePath); 
    }

    public GameData gameDate()
    {
        return (GameData)GetNode(_dataPath);
    }




}
