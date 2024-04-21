using Godot;
using System.Collections.Generic;
using System;
using static playerControl;

public class GlobalConstant : Node
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.

    public static bool BUllELTTESTMODE = true; //碰撞测试模式
    public static bool READYMODE = true; //ready()初始化打印

    public static float SHIPBASESPEED = 100f; //飞船基准速率 180*500
    public static int BEGINNINGGOLD = 200;//初始金币


    public static string mapData_loadPath = "mapdata/";
    public static string mapData_Extension = ".mapdata";
    public override void _Ready()
    {
        BUllELTTESTMODE = false;
        READYMODE = false;
    }
    public static void OPENALLTest()
    {
        BUllELTTESTMODE = true;
        READYMODE = true;
    }
    public static void CLOSELLTest()
    {
        BUllELTTESTMODE = false;
        READYMODE = false;
    }

    // 参数:
    //   itemPosition:
    //     物体正中心节点位置
    //   AreaPosition:
    //     区域左上角节点位置
    public static bool isOutArea(Vector2 itemPosition, Vector2 itemSize, Vector2 AreaPosition, Vector2 AreaSize)
    {
        //如果物体比区域大
        if(itemSize.x > AreaSize.x && itemSize.y > AreaSize.y)
        {
            return isOutArea(AreaPosition, AreaSize, itemPosition, itemSize);

        }
        else//如果物体比区域小
        {
            if ((itemPosition.x + itemSize.x / 2 > AreaPosition.x + AreaSize.x) ||
            (itemPosition.x - itemSize.x / 2 < AreaPosition.x) ||
            (itemPosition.y + itemSize.y / 2 > AreaPosition.y + AreaSize.y) ||
            (itemPosition.y - itemSize.y / 2 < AreaPosition.y))
            {

                return true;
            }
            else
            {
                return false;
            }
        }
        
    }

    // 参数:
    //   itemPosition:
    //     物体正中心节点位置
    //   AreaPosition:
    //     区域左上角节点位置
    // 以物体中心为准
    public static bool isOutAreaOther(Vector2 itemPosition, Vector2 itemSize, Vector2 AreaPosition, Vector2 AreaSize)
    {
        //如果物体比区域大
        if (itemSize.x > AreaSize.x && itemSize.y > AreaSize.y)
        {
            return isOutArea(AreaPosition, AreaSize, itemPosition, itemSize);

        }
        else//如果物体比区域小
        {
            if ((itemPosition.x  > AreaPosition.x + AreaSize.x) ||
            (itemPosition.x  < AreaPosition.x) ||
            (itemPosition.y > AreaPosition.y + AreaSize.y) ||
            (itemPosition.y < AreaPosition.y))
            {

                return true;
            }
            else
            {
                return false;
            }
        }

    }








    //返回距离最近的一组单位
    public static Dictionary<baseBulidPart, float> calculateDistaceDic(baseBulidPart item, List<baseBulidPart> dic,float dis)
    {
        Dictionary<baseBulidPart, float> result = new Dictionary<baseBulidPart, float>();
        foreach(var a in dic)
        {
            var ndis = (item.GlobalPosition - a.GlobalPosition).Length();
            if(ndis <= dis && ndis != 0)
            {
                result.Add(a, ndis);
            }
        }
        return result;
    }

    //根据权重随机取值 weight 权值 data数据 number 返回的数目个数

    public static List<int> randomFromWeight(List<int> weight , List<int> data, int number)
    {
        List<int> newdata = new List<int>();
        List<int> weightCopy = new List<int>();
        weightCopy.AddRange(weight);
        for (int i =1;i < weightCopy.Count;i++)
        {
            weightCopy[i] = weightCopy[i] + weightCopy[i - 1];
        }
        for(int rg = 0; rg < number ; rg++)
        {
            int rangenumber = (int)GD.RandRange(0, weightCopy[weightCopy.Count - 1]);
            for (int i = 0; i < weightCopy.Count; i++)
            {
                if (rangenumber < weightCopy[i])
                {
                    newdata.Add(data[i]);
                    GD.Print(data[i] + " index " + i);
                    break;
                }
            }
        }

        return newdata;

    }

    public static string getFileName(string str,int number)
    {
        string fullpath = str;
        string[] sArray = fullpath.Split(new char[2] { '.', '/' });
        string filename = sArray[sArray.Length - number - 1];
        return filename;
    }

    //怪物出生位置
    public static Vector2 getRandBitrth(Vector2 birth,float miniDistance,float maxDistance)
    {
         Vector2 newbirth= new Vector2();
        float maxValue = maxDistance - miniDistance;
        float newDistance = GD.Randf() * maxValue + miniDistance;

        newbirth.x = GD.Randf() * 2 > 1 ? GD.Randf() * newDistance : -GD.Randf() * newDistance;

        newbirth.y = (float)(GD.Randf() * 2 > 1 ? Math.Sqrt(newDistance * newDistance - newbirth.x * newbirth.x) : -Math.Sqrt(newDistance * newDistance - newbirth.x * newbirth.x));
        newbirth = newbirth + birth;

        return newbirth;
    }





}
