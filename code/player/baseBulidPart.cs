using Godot;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Reflection.Emit;
using System.Security.Policy;

public class baseBulidPart : KinematicBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.


    protected float _damagedHealthPer = 0.7f;
    protected float _slightDamagedHealthPer = 0.5f;
    protected float _veryDamagedHealthPer = 0.3f;




    public enum InterfacePos
    {
        InterfacePos_1,
        InterfacePos_2,
        InterfacePos_3,
        InterfacePos_4,
        InterfacePos_5,
        InterfacePos_6,

    }



    [Export]
    public NodePath InterfacePosition1Path;
    [Export]
    public NodePath InterfacePosition2Path;
    [Export]
    public NodePath InterfacePosition3Path;
    [Export]
    public NodePath InterfacePosition4Path;
    [Export]
    public NodePath InterfacePosition5Path;
    [Export]
    public NodePath InterfacePosition6Path;

    [Export]
    public NodePath TestInfoPath;


    //子节点物体
    protected baseBulidPart _interfacePositionBuild1;
    protected baseBulidPart _interfacePositionBuild2;
    protected baseBulidPart _interfacePositionBuild3;
    protected baseBulidPart _interfacePositionBuild4;
    protected baseBulidPart _interfacePositionBuild5;
    protected baseBulidPart _interfacePositionBuild6;
    //父节点物体
    public baseBulidPart _interfacePositionFather1;
    public baseBulidPart _interfacePositionFather2;
    public baseBulidPart _interfacePositionFather3;
    public baseBulidPart _interfacePositionFather4;
    public baseBulidPart _interfacePositionFather5;
    public baseBulidPart _interfacePositionFather6;



    public Godot.Label TestInfo
    {
        get
        {
            return (Godot.Label)GetNode(TestInfoPath);
        }
    }




    public Node2D InterfacePosition1
    {
        get
        {
            if (InterfacePosition1Path != null)
                return (Node2D)GetNode(InterfacePosition1Path);
            else
                return null;
        }
    }
    public Node2D InterfacePosition2
    {
        get
        {
            if (InterfacePosition2Path != null)
                return (Node2D)GetNode(InterfacePosition2Path);
            else
                return null;
        }
    }
    public Node2D InterfacePosition3
    {
        get
        {
            if (InterfacePosition3Path != null)
                return (Node2D)GetNode(InterfacePosition3Path);
            else
                return null;
        }
    }
    public Node2D InterfacePosition4
    {
        get
        {
            if (InterfacePosition4Path != null)
                return (Node2D)GetNode(InterfacePosition4Path);
            else
                return null;
        }
    }
    public Node2D InterfacePosition5
    {
        get
        {
            if (InterfacePosition5Path != null)
                return (Node2D)GetNode(InterfacePosition5Path);
            else
                return null;
        }
    }
    public Node2D InterfacePosition6
    {
        get
        {
            if (InterfacePosition6Path != null)
                return (Node2D)GetNode(InterfacePosition6Path);
            else
                return null;
        }
    }




    private void setFatherInterface(InterfacePos pos, baseBulidPart father)
    {
        clearAllFatherBulid();
        // GD.Print(pos + "   " + father);
        //隐藏测试标签
        TestInfo.Visible = false;
        switch (pos)
        {
            case InterfacePos.InterfacePos_1:
                _interfacePositionFather1 = father;
                TestInfo.Text = "InterfacePos.InterfacePos_1";
                break;
            case InterfacePos.InterfacePos_2:
                _interfacePositionFather2 = father;
                TestInfo.Text = "InterfacePos.InterfacePos_2";
                break;
            case InterfacePos.InterfacePos_3:
                _interfacePositionFather3 = father;
                TestInfo.Text = "InterfacePos.InterfacePos_3";
                break;
            case InterfacePos.InterfacePos_4:
                _interfacePositionFather4 = father;
                TestInfo.Text = "InterfacePos.InterfacePos_4";
                break;
            case InterfacePos.InterfacePos_5:
                _interfacePositionFather5 = father;
                TestInfo.Text = "InterfacePos.InterfacePos_5";
                break;
            case InterfacePos.InterfacePos_6:
                _interfacePositionFather6 = father;
                TestInfo.Text = "InterfacePos.InterfacePos_6";
                break;

        }

    }

    public void clearAllFatherBulid()
    {
        _interfacePositionFather1 = null;
        _interfacePositionFather2 = null;
        _interfacePositionFather3 = null;
        _interfacePositionFather4 = null;
        _interfacePositionFather5 = null;
        _interfacePositionFather6 = null;
        TestInfo.Text = " null";
    }

    public bool enabelInterfacePosition1
    {
        get
        {
            if (IsInstanceValid(_interfacePositionBuild1) || IsInstanceValid(_interfacePositionFather1))
                return false;
            else
                return true;
        }
    }
    public bool enabelInterfacePosition2
    {
        get
        {
            if (IsInstanceValid(_interfacePositionBuild2) || IsInstanceValid(_interfacePositionFather2))
                return false;
            else
                return true;
        }
    }

    public bool enabelInterfacePosition3
    {
        get
        {
            if (IsInstanceValid(_interfacePositionBuild3) || IsInstanceValid(_interfacePositionFather3))
                return false;
            else
                return true;
        }
    }

    public bool enabelInterfacePosition4
    {
        get
        {
            if (IsInstanceValid(_interfacePositionBuild4) || IsInstanceValid(_interfacePositionFather4))
                return false;
            else
                return true;
        }
    }

    public bool enabelInterfacePosition5
    {
        get
        {
            if (IsInstanceValid(_interfacePositionBuild5) || IsInstanceValid(_interfacePositionFather5))
                return false;
            else
                return true;
        }
    }

    public bool enabelInterfacePosition6
    {
        get
        {
            if (IsInstanceValid(_interfacePositionBuild6) || IsInstanceValid(_interfacePositionFather6))
                return false;
            else
                return true;
        }
    }


    private CustomPanel _connectPanel; //连接模式下的面板

    public PlayerScence _playerScenceParents;//父类节点

    public int onlyId;//游戏中的唯一id

    public int buildId;//部件id 读表

    //飞船部位总数
    public int allNumber()
    {
        int i = 0;
        if (IsInstanceValid(_interfacePositionBuild1))
        {
            i++;
            i = i +(int)_interfacePositionBuild1.allNumber();
        }
        if (IsInstanceValid(_interfacePositionBuild2))
        {
            i++;
            i = i + (int)_interfacePositionBuild2.allNumber();
        }
        if (IsInstanceValid(_interfacePositionBuild3))
        {
            i++;
            i = i + (int)_interfacePositionBuild3.allNumber();
        }
        if (IsInstanceValid(_interfacePositionBuild4))
        {
            i++;
            i = i + (int)_interfacePositionBuild4.allNumber();
        }
        if (IsInstanceValid(_interfacePositionBuild5))
        {
            i++;
            i = i + (int)_interfacePositionBuild5.allNumber();
        }
        if (IsInstanceValid(_interfacePositionBuild6))
        {
            i++;
            i = i + (int)_interfacePositionBuild6.allNumber();
        }
        return i;

    }

    public CustomPanel ConnectPanel
    {
        get
        {
            return _connectPanel;
        }
        set
        {
            _connectPanel = value;
        }
    }

    public void setBodyParents(PlayerScence parents)
    {
        _playerScenceParents = parents;

        if (IsInstanceValid(_interfacePositionBuild1))
            _interfacePositionBuild1.setBodyParents(parents);
        if (IsInstanceValid(_interfacePositionBuild2))
            _interfacePositionBuild2.setBodyParents(parents);
        if (IsInstanceValid(_interfacePositionBuild3))
            _interfacePositionBuild3.setBodyParents(parents);
        if (IsInstanceValid(_interfacePositionBuild4))
            _interfacePositionBuild4.setBodyParents(parents);
        if (IsInstanceValid(_interfacePositionBuild5))
            _interfacePositionBuild5.setBodyParents(parents);
        if (IsInstanceValid(_interfacePositionBuild6))
            _interfacePositionBuild6.setBodyParents(parents);
    }

    public bool CheckEnabelInterfacePosition(InterfacePos Pos)
    {
        switch (Pos)
        {
            case InterfacePos.InterfacePos_1:
                return enabelInterfacePosition1;
            case InterfacePos.InterfacePos_2:
                return enabelInterfacePosition2;
            case InterfacePos.InterfacePos_3:
                return enabelInterfacePosition3;
            case InterfacePos.InterfacePos_4:
                return enabelInterfacePosition4;
            case InterfacePos.InterfacePos_5:
                return enabelInterfacePosition5;
            case InterfacePos.InterfacePos_6:
                return enabelInterfacePosition6;

            default:
                return false;
        }
    }


    public override void _Ready()
    {

    }
    public override void _EnterTree()
    {

    }

    public Vector2 size()
    {
        Vector2 size = Vector2.Zero;
        size.x = 64f;
        size.y = 64f;
        return size;
    }

    public Vector2 setPostion(Vector2 InterfacePosition, Vector2 size)
    {
        Vector2 position = new Vector2(0, 0);
        if (InterfacePosition.x > 0 && InterfacePosition.y == 0)
        {
            position = new Vector2(InterfacePosition.x + size.x / 2, InterfacePosition.y);
        }
        if (InterfacePosition.x < 0 && InterfacePosition.y == 0)
        {
            position = new Vector2(InterfacePosition.x - size.x / 2, InterfacePosition.y);
        }
        if (InterfacePosition.x == 0 && InterfacePosition.y > 0)
        {
            position = new Vector2(InterfacePosition.x, InterfacePosition.y + size.y / 2);
        }
        if (InterfacePosition.x == 0 && InterfacePosition.y < 0)
        {
            position = new Vector2(InterfacePosition.x, InterfacePosition.y - size.y / 2);
        }
        return position;
    }

    public void RemoveChild(InterfacePos pos)
    {
        if (CheckEnabelInterfacePosition(pos))
            return;
        GD.Print("RemoveChild       " + pos);
        switch (pos)
        {

            case InterfacePos.InterfacePos_1:
                {
                    _interfacePositionBuild1.clearAllFatherBulid();
                    RemoveChild(_interfacePositionBuild1);
                    _interfacePositionBuild1 = null;

                }
                break;

            case InterfacePos.InterfacePos_2:
                {
                    _interfacePositionBuild2.clearAllFatherBulid();
                    RemoveChild(_interfacePositionBuild2);
                    _interfacePositionBuild2 = null;
                }
                break;

            case InterfacePos.InterfacePos_3:
                {
                    _interfacePositionBuild3.clearAllFatherBulid();
                    RemoveChild(_interfacePositionBuild3);
                    _interfacePositionBuild3 = null;
                }
                break;

            case InterfacePos.InterfacePos_4:
                {
                    _interfacePositionBuild4.clearAllFatherBulid();
                    RemoveChild(_interfacePositionBuild4);
                    _interfacePositionBuild4 = null;
                }
                break;

            case InterfacePos.InterfacePos_5:
                {
                    _interfacePositionBuild5.clearAllFatherBulid();
                    RemoveChild(_interfacePositionBuild5);
                    _interfacePositionBuild5 = null;
                }
                break;

            case InterfacePos.InterfacePos_6:
                {
                    _interfacePositionBuild6.clearAllFatherBulid();
                    RemoveChild(_interfacePositionBuild6);
                    _interfacePositionBuild6 = null;
                }
                break;


        }
    }

    public void resetChildPostion(baseBulidPart child)
    {
        //  GD.Print("resetChildPostionB" + child);
        //  GD.Print("resetChildPostionB InterfacePositionBuild1" + InterfacePositionBuild1);
        //   GD.Print("resetChildPostionB InterfacePositionBuild2" + InterfacePositionBuild2);
        //   GD.Print("resetChildPostionB InterfacePositionBuild3" + InterfacePositionBuild3);
        //   GD.Print("resetChildPostionB InterfacePositionBuild4" + InterfacePositionBuild4);
        //   GD.Print("resetChildPostionB InterfacePositionBuild5" + InterfacePositionBuild5);
        //   GD.Print("resetChildPostionB InterfacePositionBuild6" + InterfacePositionBuild6);
        //GD.Print("resetChildPostion          " + (GlobalPosition - child.GlobalPosition).Length());
        float distance = 70f;//判断是否连接的距离
        if ((GlobalPosition - child.GlobalPosition).Length() > distance)
        {
            GD.Print("resetChildPostion  111111        ");
            if (child == _interfacePositionBuild1)
            {
                //GD.Print("RemoveChild" + InterfacePos.InterfacePos_1);
                RemoveChild(InterfacePos.InterfacePos_1);
            }
            if (child == _interfacePositionBuild2)
            {
                RemoveChild(InterfacePos.InterfacePos_2);
            }
            if (child == _interfacePositionBuild3)
            {
                RemoveChild(InterfacePos.InterfacePos_3);
            }
            if (child == _interfacePositionBuild4)
            {
                RemoveChild(InterfacePos.InterfacePos_4);
            }
            if (child == _interfacePositionBuild5)
            {
                RemoveChild(InterfacePos.InterfacePos_5);
            }
            if (child == _interfacePositionBuild6)
            {
                RemoveChild(InterfacePos.InterfacePos_6);
            }

        }
        else
        {           
            if(nearlyInterFace(child) != null)
            {
                InterfacePos Npos = nearlyInterFace(child.GlobalPosition);
                GD.Print("CheckEnabelInterfacePosition          " + Npos);
                if (child == _interfacePositionBuild1)
                {
                    RemoveChild(InterfacePos.InterfacePos_1);
                }
                if (child == _interfacePositionBuild2)
                {
                    RemoveChild(InterfacePos.InterfacePos_2);
                }
                if (child == _interfacePositionBuild3)
                {
                    RemoveChild(InterfacePos.InterfacePos_3);
                }
                if (child == _interfacePositionBuild4)
                {
                    RemoveChild(InterfacePos.InterfacePos_4);
                }
                if (child == _interfacePositionBuild5)
                {
                    RemoveChild(InterfacePos.InterfacePos_5);
                }
                if (child == _interfacePositionBuild6)
                {
                    RemoveChild(InterfacePos.InterfacePos_6);
                }

                if (CheckEnabelInterfacePosition(Npos))
                {
                     
                     AddChildRe(Npos, child);
                }
            }else
            {
                //InterfacePos Npos = nearlyInterFace(child.GlobalPosition);
                GD.Print("CheckEnabelInterfacePosition          111111111111" );
                if (child == _interfacePositionBuild1)
                {
                    RemoveChild(InterfacePos.InterfacePos_1);
                }
                if (child == _interfacePositionBuild2)
                {
                    RemoveChild(InterfacePos.InterfacePos_2);
                }
                if (child == _interfacePositionBuild3)
                {
                    RemoveChild(InterfacePos.InterfacePos_3);
                }
                if (child == _interfacePositionBuild4)
                {
                    RemoveChild(InterfacePos.InterfacePos_4);
                }
                if (child == _interfacePositionBuild5)
                {
                    RemoveChild(InterfacePos.InterfacePos_5);
                }
                if (child == _interfacePositionBuild6)
                {
                    RemoveChild(InterfacePos.InterfacePos_6);
                }

            }


        }

        // GD.Print("resetChildPostionE"+ child);
        // GD.Print("resetChildPostionE InterfacePositionBuild1" + InterfacePositionBuild1);
        // GD.Print("resetChildPostionE InterfacePositionBuild2" + InterfacePositionBuild2);
        // GD.Print("resetChildPostionE InterfacePositionBuild3" + InterfacePositionBuild3);
        // GD.Print("resetChildPostionE InterfacePositionBuild4" + InterfacePositionBuild4);
        // GD.Print("resetChildPostionE InterfacePositionBuild5" + InterfacePositionBuild5);
        // GD.Print("resetChildPostionE InterfacePositionBuild6" + InterfacePositionBuild6);

    }


    public void AddChildRe(InterfacePos pos, baseBulidPart child)
    {
        //GD.Print("111111111111111" + this);
        //GD.Print("111111111111111" +pos);
        if (IsInstanceValid(child.GetParent()))
        {
            child.GetParent().RemoveChild(child);
        }
        AddChild(child);
        if (IsInstanceValid(_playerScenceParents))
        {
            child.setBodyParents(_playerScenceParents);
        }
        switch (pos)
        {
            case InterfacePos.InterfacePos_1:
                {
                    if (InterfacePosition1 != null)
                    {
                        _interfacePositionBuild1 = child;
                        child.Position = setPostion(InterfacePosition1.Position, child.size());
                        child.setFatherInterface(child.nearlyInterFace(InterfacePosition1.GlobalPosition), this);
                        // GD.Print("AddChildRe InterfacePositionBuild1" + InterfacePositionBuild1);
                    }
                    else
                    {
                        GD.Print("InterfacePosition1 == null!!!");
                    }

                }
                break;
            case InterfacePos.InterfacePos_2:
                {
                    if (InterfacePosition2 != null)
                    {
                        _interfacePositionBuild2 = child;
                        child.Position = setPostion(InterfacePosition2.Position, child.size());
                        child.setFatherInterface(child.nearlyInterFace(InterfacePosition2.GlobalPosition), this);
                    }
                    else
                    {
                        GD.Print("InterfacePosition2 == null!!!");
                    }

                }
                break;
            case InterfacePos.InterfacePos_3:
                {
                    if (InterfacePosition3 != null)
                    {
                        _interfacePositionBuild3 = child;
                        child.Position = setPostion(InterfacePosition3.Position, child.size());
                        child.setFatherInterface(child.nearlyInterFace(InterfacePosition3.GlobalPosition), this);
                    }
                    else
                    {
                        GD.Print("InterfacePosition3 == null!!!");
                    }

                }
                break;
            case InterfacePos.InterfacePos_4:
                {
                    if (InterfacePosition4 != null)
                    {
                        _interfacePositionBuild4 = child;
                        child.Position = setPostion(InterfacePosition4.Position, child.size());
                        child.setFatherInterface(child.nearlyInterFace(InterfacePosition4.GlobalPosition), this);
                    }
                    else
                    {
                        GD.Print("InterfacePosition4 == null!!!");
                    }

                }
                break;
            case InterfacePos.InterfacePos_5:
                {
                    if (InterfacePosition5 != null)
                    {
                        _interfacePositionBuild5 = child;
                        child.Position = setPostion(InterfacePosition5.Position, child.size());
                        child.setFatherInterface(child.nearlyInterFace(InterfacePosition5.GlobalPosition), this);
                    }
                    else
                    {
                        GD.Print("InterfacePosition5 == null!!!");
                    }

                }
                break;
            case InterfacePos.InterfacePos_6:
                {
                    if (InterfacePosition6 != null)
                    {
                        _interfacePositionBuild6 = child;
                        child.Position = setPostion(InterfacePosition6.Position, child.size());
                        child.setFatherInterface(child.nearlyInterFace(InterfacePosition6.GlobalPosition), this);
                    }
                    else
                    {
                        GD.Print("InterfacePosition6 == null!!!");
                    }

                }
                break;
        }
        //GD.Print("AddChildRe" + child);
        
        // GD.Print("AddChildRe InterfacePositionBuild2" + InterfacePositionBuild2);
        // GD.Print("AddChildRe InterfacePositionBuild3" + InterfacePositionBuild3);
        // GD.Print("AddChildRe InterfacePositionBuild4" + InterfacePositionBuild4);
        // GD.Print("AddChildRe InterfacePositionBuild5" + InterfacePositionBuild5);
        // GD.Print("AddChildRe InterfacePositionBuild6" + InterfacePositionBuild6);
        
    }

    //离目标最近的节点
    public InterfacePos nearlyInterFace(Vector2 position)
    {
        Dictionary<InterfacePos, float> disDic = new Dictionary<InterfacePos, float>();
        if (InterfacePosition1 != null)
            disDic.Add(InterfacePos.InterfacePos_1, (InterfacePosition1.GlobalPosition - position).Length());
        if (InterfacePosition2 != null)
            disDic.Add(InterfacePos.InterfacePos_2, (InterfacePosition2.GlobalPosition - position).Length());
        if (InterfacePosition3 != null)
            disDic.Add(InterfacePos.InterfacePos_3, (InterfacePosition3.GlobalPosition - position).Length());
        if (InterfacePosition4 != null)
            disDic.Add(InterfacePos.InterfacePos_4, (InterfacePosition4.GlobalPosition - position).Length());
        if (InterfacePosition5 != null)
            disDic.Add(InterfacePos.InterfacePos_5, (InterfacePosition5.GlobalPosition - position).Length());
        if (InterfacePosition6 != null)
            disDic.Add(InterfacePos.InterfacePos_6, (InterfacePosition6.GlobalPosition - position).Length());

        InterfacePos nearlyPos = disDic.Aggregate((x, y) => x.Value < y.Value ? x : y).Key;

        return nearlyPos;

    }

    public Node2D nearlyInterFace(baseBulidPart item)
    {
        float miniDis = 32f;

        List<Node2D> myNode = new List<Node2D>();
        List<Node2D> otherNode = new List<Node2D>();

        if (InterfacePosition1 != null)
        {
            myNode.Add(InterfacePosition1);
        }
        if (InterfacePosition2 != null)
        {
            myNode.Add(InterfacePosition2);
        }
        if (InterfacePosition3 != null)
        {
            myNode.Add(InterfacePosition3);
        }
        if (InterfacePosition4 != null)
        {
            myNode.Add(InterfacePosition4);
        }
        if (InterfacePosition5 != null)
        {
            myNode.Add(InterfacePosition5);
        }
        if (InterfacePosition6 != null)
        {
            myNode.Add(InterfacePosition6);
        }
        if (item.InterfacePosition1 != null)
        {
            otherNode.Add(item.InterfacePosition1);
        }
        if (item.InterfacePosition2 != null)
        {
            otherNode.Add(item.InterfacePosition2);
        }
        if (item.InterfacePosition3 != null)
        {
            otherNode.Add(item.InterfacePosition3);
        }
        if (item.InterfacePosition4 != null)
        {
            otherNode.Add(item.InterfacePosition4);
        }
        if (item.InterfacePosition5 != null)
        {
            otherNode.Add(item.InterfacePosition5);
        }
        if (item.InterfacePosition6 != null)
        {
            otherNode.Add(item.InterfacePosition6);
        }

        foreach(Node2D a in myNode)
        {
            //
            foreach(Node2D b in otherNode)
            {

                if ((a.GlobalPosition - b.GlobalPosition).Length() < miniDis)
                {
                    GD.Print("a.Postion" + a.GlobalPosition);
                    GD.Print("b.Postion" + b.GlobalPosition);
                    GD.Print("Length" + (a.GlobalPosition - b.GlobalPosition).Length());
                    return b;
                }
            }
        }
        return null;
    }


    public bool checkIsCanConnect(baseBulidPart item)
    {
        Dictionary<InterfacePos, float> disDic = new Dictionary<InterfacePos, float>();
        if (InterfacePosition1 != null)
            disDic.Add(InterfacePos.InterfacePos_1, (InterfacePosition1.GlobalPosition - item.GlobalPosition).Length());
        if (InterfacePosition2 != null)
            disDic.Add(InterfacePos.InterfacePos_2, (InterfacePosition2.GlobalPosition - item.GlobalPosition).Length());
        if (InterfacePosition3 != null)
            disDic.Add(InterfacePos.InterfacePos_3, (InterfacePosition3.GlobalPosition - item.GlobalPosition).Length());
        if (InterfacePosition4 != null)
            disDic.Add(InterfacePos.InterfacePos_4, (InterfacePosition4.GlobalPosition - item.GlobalPosition).Length());
        if (InterfacePosition5 != null)
            disDic.Add(InterfacePos.InterfacePos_5, (InterfacePosition5.GlobalPosition - item.GlobalPosition).Length());
        if (InterfacePosition6 != null)
            disDic.Add(InterfacePos.InterfacePos_6, (InterfacePosition6.GlobalPosition - item.GlobalPosition).Length());

        float value = disDic.Aggregate((x, y) => x.Value < y.Value ? x : y).Value;

       // GD.Print("checkIsCanConnect" + value);
        if(value > 42f)
        {
            return false;
        }else
        {
            return true ;
        }
    }


    public bool checkIsConnect(baseBulidPart item)
    {
        if ((_interfacePositionBuild1 == item) ||
            (_interfacePositionBuild2 == item) ||
            (_interfacePositionBuild3 == item) ||
            (_interfacePositionBuild4 == item) ||
            (_interfacePositionBuild5 == item) ||
            (_interfacePositionBuild6 == item))
            return true;
        if ((item._interfacePositionBuild1 == this) ||
            (item._interfacePositionBuild2 == this) ||
            (item._interfacePositionBuild3 == this) ||
            (item._interfacePositionBuild4 == this) ||
            (item._interfacePositionBuild5 == this) ||
            (item._interfacePositionBuild6 == this))
            return true;
        return false;
    }

    public void CustomFree()
    {
        if (IsInstanceValid(_interfacePositionBuild1))
            _interfacePositionBuild1.CustomFree();
        if (IsInstanceValid(_interfacePositionBuild2))
            _interfacePositionBuild2.CustomFree();
        if (IsInstanceValid(_interfacePositionBuild3))
            _interfacePositionBuild3.CustomFree();
        if (IsInstanceValid(_interfacePositionBuild4))
            _interfacePositionBuild4.CustomFree();
        if (IsInstanceValid(_interfacePositionBuild5))
            _interfacePositionBuild5.CustomFree();
        if (IsInstanceValid(_interfacePositionBuild6))
            _interfacePositionBuild6.CustomFree();

        _connectPanel.isSelected = false;
        QueueFree();
    }
    public void removeFromDic(List<baseBulidPart> dic)
    {
        List<baseBulidPart> deletList = new List<baseBulidPart>(dic);

        for (int i = 0; i < deletList.Count; i++)
        {
            if (_interfacePositionBuild1 == deletList[i])
                deletList[i].removeFromDic(dic);
            if (_interfacePositionBuild2 == deletList[i])
                deletList[i].removeFromDic(dic);
            if (_interfacePositionBuild3 == deletList[i])
                deletList[i].removeFromDic(dic);
            if (_interfacePositionBuild4 == deletList[i])
                deletList[i].removeFromDic(dic);
            if (_interfacePositionBuild5 == deletList[i])
                deletList[i].removeFromDic(dic);
            if (_interfacePositionBuild6 == deletList[i])
                deletList[i].removeFromDic(dic);
            if (this == deletList[i])
                dic.Remove(deletList[i]);
        }

    }

    private void copyfather(baseBulidPart orgin, baseBulidPart copy)
    {
        //copy.clearAllFatherBulid();
        if (IsInstanceValid(orgin._interfacePositionFather1))
            copy.setFatherInterface(InterfacePos.InterfacePos_1, (baseBulidPart)copy.GetParent());
        if (IsInstanceValid(orgin._interfacePositionFather2))
            copy.setFatherInterface(InterfacePos.InterfacePos_2, (baseBulidPart)copy.GetParent());
        if (IsInstanceValid(orgin._interfacePositionFather3))
            copy.setFatherInterface(InterfacePos.InterfacePos_3, (baseBulidPart)copy.GetParent());
        if (IsInstanceValid(orgin._interfacePositionFather4))
            copy.setFatherInterface(InterfacePos.InterfacePos_4, (baseBulidPart)copy.GetParent());
        if (IsInstanceValid(orgin._interfacePositionFather5))
            copy.setFatherInterface(InterfacePos.InterfacePos_5, (baseBulidPart)copy.GetParent());
        if (IsInstanceValid(orgin._interfacePositionFather6))
            copy.setFatherInterface(InterfacePos.InterfacePos_6, (baseBulidPart)copy.GetParent());

    }
    //替换某个部件
    public void changePart(baseBulidPart orgin)
    {
        if (IsInstanceValid(this._interfacePositionFather1))
        {
            this._interfacePositionFather1.AddChildRe(this._interfacePositionFather1.nearlyInterFace(orgin.InterfacePosition1.GlobalPosition), orgin);
            //GD.Print("this._interfacePositionFather1");
        }
            
        if (IsInstanceValid(this._interfacePositionFather2))
        {
            this._interfacePositionFather2.AddChildRe(this._interfacePositionFather2.nearlyInterFace(orgin.InterfacePosition2.GlobalPosition), orgin);
            //GD.Print("this._interfacePositionFather2");
        }
            
        if (IsInstanceValid(this._interfacePositionFather3))
        {
            this._interfacePositionFather3.AddChildRe(this._interfacePositionFather3.nearlyInterFace(orgin.InterfacePosition3.GlobalPosition), orgin);
            //GD.Print("this._interfacePositionFather3");
        }
            
        if (IsInstanceValid(this._interfacePositionFather4))
        {
            this._interfacePositionFather4.AddChildRe(this._interfacePositionFather4.nearlyInterFace(orgin.InterfacePosition4.GlobalPosition), orgin);
            //GD.Print("this._interfacePositionFather4");
        }
            
        if (IsInstanceValid(this._interfacePositionFather5))
        {
            this._interfacePositionFather5.AddChildRe(this._interfacePositionFather5.nearlyInterFace(orgin.InterfacePosition5.GlobalPosition), orgin);
        }
            
        if (IsInstanceValid(this._interfacePositionFather6))
        {
            this._interfacePositionFather6.AddChildRe(this._interfacePositionFather6.nearlyInterFace(orgin.InterfacePosition6.GlobalPosition), orgin);
        }
            
        this._interfacePositionFather1 = null;
        this._interfacePositionFather2 = null;
        this._interfacePositionFather3 = null;
        this._interfacePositionFather4 = null;
        this._interfacePositionFather5 = null;
        this._interfacePositionFather6 = null;

        if (IsInstanceValid(this._interfacePositionBuild1))
            orgin.AddChildRe(InterfacePos.InterfacePos_1, this._interfacePositionBuild1);
        if (IsInstanceValid(this._interfacePositionBuild2))
            orgin.AddChildRe(InterfacePos.InterfacePos_2, this._interfacePositionBuild2);
        if (IsInstanceValid(this._interfacePositionBuild3))
            orgin.AddChildRe(InterfacePos.InterfacePos_3, this._interfacePositionBuild3);
        if (IsInstanceValid(this._interfacePositionBuild4))
            orgin.AddChildRe(InterfacePos.InterfacePos_4, this._interfacePositionBuild4);
        if (IsInstanceValid(this._interfacePositionBuild5))
            orgin.AddChildRe(InterfacePos.InterfacePos_5, this._interfacePositionBuild5);
        if (IsInstanceValid(this._interfacePositionBuild6))
            orgin.AddChildRe(InterfacePos.InterfacePos_6, this._interfacePositionBuild6);
        this._interfacePositionBuild1 = null;
        this._interfacePositionBuild2 = null;
        this._interfacePositionBuild3 = null;
        this._interfacePositionBuild4 = null;
        this._interfacePositionBuild5 = null;
        this._interfacePositionBuild6 = null;

        _playerScenceParents = null;
        _connectPanel = null;
        this.QueueFree() ;
    }

    public baseBulidPart copy()
    {
        var player1 = GD.Load<PackedScene>(this.Filename);
        //GD.Print("copy()" + this.Filename);
        var player1In = (baseBulidPart)player1.Instance();
        player1In.onlyId = this.onlyId;
        player1In.buildId = this.buildId;
        this.GetParent().AddChild(player1In);
        if (IsInstanceValid(_interfacePositionBuild1))
        {
            var InterfacePosition1In = _interfacePositionBuild1.copy();
            player1In.AddChildRe(InterfacePos.InterfacePos_1, InterfacePosition1In);
            copyfather(_interfacePositionBuild1, InterfacePosition1In);
        }
        if (IsInstanceValid(_interfacePositionBuild2))
        {
            var InterfacePosition2In = _interfacePositionBuild2.copy();
            player1In.AddChildRe(InterfacePos.InterfacePos_2, InterfacePosition2In);
            copyfather(_interfacePositionBuild2, InterfacePosition2In);
        }
        if (IsInstanceValid(_interfacePositionBuild3))
        {
            var InterfacePosition3In = _interfacePositionBuild3.copy();
            player1In.AddChildRe(InterfacePos.InterfacePos_3, InterfacePosition3In);
            copyfather(_interfacePositionBuild3, InterfacePosition3In);
        }
        if (IsInstanceValid(_interfacePositionBuild4))
        {
            var InterfacePosition4In = _interfacePositionBuild4.copy();
            player1In.AddChildRe(InterfacePos.InterfacePos_4, InterfacePosition4In);
            copyfather(_interfacePositionBuild4, InterfacePosition4In);
        }
        if (IsInstanceValid(_interfacePositionBuild5))
        {
            var InterfacePosition5In = _interfacePositionBuild5.copy();
            player1In.AddChildRe(InterfacePos.InterfacePos_5, InterfacePosition5In);
            copyfather(_interfacePositionBuild5, InterfacePosition5In);
        }
        if (IsInstanceValid(_interfacePositionBuild6))
        {
            var InterfacePosition6In = _interfacePositionBuild6.copy();
            player1In.AddChildRe(InterfacePos.InterfacePos_6, InterfacePosition6In);
            copyfather(_interfacePositionBuild6, InterfacePosition6In);
        }
        return player1In;
    }


    public List<baseBulidPart> setList()
    {
        List<baseBulidPart> newlist = new List<baseBulidPart>();
        newlist.Add(this);
        if (IsInstanceValid(_interfacePositionBuild1))
            newlist.AddRange(_interfacePositionBuild1.setList());
        if (IsInstanceValid(_interfacePositionBuild2))
            newlist.AddRange(_interfacePositionBuild2.setList());
        if (IsInstanceValid(_interfacePositionBuild3))
            newlist.AddRange(_interfacePositionBuild3.setList());
        if (IsInstanceValid(_interfacePositionBuild4))
            newlist.AddRange(_interfacePositionBuild4.setList());
        if (IsInstanceValid(_interfacePositionBuild5))
            newlist.AddRange(_interfacePositionBuild5.setList());
        if (IsInstanceValid(_interfacePositionBuild6))
            newlist.AddRange(_interfacePositionBuild6.setList());
        return newlist;

    }



    public virtual void playerReciveDamge(float value)
    {
        //GD.Print("setBodyParents" + this);
        _playerScenceParents.on_healthchange(value);
    }


    //离开了战斗区域
    public virtual void exitedFightArea()
    {
        GD.Print("exitedFightArea" + this);
    }

    //进入了战斗区域
    public virtual void enterFightArea()
    {
        GD.Print("enterFightArea" + this);
    }

    #region 文件保存

    private const string SaveData_ID = "id";
    private const string SaveData_bulidID = "bulidId";
    private const string SaveData_filename = "filename";
    private const string SaveData_interfacePositionBuild1 = "interfacePositionBuild1";
    private const string SaveData_interfacePositionBuild2 = "interfacePositionBuild2";
    private const string SaveData_interfacePositionBuild3 = "interfacePositionBuild3";
    private const string SaveData_interfacePositionBuild4 = "interfacePositionBuild4";
    private const string SaveData_interfacePositionBuild5 = "interfacePositionBuild5";
    private const string SaveData_interfacePositionBuild6 = "interfacePositionBuild6";
    private const string SaveData_interfacePositionFather1 = "interfacePositionFather1";
    private const string SaveData_interfacePositionFather2 = "interfacePositionFather2";
    private const string SaveData_interfacePositionFather3 = "interfacePositionFather3";
    private const string SaveData_interfacePositionFather4 = "interfacePositionFather4";
    private const string SaveData_interfacePositionFather5 = "interfacePositionFather5";
    private const string SaveData_interfacePositionFather6 = "interfacePositionFather6";


    //文件保存
    public Godot.Collections.Dictionary<string, object> Save()
    {
        Godot.Collections.Dictionary<string, object> saveData = new Godot.Collections.Dictionary<string, object>();
        saveData.Add(SaveData_ID, this.onlyId);
        saveData.Add(SaveData_bulidID, this.buildId);
        saveData.Add(SaveData_filename, this.Filename);
        if (IsInstanceValid(_interfacePositionBuild1))
            saveData.Add(SaveData_interfacePositionBuild1, _interfacePositionBuild1.Save());
        if (IsInstanceValid(_interfacePositionBuild2))
            saveData.Add(SaveData_interfacePositionBuild2, _interfacePositionBuild2.Save());
        if (IsInstanceValid(_interfacePositionBuild3))
            saveData.Add(SaveData_interfacePositionBuild3, _interfacePositionBuild3.Save());
        if (IsInstanceValid(_interfacePositionBuild4))
            saveData.Add(SaveData_interfacePositionBuild4, _interfacePositionBuild4.Save());
        if (IsInstanceValid(_interfacePositionBuild5))
            saveData.Add(SaveData_interfacePositionBuild5, _interfacePositionBuild5.Save());
        if (IsInstanceValid(_interfacePositionBuild6))
            saveData.Add(SaveData_interfacePositionBuild6, _interfacePositionBuild6.Save());
        if (IsInstanceValid(_interfacePositionFather1))
            saveData.Add(SaveData_interfacePositionFather1, 1);
        if (IsInstanceValid(_interfacePositionFather2))
            saveData.Add(SaveData_interfacePositionFather2, 1);
        if (IsInstanceValid(_interfacePositionFather3))
            saveData.Add(SaveData_interfacePositionFather3, 1);
        if (IsInstanceValid(_interfacePositionFather4))
            saveData.Add(SaveData_interfacePositionFather4, 1);
        if (IsInstanceValid(_interfacePositionFather5))
            saveData.Add(SaveData_interfacePositionFather5, 1);
        if (IsInstanceValid(_interfacePositionFather6))
            saveData.Add(SaveData_interfacePositionFather6, 1);

        return saveData;

    }


    //解析数据
    public baseBulidPart loadData(Godot.Collections.Dictionary<string, object> data)
    {


        var player1 = GD.Load<PackedScene>(data[SaveData_filename].ToString());
        //GD.Print("loadData()" + data[SaveData_filename].ToString());
        var player1In = (baseBulidPart)player1.Instance();
        player1In.onlyId = data[SaveData_ID].ToString().ToInt();
        player1In.buildId = data[SaveData_bulidID].ToString().ToInt();
        AddChild(player1In);
        foreach (KeyValuePair<string, object> entry in data)
        {
            string key = entry.Key.ToString();

            if (key == SaveData_interfacePositionBuild1)
                player1In.AddChildRe(InterfacePos.InterfacePos_1, player1In.loadData(new Godot.Collections.Dictionary<string, object>((Godot.Collections.Dictionary)entry.Value)));
            if (key == SaveData_interfacePositionBuild2)
                player1In.AddChildRe(InterfacePos.InterfacePos_2, player1In.loadData(new Godot.Collections.Dictionary<string, object>((Godot.Collections.Dictionary)entry.Value)));
            if (key == SaveData_interfacePositionBuild3)
                player1In.AddChildRe(InterfacePos.InterfacePos_3, player1In.loadData(new Godot.Collections.Dictionary<string, object>((Godot.Collections.Dictionary)entry.Value)));
            if (key == SaveData_interfacePositionBuild4)
                player1In.AddChildRe(InterfacePos.InterfacePos_4, player1In.loadData(new Godot.Collections.Dictionary<string, object>((Godot.Collections.Dictionary)entry.Value)));
            if (key == SaveData_interfacePositionBuild5)
                player1In.AddChildRe(InterfacePos.InterfacePos_5, player1In.loadData(new Godot.Collections.Dictionary<string, object>((Godot.Collections.Dictionary)entry.Value)));
            if (key == SaveData_interfacePositionBuild6)
                player1In.AddChildRe(InterfacePos.InterfacePos_6, player1In.loadData(new Godot.Collections.Dictionary<string, object>((Godot.Collections.Dictionary)entry.Value)));
            if (key == SaveData_interfacePositionFather1)
                player1In.setFatherInterface(InterfacePos.InterfacePos_1, (baseBulidPart)player1In.GetParent());
            if (key == SaveData_interfacePositionFather2)
                player1In.setFatherInterface(InterfacePos.InterfacePos_2, (baseBulidPart)player1In.GetParent());
            if (key == SaveData_interfacePositionFather3)
                player1In.setFatherInterface(InterfacePos.InterfacePos_3, (baseBulidPart)player1In.GetParent());
            if (key == SaveData_interfacePositionFather4)
                player1In.setFatherInterface(InterfacePos.InterfacePos_4, (baseBulidPart)player1In.GetParent());
            if (key == SaveData_interfacePositionFather5)
                player1In.setFatherInterface(InterfacePos.InterfacePos_5, (baseBulidPart)player1In.GetParent());
            if (key == SaveData_interfacePositionFather6)
                player1In.setFatherInterface(InterfacePos.InterfacePos_6, (baseBulidPart)player1In.GetParent());



        }
        //GD.Print("SaveData_interfacePositionFather()" + (baseBulidPart)player1In.GetParent());
        RemoveChild(player1In);
        this.GetParent().AddChild(player1In);
        return player1In;
    }

    #endregion

    protected cfg.playerCoreCfg _shipBulidData;
    protected cfg.bulletInfoCfg _bulletData;

    public cfg.playerCoreCfg shipData()
    {
        cfg.playerCoreCfg data = new cfg.playerCoreCfg();
        foreach (var item in cfg.playerCoreCfg.GetAllList())
        {
            if (item.id == this.buildId)
            {
                data.param = item.param;
                data.damage = item.damage;
                data.reloadTime = item.reloadTime;
                data.speed = item.speed;
                data.health = item.health;
                data.shiled = item.shiled;
                data.type = item.type;
                data.bulletId = item.bulletId;
                data.aimDistance = item.aimDistance;
                if (item.type == cfg.playerCoreCfg.shipPartDataType.derived)
                {
                    data.power = -item.power;
                    data.corePower += 0;
                    data.partPower += item.power;
                    
                } else
                {
                    data.power = item.power;
                    data.corePower += item.power;
                    data.partPower += 0;
                  
                }
            }
        }

        
        if (IsInstanceValid(_interfacePositionBuild1))
        {
            cfg.playerCoreCfg partdata = _interfacePositionBuild1.shipData();
            data.speed += partdata.speed;
            data.health += partdata.health;
            data.shiled += partdata.shiled;
            data.partPower += partdata.partPower;
            data.corePower += partdata.corePower;
        }
        if (IsInstanceValid(_interfacePositionBuild2))
        {
            cfg.playerCoreCfg partdata = _interfacePositionBuild2.shipData();
            data.speed += partdata.speed;
            data.health += partdata.health;
            data.shiled += partdata.shiled;
            data.partPower += partdata.partPower;
            data.corePower += partdata.corePower;

        }
        if (IsInstanceValid(_interfacePositionBuild3))
        {
            cfg.playerCoreCfg partdata = _interfacePositionBuild3.shipData();
            data.speed += partdata.speed;
            data.health += partdata.health;
            data.shiled += partdata.shiled;
            data.partPower += partdata.partPower;
            data.corePower += partdata.corePower;
        }
        if (IsInstanceValid(_interfacePositionBuild4))
        {
            cfg.playerCoreCfg partdata = _interfacePositionBuild4.shipData();
            data.speed += partdata.speed;
            data.health += partdata.health;
            data.shiled += partdata.shiled;
            data.partPower += partdata.partPower;
            data.corePower += partdata.corePower;
        }
        if (IsInstanceValid(_interfacePositionBuild5))
        {
            cfg.playerCoreCfg partdata = _interfacePositionBuild5.shipData();
            data.speed += partdata.speed;
            data.health += partdata.health;
            data.shiled += partdata.shiled;
            data.partPower += partdata.partPower;
            data.corePower += partdata.corePower;
        }
        if (IsInstanceValid(_interfacePositionBuild6))
        {
            cfg.playerCoreCfg partdata = _interfacePositionBuild6.shipData();
            data.speed += partdata.speed;
            data.health += partdata.health;
            data.shiled += partdata.shiled;
            data.partPower += partdata.partPower;
            data.corePower += partdata.corePower;
        }
        _shipBulidData = data;

        foreach (var item in cfg.bulletInfoCfg.GetAllList())
        {
            if (data.bulletId == item.id)
            {
                _bulletData = item;
            }
        }

       // GD.Print("data.type/data.power/data.partPower/data.corePower    " + data.type + " " + data.power + " " + data.partPower + " " + data.corePower);
        return data;
    }

    //检查虚假的链接通道1
    public void shipCheckAssembledAgain(List<baseBulidPart> itemDic)
    {
        foreach (var item in itemDic)
        {
            var newdic = GlobalConstant.calculateDistaceDic(item, itemDic, 100f);
            if (newdic.Count > 0)
            {
                baseBulidPart nearlyitem = newdic.Aggregate((x, y) => x.Value < y.Value ? x : y).Key;


                var newdicN = newdic.OrderBy(o => o.Value).ToDictionary(o => o.Key, p => p.Value);
                //newdicN.Remove(nearlyitem);

                foreach (var items in newdicN)
                {
                    if (items.Value < 65f)
                    {
                        //GD.Print("+__+_+_++_+" + items.Key);
                        baseBulidPart nearlykey = items.Key;
                        item.checkInterfaceChannelFake(nearlykey);

                    }
                }
            }
        }

    }
    //检查虚假的链接通道2
    public void checkInterfaceChannelFake(baseBulidPart baseItem)
    {
        List<Node2D> myInterface = new List<Node2D>();
        List<Node2D> toInterface = new List<Node2D>();
        if (InterfacePosition1 != null)
            myInterface.Add(InterfacePosition1);
        if (InterfacePosition2 != null)
            myInterface.Add(InterfacePosition2);
        if (InterfacePosition3 != null)
            myInterface.Add(InterfacePosition3);
        if (InterfacePosition4 != null)
            myInterface.Add(InterfacePosition4);
        if (InterfacePosition5 != null)
            myInterface.Add(InterfacePosition5);
        if (InterfacePosition6 != null)
            myInterface.Add(InterfacePosition6);

        if (baseItem.InterfacePosition1 != null)
            toInterface.Add(baseItem.InterfacePosition1);
        if (baseItem.InterfacePosition2 != null)
            toInterface.Add(baseItem.InterfacePosition2);
        if (baseItem.InterfacePosition3 != null)
            toInterface.Add(baseItem.InterfacePosition3);
        if (baseItem.InterfacePosition4 != null)
            toInterface.Add(baseItem.InterfacePosition4);
        if (baseItem.InterfacePosition5 != null)
            toInterface.Add(baseItem.InterfacePosition5);
        if (baseItem.InterfacePosition6 != null)
            toInterface.Add(baseItem.InterfacePosition6);


        foreach(var item in myInterface)
        {
            foreach(var item2 in toInterface)
            {
                if ((item.GlobalPosition - item2.GlobalPosition).Length() < 5f)
                {

                    if (item == InterfacePosition1)
                        interface1ChannelOn();
                    if (item == InterfacePosition2)
                        interface2ChannelOn();
                    if (item == InterfacePosition3)
                        interface3ChannelOn();
                    if (item == InterfacePosition4)
                        interface4ChannelOn();
                    if (item == InterfacePosition5)
                        interface5ChannelOn();
                    if (item == InterfacePosition6)
                        interface6ChannelOn();

                    if (item2 == baseItem.InterfacePosition1)
                        baseItem.interface1ChannelOn();
                    if (item2 == baseItem.InterfacePosition2)
                        baseItem.interface2ChannelOn();
                    if (item2 == baseItem.InterfacePosition3)
                        baseItem.interface3ChannelOn();
                    if (item2 == baseItem.InterfacePosition4)
                        baseItem.interface4ChannelOn();
                    if (item2 == baseItem.InterfacePosition5)
                        baseItem.interface5ChannelOn();
                    if (item2 == baseItem.InterfacePosition6)
                        baseItem.interface6ChannelOn();
                }
            }
        }



    }


    #region 动画虚方法

    public void checkinterfaceChannel()
    {
        if (IsInstanceValid(_interfacePositionBuild1) || IsInstanceValid(_interfacePositionFather1))
            interface1ChannelOn();
        else
            interface1ChannelOff();
        if (IsInstanceValid(_interfacePositionBuild2) || IsInstanceValid(_interfacePositionFather2))
            interface2ChannelOn();
        else
            interface2ChannelOff();
        if (IsInstanceValid(_interfacePositionBuild3) || IsInstanceValid(_interfacePositionFather3))
            interface3ChannelOn();
        else
            interface3ChannelOff();
        if (IsInstanceValid(_interfacePositionBuild4) || IsInstanceValid(_interfacePositionFather4))
            interface4ChannelOn();
        else
            interface4ChannelOff();
        if (IsInstanceValid(_interfacePositionBuild5) || IsInstanceValid(_interfacePositionFather5))
            interface5ChannelOn();
        else
            interface5ChannelOff();
        if (IsInstanceValid(_interfacePositionBuild6) || IsInstanceValid(_interfacePositionFather6))
            interface6ChannelOn();
        else
            interface6ChannelOff();
        if (IsInstanceValid(_interfacePositionBuild1))
            _interfacePositionBuild1.checkinterfaceChannel();
        if (IsInstanceValid(_interfacePositionBuild2))
            _interfacePositionBuild2.checkinterfaceChannel();
        if (IsInstanceValid(_interfacePositionBuild3))
            _interfacePositionBuild3.checkinterfaceChannel();
        if (IsInstanceValid(_interfacePositionBuild4))
            _interfacePositionBuild4.checkinterfaceChannel();
        if (IsInstanceValid(_interfacePositionBuild5))
            _interfacePositionBuild5.checkinterfaceChannel();
        if (IsInstanceValid(_interfacePositionBuild6))
            _interfacePositionBuild6.checkinterfaceChannel();


    }


     


    public virtual void interface1ChannelOn()
    {

    }
    public virtual void interface2ChannelOn()
    {

    }
    public virtual void interface3ChannelOn()
    {

    }
    public virtual void interface4ChannelOn()
    {

    }
    public virtual void interface5ChannelOn()
    {

    }
    public virtual void interface6ChannelOn()
    {

    }
    public virtual void interface1ChannelOff()
    {

    }
    public virtual void interface2ChannelOff()
    {

    }
    public virtual void interface3ChannelOff()
    {

    }
    public virtual void interface4ChannelOff()
    {

    }
    public virtual void interface5ChannelOff()
    {

    }
    public virtual void interface6ChannelOff()
    {

    }

    public virtual void engineON()
    {

    }

    public virtual void updateTradFromHealth(float per = 1f)
    {

    }


    #endregion

}
