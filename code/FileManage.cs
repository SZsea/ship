using cfg;
using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using static cfg.playerCoreCfg;

namespace cfg
{
    //飞船组件表
    public class playerCoreCfg 
    {
        public int id;
        public string name;
        public string path;
        public shipPartDataType type;
        public int bulletId;
        public Dictionary<string, string> param;
        public int updateId;
        public int updateGold;
        public int price;//购买价格
        public int salePrice;//售卖价格
        public bool unlockedBegin;//是否开始解锁
        public int aimDistance; //射程
        public int quality;//品质


        public enum shipPartDataType
        {
            core = 0,
            derived = 1
        }


        public int partPower = 0;//计算用
        public int corePower = 0;//计算用
        public int power = 0;
        public int damage = 0;
        public float reloadTime = 0;
        public int speed = 0;
        public int health = 0;
        public int shiled = 0;
        
        
        public void dealWithAttri()
        {
            foreach(var item in param)
            {
                if(item.Key == "1")
                    power = item.Value.ToInt();
                if (item.Key == "2")
                    health = item.Value.ToInt();
                if (item.Key == "3")
                    shiled = item.Value.ToInt();
                if (item.Key == "4")
                    speed = item.Value.ToInt();
                if (item.Key == "5")
                    damage = item.Value.ToInt();
                if (item.Key == "101")
                    reloadTime = item.Value.ToFloat();
            }
            
        }




        private static Dictionary<int, playerCoreCfg> all = null;
        private static List<playerCoreCfg> allList = null;

        public static Dictionary<int, playerCoreCfg> GetAll()
        {
            return all;
        }

        public static List<playerCoreCfg> GetAllList()
        {
            return allList;
        }

        public void setcfg(Dictionary<int, playerCoreCfg> dic, List<playerCoreCfg> List)
        {
            all = dic;
            allList = List;

        }
    }
    //关卡表
    public class stageLevelInfoCfg
    {
        public int id;
        public string name;
        public string path;
        public int nextLevelV;
        public int nextLevelF;
        public List<int> monsterList;//怪物id数组
        public int bossId;
        public int limitTime;
        public List<int> refreshTimeList;//怪物出现时间
        public List<int> monsterMaxList;//怪物最多数目
        public List<int> monsterOnceMiniList;//怪物单次刷新最小数目
        public List<int> monsterOnceMaxList;//怪物单次刷新最大数目
        public List<float> refreshIntervalList;//单次刷新中的刷新间隔
        public Dictionary<int,int> rewardNormal;//一般奖励
        public Dictionary<int, int> rewardSpecial;//特殊奖励
        public int rewardPrice;//奖励的固定金钱


        private static Dictionary<int, stageLevelInfoCfg> all = null;
        private static List<stageLevelInfoCfg> allList = null;

        public static Dictionary<int, stageLevelInfoCfg> GetAll()
        {
            return all;
        }

        public static List<stageLevelInfoCfg> GetAllList()
        {
            return allList;
        }

        public void setcfg(Dictionary<int, stageLevelInfoCfg> dic, List<stageLevelInfoCfg> List)
        {
            all = dic;
            allList = List;
        }
    }

    public class enemyInfoCfg
    {
        public int id;
        public string name;
        public string path;
        public int health;
        public int damge;
        public int speed;
        public bool isBoss;
        public int bulletId;
        public int onceShootMaxNumber;//单次射击最大子弹数目
        public float reload_oneShoot;//单次射击中的子弹间隔
        public int reload_time;//多次射击间隔
        public int rewardPrice;//击杀金钱
        public int aimDistance;//最远射击距离
        public bool isRandom;//随机生成



        private static Dictionary<int, enemyInfoCfg> all = null;
        private static List<enemyInfoCfg> allList = null;

        public static Dictionary<int, enemyInfoCfg> GetAll()
        {
            return all;
        }

        public static List<enemyInfoCfg> GetAllList()
        {
            return allList;
        }

        public void setcfg(Dictionary<int, enemyInfoCfg> dic, List<enemyInfoCfg> List)
        {
            all = dic;
            allList = List;
        }
    }


    public class bulletInfoCfg
    {
        public int id;
        public string name;
        public string path;
        public int mode;


        private static Dictionary<int, bulletInfoCfg> all = null;
        private static List<bulletInfoCfg> allList = null;

        public static Dictionary<int, bulletInfoCfg> GetAll()
        {
            return all;
        }

        public static List<bulletInfoCfg> GetAllList()
        {
            return allList;
        }

        public void setcfg(Dictionary<int, bulletInfoCfg> dic, List<bulletInfoCfg> List)
        {
            all = dic;
            allList = List;
        }
    }

    public class shipBuildAttributesCfg
    {
        public int id;
        public string ch;
        public string eng;
        public bool allShow;
        public bool allAddition;
        public bool partShow;


        private static Dictionary<int, shipBuildAttributesCfg> all = null;
        private static List<shipBuildAttributesCfg> allList = null;

        public static Dictionary<int, shipBuildAttributesCfg> GetAll()
        {
            return all;
        }

        public static List<shipBuildAttributesCfg> GetAllList()
        {
            return allList;
        }

        public void setcfg(Dictionary<int, shipBuildAttributesCfg> dic, List<shipBuildAttributesCfg> List)
        {
            all = dic;
            allList = List;
        }
    }


    public class rangEnemyCfg
    {
        public int id;
        public string path;
        public bool isBoss;


        private static Dictionary<int, rangEnemyCfg> all = null;
        private static List<rangEnemyCfg> allList = null;

        private static List<rangEnemyCfg> allBoss = null;  //全部都是boss
        private static List<rangEnemyCfg> allNormal = null; // 全部都是小怪
        public static Dictionary<int, rangEnemyCfg> GetAll()
        {
            return all;
        }

        public static List<rangEnemyCfg> GetAllList()
        {
            return allList;
        }

        public static List<rangEnemyCfg> GetAllBoss()
        {
            return allBoss;
        }

        public static List<rangEnemyCfg> GetAllNormal()
        {
            return allNormal;
        }

        public void setcfg(Dictionary<int, rangEnemyCfg> dic, List<rangEnemyCfg> List)
        {
            all = dic;
            allList = List;
        }

        public void setcfg(List<rangEnemyCfg> dic, List<rangEnemyCfg> List)
        {
            allBoss = dic;
            allNormal = List;
        }
    }

    public class translateCfg
    {
        public int id;
        public string english;
        public string chinese;


        private static Dictionary<int, translateCfg> all = null;
        private static List<translateCfg> allList = null;

        public static Dictionary<int, translateCfg> GetAll()
        {
            return all;
        }

        public static List<translateCfg> GetAllList()
        {
            return allList;
        }

        public void setcfg(Dictionary<int, translateCfg> dic, List<translateCfg> List)
        {
            all = dic;
            allList = List;
        }

    }
}


public class FileManage : Node
{
    public static cfg.playerCoreCfg playerCoreCfgFile = new cfg.playerCoreCfg();
    public static cfg.stageLevelInfoCfg stageLevelInfoCfgFile = new cfg.stageLevelInfoCfg();
    public static cfg.enemyInfoCfg enemyInfoCfgFile = new cfg.enemyInfoCfg();
    public static cfg.bulletInfoCfg bulletInfoCfgFile = new cfg.bulletInfoCfg();
    public static cfg.shipBuildAttributesCfg shipBuildAttributesCfgFile = new cfg.shipBuildAttributesCfg();
    public static cfg.rangEnemyCfg rangEnemyCfgFile = new cfg.rangEnemyCfg();
    public static cfg.translateCfg translateCfgFile = new cfg.translateCfg();
    public override void _Ready()
    {
        List<Dictionary<string, string>> list1 = _parse_csv_file("res://configuration/F-飞船组件表.csv");
        List<Dictionary<string, string>> list2 = _parse_csv_file("res://configuration/G-关卡表.csv");
        List<Dictionary<string, string>> list3 = _parse_csv_file("res://configuration/G-怪物表.csv");
        List<Dictionary<string, string>> list4 = _parse_csv_file("res://configuration/Z-子弹表.csv");
        List<Dictionary<string, string>> list5 = _parse_csv_file("res://configuration/S-属性表.csv");
        List<Dictionary<string, string>> list6 = _parse_csv_file("res://configuration/S-随机敌人表.csv");
        List<Dictionary<string, string>> list7 = _parse_csv_file("res://configuration/Y-语言表.csv");
        listIntoCfg(list1, playerCoreCfgFile);               
        listIntoCfg(list2, stageLevelInfoCfgFile);
        listIntoCfg(list3, enemyInfoCfgFile);
        listIntoCfg(list4, bulletInfoCfgFile);
        listIntoCfg(list5, shipBuildAttributesCfgFile);
        listIntoCfg(list6, rangEnemyCfgFile);
        listIntoCfg(list7, translateCfgFile);
    }

    public void listIntoCfg<T>(List<Dictionary<string, string>> list, T cfg) //解析为具体的类
    {
        if(cfg.GetType() == typeof(cfg.playerCoreCfg))
        {
            Dictionary<int, cfg.playerCoreCfg> dicNew = new Dictionary<int, cfg.playerCoreCfg>();
            List<cfg.playerCoreCfg> listcfg = new List<playerCoreCfg>();
            for (int i = 0; i < list.Count; i++)
            {

                cfg.playerCoreCfg cfgNew = new cfg.playerCoreCfg();
                foreach(var item in list[i])
                {
                    FieldInfo fieldInfo = cfgNew.GetType().GetField(item.Key);
                    if (fieldInfo != null)
                    {
                        cfgNew.GetType().GetField(item.Key).SetValue(cfgNew, transValue(item.Value, fieldInfo.FieldType));
                        //GD.Print(item.Key+"   "+ fieldInfo.FieldType + "  " + transValue(item.Value, fieldInfo.FieldType));
                    }
                        

                }
                cfgNew.dealWithAttri();
                listcfg.Add(cfgNew);
                dicNew.Add(i, cfgNew);
                
            }
            playerCoreCfgFile.setcfg(dicNew, listcfg);
        }

        if (cfg.GetType() == typeof(cfg.stageLevelInfoCfg))
        {
            Dictionary<int, cfg.stageLevelInfoCfg> dicNew = new Dictionary<int, cfg.stageLevelInfoCfg>();
            List<cfg.stageLevelInfoCfg> listcfg = new List<stageLevelInfoCfg>();
            for (int i = 0; i < list.Count; i++)
            {
                cfg.stageLevelInfoCfg cfgNew = new cfg.stageLevelInfoCfg();
                foreach (var item in list[i])
                {
                    FieldInfo fieldInfo = cfgNew.GetType().GetField(item.Key);
                    if (fieldInfo != null)
                    {
                        cfgNew.GetType().GetField(item.Key).SetValue(cfgNew, transValue(item.Value, fieldInfo.FieldType));
                        //GD.Print(item.Key+"   "+ fieldInfo.FieldType + "  " + transValue(item.Value, fieldInfo.FieldType));
                    }


                }

                listcfg.Add(cfgNew);
                dicNew.Add(i, cfgNew);

            }
            stageLevelInfoCfgFile.setcfg(dicNew, listcfg);
        }

        if (cfg.GetType() == typeof(cfg.enemyInfoCfg))
        {
            Dictionary<int, cfg.enemyInfoCfg> dicNew = new Dictionary<int, cfg.enemyInfoCfg>();
            List<cfg.enemyInfoCfg> listcfg = new List<enemyInfoCfg>();
            for (int i = 0; i < list.Count; i++)
            {
                cfg.enemyInfoCfg cfgNew = new cfg.enemyInfoCfg();
                foreach (var item in list[i])
                {
                    FieldInfo fieldInfo = cfgNew.GetType().GetField(item.Key);
                    if (fieldInfo != null)
                    {
                        cfgNew.GetType().GetField(item.Key).SetValue(cfgNew, transValue(item.Value, fieldInfo.FieldType));
                        //GD.Print(item.Key+"   "+ fieldInfo.FieldType + "  " + transValue(item.Value, fieldInfo.FieldType));
                    }


                }


                listcfg.Add(cfgNew);
                dicNew.Add(i, cfgNew);

            }
            enemyInfoCfgFile.setcfg(dicNew, listcfg);
        }

        if (cfg.GetType() == typeof(cfg.bulletInfoCfg))
        {
            Dictionary<int, cfg.bulletInfoCfg> dicNew = new Dictionary<int, cfg.bulletInfoCfg>();
            List<cfg.bulletInfoCfg> listcfg = new List<bulletInfoCfg>();
            for (int i = 0; i < list.Count; i++)
            {
                cfg.bulletInfoCfg cfgNew = new cfg.bulletInfoCfg();
                foreach (var item in list[i])
                {
                    FieldInfo fieldInfo = cfgNew.GetType().GetField(item.Key);
                    if (fieldInfo != null)
                    {
                        cfgNew.GetType().GetField(item.Key).SetValue(cfgNew, transValue(item.Value, fieldInfo.FieldType));
                        //GD.Print(item.Key+"   "+ fieldInfo.FieldType + "  " + transValue(item.Value, fieldInfo.FieldType));
                    }


                }


                listcfg.Add(cfgNew);
                dicNew.Add(i, cfgNew);

            }
            bulletInfoCfgFile.setcfg(dicNew, listcfg);
        }

        if (cfg.GetType() == typeof(cfg.shipBuildAttributesCfg))
        {
            Dictionary<int, cfg.shipBuildAttributesCfg> dicNew = new Dictionary<int, cfg.shipBuildAttributesCfg>();
            List<cfg.shipBuildAttributesCfg> listcfg = new List<shipBuildAttributesCfg>();
            for (int i = 0; i < list.Count; i++)
            {
                cfg.shipBuildAttributesCfg cfgNew = new cfg.shipBuildAttributesCfg();
                foreach (var item in list[i])
                {
                    FieldInfo fieldInfo = cfgNew.GetType().GetField(item.Key);
                    if (fieldInfo != null)
                    {
                        cfgNew.GetType().GetField(item.Key).SetValue(cfgNew, transValue(item.Value, fieldInfo.FieldType));
                        //GD.Print(item.Key+"   "+ fieldInfo.FieldType + "  " + transValue(item.Value, fieldInfo.FieldType));
                    }


                }


                listcfg.Add(cfgNew);
                dicNew.Add(i, cfgNew);

            }
            shipBuildAttributesCfgFile.setcfg(dicNew, listcfg);
        }


        if (cfg.GetType() == typeof(cfg.rangEnemyCfg))
        {
            Dictionary<int, cfg.rangEnemyCfg> dicNew = new Dictionary<int, cfg.rangEnemyCfg>();
            List<cfg.rangEnemyCfg> listcfg = new List<rangEnemyCfg>();
            List<cfg.rangEnemyCfg> allBoss = new List<rangEnemyCfg>();
            List<cfg.rangEnemyCfg> allLittle = new List<rangEnemyCfg>();
            for (int i = 0; i < list.Count; i++)
            {

                cfg.rangEnemyCfg cfgNew = new cfg.rangEnemyCfg();
                foreach (var item in list[i])
                {
                    FieldInfo fieldInfo = cfgNew.GetType().GetField(item.Key);
                    if (fieldInfo != null)
                    {
                        cfgNew.GetType().GetField(item.Key).SetValue(cfgNew, transValue(item.Value, fieldInfo.FieldType));
                        //GD.Print(item.Key+"   "+ fieldInfo.FieldType + "  " + transValue(item.Value, fieldInfo.FieldType));
                    }


                }
                listcfg.Add(cfgNew);
                dicNew.Add(i, cfgNew);
                if(cfgNew.isBoss == true)
                {
                    allBoss.Add(cfgNew);
                }
                else
                {
                    allLittle.Add(cfgNew);
                }
                

            }
            rangEnemyCfgFile.setcfg(dicNew, listcfg);
            rangEnemyCfgFile.setcfg(allBoss, allLittle);
        }


        if (cfg.GetType() == typeof(cfg.translateCfg))
        {
            Dictionary<int, cfg.translateCfg> dicNew = new Dictionary<int, cfg.translateCfg>();
            List<cfg.translateCfg> listcfg = new List<translateCfg>();

            for (int i = 0; i < list.Count; i++)
            {

                cfg.translateCfg cfgNew = new cfg.translateCfg();
                foreach (var item in list[i])
                {
                    FieldInfo fieldInfo = cfgNew.GetType().GetField(item.Key);
                    if (fieldInfo != null)
                    {
                        cfgNew.GetType().GetField(item.Key).SetValue(cfgNew, transValue(item.Value, fieldInfo.FieldType));
                        //GD.Print(item.Key+"   "+ fieldInfo.FieldType + "  " + transValue(item.Value, fieldInfo.FieldType));
                    }


                }
                listcfg.Add(cfgNew);
                dicNew.Add(i, cfgNew);


            }
            translateCfgFile.setcfg(dicNew, listcfg);
        }
    }








    private List<Dictionary<string, string>> _parse_csv_file(string path) //文件解析
    {
        List<List<string>> dataAll = new List<List<string>>();
        List<Dictionary<string, string>> returnData = new List<Dictionary<string, string>>();
        var file = new File();
        file.Open(path, File.ModeFlags.Read);
        var temp = file.GetCsvLine(); //一行一行的读
        while(temp.Length > 1)
        {
            List<string> data = new List<string>();
            for (int i = 0; i < temp.Length; i++)
            {
                data.Add(temp[i]);
                //Debug.Print(temp[i] + "  ");
            }
            dataAll.Add(data);
            temp = file.GetCsvLine();
            
        }        
       
        file.Close();
        for(int i = 1;i< dataAll.Count; i++)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            for (int j =0;j < dataAll[i].Count; j++)
            {                
                dic.Add(dataAll[0][j], dataAll[i][j]);
            }
            returnData.Add(dic);
        }

        return returnData;
    }


    private Dictionary<string, string> stringToDic(string str)
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();
        if(str.Length > 0)
        {
            string[] strArray = str.Split(";");
            for (int i = 0; i < strArray.Length; i++)
            {
                string[] strArrayb = strArray[i].Split(":");
                dic.Add(strArrayb[0], strArrayb[1]);
            }
        }
        return dic;
    }


    private Dictionary<int, int> stringToDicI(string str)
    {
        Dictionary<int, int> dic = new Dictionary<int, int>();
        if (str.Length > 0)
        {
            string[] strArray = str.Split(";");
            for (int i = 0; i < strArray.Length; i++)
            {
                string[] strArrayb = strArray[i].Split(":");
                dic.Add(strArrayb[0].ToInt(), strArrayb[1].ToInt());
            }
        }
        return dic;
    }


    private List<int> stringToListI(string str)
    {
        List<int> list = new List<int>();
        if (str.Length > 0)
        {
            string[] strArray = str.Split(";");
            for (int i = 0; i < strArray.Length; i++)
            {
                list.Add(strArray[i].ToInt());
            }
        }
        return list;
    }

    private List<float> stringToListF(string str)
    {
        List<float> list = new List<float>();
        if (str.Length > 0)
        {
            string[] strArray = str.Split(";");
            for (int i = 0; i < strArray.Length; i++)
            {
                list.Add(strArray[i].ToFloat());
            }
        }
        return list;
    }


    private bool stringToBool(string str)
    {
        var a = str.ToInt();
        if (a > 0)
            return true;
        else
            return false;


    }


    //动态类型转换
    private object transValue(string str,Type type)
    {
        
        if (type == typeof(int))
            return Convert.ChangeType(str, typeof(int));
        if (type == typeof(float))
            return Convert.ChangeType(str, typeof(float));
        if (type == typeof(shipPartDataType))
        {
            if (str.ToInt() == 0)
                return shipPartDataType.core;
            if (str.ToInt() == 1)
                return shipPartDataType.derived;
        }
        if (type == typeof(Dictionary<string, string>))
            return stringToDic(str);
        if (type == typeof(Dictionary<int, int>))
            return stringToDicI(str);
        if (type == typeof(List<int>))
            return stringToListI(str);
        if (type == typeof(List<float>))
            return stringToListF(str);
        if (type == typeof(bool))
            return stringToBool(str);

        return (object)str;
    }


}
