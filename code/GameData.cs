using cfg;
using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;

public class GameData : Node2D
{
    //主要处理存储数据


    private const string SaveData_playerBulidPacked= "SaveData_playerBulidPacked";//仓库部件
    private const string SaveData_playerBulidData = "SaveData_playerBulidData";//玩家飞船组件
    private const string SaveData_playerGoldData = "SaveData_playerGoldData";//玩家金币
    private const string SaveData_playerEevetGoldData = "SaveData_playerEevetGoldData";//玩家已有金币
    private const string SaveData_playerUnlokcedPacked = "SaveData_playerUnlokcedPacked";//玩家已经解锁部位
    private const string SaveData_playerEverPlayedLevel = "SaveData_playerEverPlayedLevel";//玩家已经玩到的关卡
    private const string SaveData_playerLastPlayerLevel = "SaveData_playerLastPlayerLevel";//玩家关卡重复次数

    private const string SaveData_playerUnlokcedPackedALLSave = "SaveData_playerUnlokcedPackedALLSave";//玩家已经解锁部位 展示用
    private const string SaveData_playerUsedLanguage = "SaveData_playerUsedLanguage";//玩家正在使用的语言

    private baseBulidPart _playerBulidData;

    private Dictionary<int, int> _playerBulidPacked = new Dictionary<int, int>(); //序号 表中的id 正在使用的仓库
    private List<baseBulidPart> _playerUsingPaced = new List<baseBulidPart>(); //正在使用的部件的序号
    private List<int> _unlockedPacked = new List<int>();//已解锁部件

    private List<int> _unlockedPackedForever = new List<int>();//已永久解锁部件

    public int playerEverPlayedLevel = 0;

    public int playerLastPlayerLevel = 0;

    private int _playersGold = GlobalConstant.BEGINNINGGOLD;

    private int _playerGoldEver = 0; //统计数据



    public language languageNow = language.chinese;
    public enum language
    {
        chinese = 0,
        english = 1
    }
    public List<int> UnlockedPacked
    {
        get
        {
            if (_unlockedPacked.Count == 0)
            {
                foreach (var item in cfg.playerCoreCfg.GetAllList())
                {

                    if (item.unlockedBegin)
                    {
                       _unlockedPacked.Add(item.id);
                    }                   

                }
            }
            return _unlockedPacked;
        }
        set
        {
            _unlockedPacked = value;
        }
    }


    public List<int> unlockedPackedForever
    {
        get
        {
            if(_unlockedPackedForever.Count == 0)
            {
                foreach (var item in cfg.playerCoreCfg.GetAllList())
                {
                    if (item.id % 10 == 0)
                    {
                        if (item.unlockedBegin == true)
                        {
                            if (!_unlockedPackedForever.Contains(item.id))
                                _unlockedPackedForever.Add(item.id);
                        }
                    }

                }
            }
            return _unlockedPackedForever;
        }
        set
        {
            _unlockedPackedForever = value;
        }
    }


    public int PlayersGold
    {
        get
        {
            return _playersGold;
        }

    }

    public int PlayerGoldEver
    {
        get
        {
            return _playerGoldEver;
        }

    }

    public void goldChange(int number)
    {
        _playersGold += number;
        if(number > 0)
            _playerGoldEver += number;

    }

    public void resumeGold()
    {
        _playersGold = GlobalConstant.BEGINNINGGOLD;
        _playerGoldEver = _playersGold;
    }

    //数据 ##测试数据位置
    public Dictionary<int,int> playerBulidPacked
    {
        get
        {
            if(_playerBulidPacked.Count == 0)
            {
                _playerBulidPacked.Add(0, 100010);

                //_playerBulidPacked.Add(1, 200010);
            }
            return _playerBulidPacked;
        }
        set
        {
            _playerBulidPacked = value;
        }
    }

    public List<baseBulidPart> playerUsingPaced
    {
        get
        {
            return _playerUsingPaced;
        }
        set
        {
            playerUsingPaced.Clear();
            _playerUsingPaced = value;
        }
    }



    public baseBulidPart playerBulidData
    {
        get
        {
            if(IsInstanceValid(_playerBulidData))
            {
                return _playerBulidData;
            }else
            {
                return null;
            }
            
        }
        set
        {
            if(value == null)
            {
                _playerBulidData = null;
                return;
            }
            if (IsInstanceValid(_playerBulidData))
            {
                _playerBulidData.Free();
            }
            _playerBulidData = value;
            _playerBulidData.GetParent().RemoveChild(_playerBulidData);
            AddChild(_playerBulidData);
            playerUsingPaced = _playerBulidData.setList();
            //SaveGame();
            //GD.Print(_playerBulidData);
        }
    }
    public override void _Ready()
    {

        
    }


    public void SaveGame()
    {
        Godot.Collections.Dictionary<string, object> saveData = new Godot.Collections.Dictionary<string, object>();

        saveData.Add(SaveData_playerUnlokcedPacked, UnlockedPacked);//已解锁部件
        saveData.Add(SaveData_playerBulidPacked, playerBulidPacked);//部件存储
        saveData.Add(SaveData_playerBulidData, playerBulidData.Save());//保存玩家飞船
        saveData.Add(SaveData_playerGoldData, PlayersGold);//金币
        saveData.Add(SaveData_playerEevetGoldData, PlayerGoldEver);//已解锁金币
        saveData.Add(SaveData_playerEverPlayedLevel, playerEverPlayedLevel);//已玩到的关卡
        saveData.Add(SaveData_playerLastPlayerLevel, playerLastPlayerLevel);//重复的关卡次数
        saveData.Add(SaveData_playerUsedLanguage, languageNow);//正在使用的语言

        var saveGame = new Godot.File();
        saveGame.Open("user://savegame1.save", Godot.File.ModeFlags.Write);

        // Store the save dictionary as a new line in the save file.
        saveGame.StoreLine(JSON.Print(saveData));

        saveGame.Close();


        Godot.Collections.Dictionary<string, object> saveDataForever = new Godot.Collections.Dictionary<string, object>();

        saveDataForever.Add(SaveData_playerUnlokcedPackedALLSave, unlockedPackedForever);//永久存档 保存玩家已解锁部件

        var saveGameForever = new Godot.File();
        saveGameForever.Open("user://savegame2.save", Godot.File.ModeFlags.Write);

        // Store the save dictionary as a new line in the save file.
        saveGameForever.StoreLine(JSON.Print(saveDataForever));

        saveGameForever.Close();

    }

    //保持已解锁的部件
    public void LoadForeverData()
    {
        var saveGame = new Godot.File();
        if (!saveGame.FileExists("user://savegame2.save"))
            return; // Error! We don't have a save to load.
        saveGame.Open("user://savegame2.save", Godot.File.ModeFlags.Read);
        if (saveGame.GetLen() < 3)
            return;
        while (saveGame.GetPosition() < saveGame.GetLen())
        {
            // Get the saved dictionary from the next line in the save file
            var nodeData = new Godot.Collections.Dictionary<string, object>((Godot.Collections.Dictionary)JSON.Parse(saveGame.GetLine()).Result);
            foreach (KeyValuePair<string, object> entry in nodeData)
            {
                string key = entry.Key.ToString();

                if (key == SaveData_playerUnlokcedPackedALLSave)
                {
                    var playerUnlokcedPacked = new Godot.Collections.Array<string>((Godot.Collections.Array)entry.Value);
                    foreach (var item in playerUnlokcedPacked)
                    {
                        if (!unlockedPackedForever.Contains(item.ToString().ToInt()))
                        {
                            unlockedPackedForever.Add(item.ToString().ToInt());
                        }
                        
                    }
                }
                //GD.Print("  LoadGame()   " + entry.Value.GetType());
                //GD.Print("  LoadGame()   " + entry.Value);
            }

        }
        saveGame.Close();
    }



    public void LoadGame()
    {
        var saveGame = new Godot.File();
        if (!saveGame.FileExists("user://savegame1.save"))
            return; // Error! We don't have a save to load.
        baseBulidPart bulidData = new baseBulidPart();
        AddChild(bulidData);
        // Load the file line by line and process that dictionary to restore the object
        // it represents.
        saveGame.Open("user://savegame1.save", Godot.File.ModeFlags.Read);
        if (saveGame.GetLen() < 3)
        {
            bulidData.QueueFree();
            return;
        }           
        //GD.Print("  LoadGame()   " + saveGame.GetLen());
        while (saveGame.GetPosition() < saveGame.GetLen())
        {
            // Get the saved dictionary from the next line in the save file
            var nodeData = new Godot.Collections.Dictionary<string, object>((Godot.Collections.Dictionary)JSON.Parse(saveGame.GetLine()).Result);
            foreach (KeyValuePair<string, object> entry in nodeData)
            {
                string key = entry.Key.ToString();
                if (key == SaveData_playerBulidPacked)
                {
                    //GD.Print("  LoadGame()   " + entry.Value.GetType());
                    //GD.Print("  LoadGame()   " + entry.Value);
                    var playerBulidPackedData = new Godot.Collections.Dictionary<string, object>((Godot.Collections.Dictionary)entry.Value);
                    foreach (var item in playerBulidPackedData)
                    {
                        _playerBulidPacked.Add(item.Key.ToInt(), item.Value.ToString().ToInt());
                    }
                }
                    
                if (key == SaveData_playerBulidData)
                    playerBulidData = bulidData.loadData(new Godot.Collections.Dictionary<string, object>((Godot.Collections.Dictionary)entry.Value));
                if (key == SaveData_playerGoldData)
                    _playersGold = entry.Value.ToString().ToInt();
                if (key == SaveData_playerUnlokcedPacked)
                {
                    var playerUnlokcedPacked = new Godot.Collections.Array<string>((Godot.Collections.Array)entry.Value);
                    foreach (var item in playerUnlokcedPacked)
                    {
                        _unlockedPacked.Add(item.ToString().ToInt());
                    }
                }
                if(key == SaveData_playerEevetGoldData)
                    _playerGoldEver = entry.Value.ToString().ToInt();
                if (key == SaveData_playerEverPlayedLevel)
                    playerEverPlayedLevel = entry.Value.ToString().ToInt();
                if (key == SaveData_playerLastPlayerLevel)
                    playerLastPlayerLevel = entry.Value.ToString().ToInt();
                if (key == SaveData_playerUsedLanguage)
                {
                    if (entry.Value.ToString().ToInt() == 0)
                        languageNow = language.chinese;
                    if (entry.Value.ToString().ToInt() == 1)
                        languageNow = language.english;
                }
                //GD.Print("  LoadGame()   " + entry.Value.GetType());
                //GD.Print("  LoadGame()   " + entry.Value);
            }

        }
        bulidData.QueueFree();
        saveGame.Close();
    }


    public void clearData()
    {
        var saveGame = new Godot.File();
        saveGame.Open("user://savegame1.save", Godot.File.ModeFlags.Write);


        // Store the save dictionary as a new line in the save file.
        saveGame.StoreLine("");


        saveGame.Close();
    }

}
