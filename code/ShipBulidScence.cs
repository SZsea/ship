using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Numerics;
using Vector2 = Godot.Vector2;

public class ShipBulidScence : SZBaseNode2D
{

    [Export]
    public NodePath CameraPath;
    [Export]
    public NodePath PlayerBodyNodePath;
    [Export]
    public NodePath PlayerBodyNodeAreaPath;
    [Export]
    public NodePath StartButtonPath;
    [Export]
    public NodePath ScrollContainerPath;
    [Export]
    public NodePath BuildUILayerPath;
    [Export]
    public NodePath ShoppingButtonPath;
    [Export]
    public NodePath ShoppingUIPanelPath;
    [Export]
    public NodePath DeleteAreaPanelPath;
    [Export]
    public NodePath VisibileNodePath;
    [Export]
    public NodePath bgPath;
    [Export]
    public NodePath deletePath;

    private Camera2D _camera;
    private Node2D _playerBodyNode;
    private Control _playerBodyNodeArea;
    private bulidScenceScroll _scrollContainer;
    private Button _startButton;
    private BulidUILayer _bulidUILayer;
    private Button _shoppingButton;
    private BulidUI_partLayer _shipPartDiscLayer;
    private BulidUI_shopping _shoppingUIPanel;
    private Control _deleteAreaArea;
    private CustomNotifyPanel _visibileNode;
    private Label deleteLabel;

    public GameMainBg _bg;


    public int LevelIndex
    {
        set
        {
            _shoppingUIPanel.levelIndex = value;
        }
    }


    public override void _Ready()
    {
        //如果输入没有反应检查tscn各个部件的输入检测是否冲突
        _camera = GetNode<Camera2D>(CameraPath);
        _camera.Current = true;
        _playerBodyNode = GetNode<Node2D>(PlayerBodyNodePath);
        _playerBodyNodeArea = GetNode<Control>(PlayerBodyNodeAreaPath);
        _scrollContainer = GetNode<bulidScenceScroll>(ScrollContainerPath);
        _scrollContainer.language = gameDate().languageNow;
        _scrollContainer.fatherNode = this;
        _bulidUILayer = GetNode<BulidUILayer>(BuildUILayerPath);
        _deleteAreaArea = GetNode<Control>(DeleteAreaPanelPath);
        _visibileNode = GetNode<CustomNotifyPanel>(VisibileNodePath);
        _bg = GetNode<GameMainBg>(bgPath);
        _bg.hideShip();
        deleteLabel = GetNode<Label>(deletePath);
        if (gameDate().languageNow == GameData.language.chinese)
            deleteLabel.Text = "部件回收";
        if (gameDate().languageNow == GameData.language.english)
            deleteLabel.Text = "recycling";


        _playerBodyNodeArea.MouseFilter = Control.MouseFilterEnum.Ignore; //关闭区域的输入检测
        _deleteAreaArea.MouseFilter = Control.MouseFilterEnum.Ignore; //关闭区域的输入检测
        _startButton = (Button)GetNode(StartButtonPath);
        if(gameDate().languageNow == GameData.language.chinese)
            _startButton.Text = "开始游戏";
        if (gameDate().languageNow == GameData.language.english)
            _startButton.Text = "start";

        _shoppingButton = (Button)GetNode(ShoppingButtonPath);
        if (gameDate().languageNow == GameData.language.chinese)
            _shoppingButton.Text = "商店";
        if (gameDate().languageNow == GameData.language.english)
            _shoppingButton.Text = "shop";

        _shoppingButton.Connect("pressed", this, "_ShowShopping");
        var panel = GD.Load<PackedScene>("res://scence/UI/BulidUI_partLayer.tscn");
        _shipPartDiscLayer = (BulidUI_partLayer)panel.Instance();
        AddChild(_shipPartDiscLayer);
        _shipPartDiscLayer.Hide();
        _shoppingUIPanel = (BulidUI_shopping)GetNode(ShoppingUIPanelPath);
        _shoppingUIPanel.languageNow = gameDate().languageNow;
        _shoppingUIPanel._shipBulidScence = this;
        _shoppingUIPanel.changePirceGold(freshShipGold[_shoppingUIPanel.refreshTime]);
        _shoppingUIPanel.Hide();
        



        List<string> path = new List<string>();
        List<cfg.playerCoreCfg.shipPartDataType> pathtype = new List<cfg.playerCoreCfg.shipPartDataType>();
        List<int> sequenceId = new List<int>();
        List<int> bulidId = new List<int>();
        List<int> updateId = new List<int>();
        List<int> gold = new List<int>();
        List<int> qualitys = new List<int>(); 

        foreach (var item in gameDate().playerBulidPacked)
        {
            foreach (var itemCfg in cfg.playerCoreCfg.GetAllList())
            {
                if (itemCfg.id == item.Value)
                {
                    path.Add(itemCfg.path);
                    pathtype.Add(itemCfg.type);
                    sequenceId.Add(item.Key);
                    bulidId.Add(itemCfg.id);
                    updateId.Add(itemCfg.updateId);
                    gold.Add(itemCfg.updateGold);
                    qualitys.Add(itemCfg.quality);

                }
            }
        }
        _scrollContainer.initWithData(path, pathtype, sequenceId, bulidId, updateId,gold, qualitys);
        //正在使用的部位
        if(gameDate().playerUsingPaced.Count > 0)
        {
            var player1In = gameDate().playerBulidData.copy();
            player1In.GetParent().RemoveChild(player1In);
            _playerBodyNode.AddChild(player1In);
            _coreItem = player1In;
            _coreItem.GlobalPosition = _playerBodyNode.GlobalPosition;
            
            ItemDic = player1In.setList();
            foreach(var item in ItemDic)
            {
                item.Connect("input_event", this, "_on_core_input_event", new Godot.Collections.Array(item));
                item.InputPickable = true;
                //GD.Print(item, "+++++", player1In.IsConnected("input_event", this, "_on_core_input_event"));
            }
           
            gameDate().playerUsingPaced = _coreItem.setList();
            _scrollContainer.updateData(gameDate().playerUsingPaced);
            _bulidUILayer.updateWithData(_coreItem,gameDate().languageNow);            
            _coreItem.checkinterfaceChannel();
            _coreItem.shipCheckAssembledAgain(_coreItem.setList());
        }
        else
        {
            //初始位置
            foreach (var item in gameDate().playerBulidPacked)
            {
                foreach (var itemCfg in cfg.playerCoreCfg.GetAllList())
                {
                    if (itemCfg.id == item.Value && item.Key == 0)
                    {
                        var player1 = GD.Load<PackedScene>(itemCfg.path);
                        var player1In = (baseBulidPart)player1.Instance();
                        _playerBodyNode.AddChild(player1In);
                        player1In.GlobalPosition = GetGlobalMousePosition();
                        player1In.InputPickable = true;
                        player1In.onlyId = item.Key;
                        player1In.buildId = itemCfg.id;
                        player1In.ConnectPanel = _scrollContainer._firstPanel;
                        player1In.Connect("input_event", this, "_on_core_input_event", new Godot.Collections.Array(player1In));
                        ItemDic.Add(player1In);
                        _coreItem = player1In;
                        _coreItem.GlobalPosition = _playerBodyNode.GlobalPosition;
                    }
                }
            }
            //GD.Print(gameDate().playerBulidPacked);
            gameDate().playerUsingPaced = _coreItem.setList();
            _scrollContainer.updateData(gameDate().playerUsingPaced);
            _bulidUILayer.updateWithData(_coreItem, gameDate().languageNow);
            _coreItem.checkinterfaceChannel();
            _coreItem.shipCheckAssembledAgain(_coreItem.setList());
        }
        _bulidUILayer.updateGold(gameDate().PlayersGold);
    }




    private List<baseBulidPart> ItemDic = new List<baseBulidPart>();
    private baseBulidPart _coreItem;
    public void newItem(string path, CustomPanel panel, cfg.playerCoreCfg.shipPartDataType pathType,int bulidId)
    {
        var player1 = GD.Load<PackedScene>(path);
        var player1In = (baseBulidPart)player1.Instance();
        _playerBodyNode.AddChild(player1In);
        player1In.GlobalPosition = GetGlobalMousePosition();
        player1In.InputPickable = true;
        player1In.onlyId = panel.sequenceId;
        player1In.buildId = bulidId;
        player1In.ConnectPanel = panel;
        player1In.Connect("input_event", this, "_on_core_input_event", new Godot.Collections.Array(player1In));
       
        ItemDic.Add(player1In);
        if(pathType == cfg.playerCoreCfg.shipPartDataType.core && !IsInstanceValid(_coreItem))
        {
            _coreItem = player1In;
            _coreItem.GlobalPosition = _playerBodyNode.GlobalPosition;
        }

        if (!GlobalConstant.isOutAreaOther(player1In.GlobalPosition, player1In.size(), _deleteAreaArea.RectGlobalPosition, _deleteAreaArea.RectSize))
        {
            
            //GD.Print("删除");
            deletePart(player1In.onlyId);
        }

        if (!GlobalConstant.isOutArea(player1In.GlobalPosition, player1In.size(), _playerBodyNodeArea.RectGlobalPosition, _playerBodyNodeArea.RectSize))
        {
            player1In.ConnectPanel.isSelected = true;
            shipCheckAssembled(player1In);
        }
        else
        {
            ItemDic.Remove(player1In);
            player1In.QueueFree();           
        }
        if(IsInstanceValid(_coreItem))
           _coreItem.checkinterfaceChannel();
        _coreItem.shipCheckAssembledAgain(_coreItem.setList());
        //GD.Print(player1In.IsConnected("input_event", this, "_on_core_input_event"));
    }

    private bool _dragging = false;
    private baseBulidPart _draggingItem;

    //屏蔽其他
    private void shieldOtherInput(baseBulidPart item,bool YrN)
    {
        foreach(var i in ItemDic)
        {
            if(i != item)
            {
                i.SetBlockSignals(YrN);
            }
        }
    }

    //拖动移动
    private void _on_core_input_event(Node viewport, InputEvent @event, int shape_idx, baseBulidPart item)
    {
        //GD.Print("mouseEvent" + viewport);
        if (@event is InputEventMouseButton mouseEvent && (ButtonList)mouseEvent.ButtonIndex == ButtonList.Left)
        {
            //GD.Print("mouseEvent" + mouseEvent);
            if (!_dragging && mouseEvent.Pressed)
            {
                _draggingItem = item;
                _dragging = true;
                GD.Print("dragging" + item);
                shieldOtherInput(item, true);
                //panel.isSelected = true;
                _shipPartDiscLayer.Show();
                _shipPartDiscLayer.updateWithData(item);
                _shipPartDiscLayer.SetGlobalPosition(item.GlobalPosition + new Vector2(100, 20));
            }

            // Stop dragging if the button is released.
            if (_dragging && !mouseEvent.Pressed)
            {

                _shipPartDiscLayer.Hide();
                _dragging = false;
                GD.Print("released" + item);


                if (!GlobalConstant.isOutAreaOther(item.GlobalPosition, new Vector2(64, 64), _deleteAreaArea.RectGlobalPosition, _deleteAreaArea.RectSize))
                {
                    deletePart(item.onlyId);
                    //GD.Print("删除");
                }

                if (GlobalConstant.isOutArea(item.GlobalPosition,new Vector2(64,64), _playerBodyNodeArea.RectGlobalPosition, _playerBodyNodeArea.RectSize))
                {
                    if (item == _coreItem)
                    {
                        _coreItem.GlobalPosition = new Vector2(0, 0);
                        //杜绝删掉原始
                        //_coreItem = null;
                    }else
                    {
                        item.ConnectPanel.isSelected = false;
                        item.removeFromDic(ItemDic);
                        if (item.GetParent() == _coreItem)
                        {
                            _coreItem.resetChildPostion(item);                           
                        }
                        item.CustomFree();

                    }
                        
                }else
                {
                    if (item == _coreItem)
                    {
                        _coreItem.GlobalPosition = _playerBodyNode.GlobalPosition;
                    }
                    if (item.GetParent() == _playerBodyNode)
                    {
                        shipCheckAssembled(item);
                        
                    }
                    else
                    {
                        var parent = item.GetParent<baseBulidPart>();
                        Vector2 oldpositon = item.GlobalPosition;
                        parent.resetChildPostion(item);
                        GD.Print("_coreItem       +++++++++++++          _coreItem");
                        //shipCheckAssembled(item);
                        //GD.Print("item.GetParent()" + item.GetParent());
                        //错误返回位置
                        if (item.GetParent() == null)
                        {
                            
                            _playerBodyNode.AddChild(item);
                            item.GlobalPosition = oldpositon;
                           
                        }
                    }
                   
                }
                
            }
            shieldOtherInput(item, false);



            if (IsInstanceValid(_coreItem))
            {
                //GD.Print("_bulidUILayer.updateWithData!!!");
                _coreItem.checkinterfaceChannel();                
                _bulidUILayer.updateWithData(_coreItem, gameDate().languageNow);
            }
            else
            {
                //GD.Print("_bulidUILayer.updateWithData");
                _bulidUILayer.updateWithData(_coreItem, gameDate().languageNow);
            }
            _coreItem.shipCheckAssembledAgain(ItemDic);
        }           
    }




    private void shipCheckAssembled(baseBulidPart item)
    {
        var newdic = GlobalConstant.calculateDistaceDic(item, ItemDic, 100f);
        
        if (newdic.Count > 0)
        {
            baseBulidPart nearlyitem = newdic.Aggregate((x, y) => x.Value < y.Value ? x : y).Key;


            if (nearlyitem == _coreItem)
            {
                if ((item.GlobalPosition - nearlyitem.GlobalPosition).Length() < 60f)
                {
                    item.GlobalPosition = nearlyitem.GlobalPosition + new Vector2(0, 100f);
                }
                //GD.Print("_coreItem                 _coreItem");
               
                if (nearlyitem.CheckEnabelInterfacePosition(nearlyitem.nearlyInterFace(item.GlobalPosition)) && item.checkIsCanConnect(nearlyitem))
                {
                    nearlyitem.AddChildRe(nearlyitem.nearlyInterFace(item.GlobalPosition), item);

                }


            }
            else if(!item.checkIsConnect(nearlyitem))
            {
                if ((item.GlobalPosition - nearlyitem.GlobalPosition).Length() < 60f)
                {
                    item.GlobalPosition = nearlyitem.GlobalPosition + new Vector2(0, 100f);
                }

                if (item == _coreItem)
                {
                    if (item.CheckEnabelInterfacePosition(item.nearlyInterFace(nearlyitem.GlobalPosition)) && nearlyitem.checkIsCanConnect(item))
                    {
                        item.AddChildRe(item.nearlyInterFace(nearlyitem.GlobalPosition), nearlyitem);
                    }
                }
                else
                {
                   // GD.Print("CheckEnabelInterfacePosition" + nearlyitem + "  " + item + "  " + nearlyitem.nearlyInterFace(item.GlobalPosition));
                    //GD.Print("CheckEnabelInterfacePosition----------------------------------------------------------------------------" );

                    if (nearlyitem.CheckEnabelInterfacePosition(nearlyitem.nearlyInterFace(item.GlobalPosition)) && item.checkIsCanConnect(nearlyitem))
                    {

                        nearlyitem.AddChildRe(nearlyitem.nearlyInterFace(item.GlobalPosition), item);
                    }
                }

            }
        }

        _bulidUILayer.updateWithData(_coreItem, gameDate().languageNow);
    }

    //检查升级钱够不够
    public bool updateitem(int gold)
    {
        if (gameDate().PlayersGold >= gold)
        {
            gameDate().goldChange(-gold);
            _bulidUILayer.updateGold(gameDate().PlayersGold);
            GD.Print("gold  -" + gold);
            return true;
        }
        else
        {
            GD.Print("need more gold");
            return false;
        }
    }

    //更新数据
    public void updateDate(int sequenceIds, int value)
    {
        gameDate().playerBulidPacked[sequenceIds] = value;
        //GD.Print("数据"+gameDate().playerBulidPacked[sequenceIds]);
        List<string> path = new List<string>();
        List<cfg.playerCoreCfg.shipPartDataType> pathtype = new List<cfg.playerCoreCfg.shipPartDataType>();
        List<int> sequenceId = new List<int>();
        List<int> bulidId = new List<int>();
        List<int> updateId = new List<int>();
        List<int> gold = new List<int>();
        List<int> qualitys = new List<int>();

        foreach (var item in gameDate().playerBulidPacked)
        {
            foreach (var itemCfg in cfg.playerCoreCfg.GetAllList())
            {
                if (itemCfg.id == item.Value)
                {
                    path.Add(itemCfg.path);
                    pathtype.Add(itemCfg.type);
                    sequenceId.Add(item.Key);
                    bulidId.Add(itemCfg.id);
                    updateId.Add(itemCfg.updateId);
                    gold.Add(itemCfg.updateGold);
                    qualitys.Add(itemCfg.quality);
                }
            }
        }
        gameDate().playerUsingPaced = _coreItem.setList();
        _scrollContainer.initWithData(path, pathtype, sequenceId, bulidId, updateId, gold, qualitys);
        _scrollContainer.updateData(gameDate().playerUsingPaced);
        _bulidUILayer.updateWithData(_coreItem, gameDate().languageNow);
        foreach (var item  in ItemDic)
        {
            if(item.onlyId == sequenceIds)
            {
                foreach (var itemCfg in cfg.playerCoreCfg.GetAllList())
                {
                    if(value == itemCfg.id)
                    {
                        //GD.Print("替换" + itemCfg.id);
                        var player1 = GD.Load<PackedScene>(itemCfg.path);
                        var player1In = (baseBulidPart)player1.Instance();        
                        item.GetParent().AddChild(player1In);
                        player1In.GlobalPosition = item.GlobalPosition;
                        player1In.InputPickable = item.InputPickable;
                        player1In.onlyId = item.onlyId;
                        player1In.buildId = value;
                        player1In.ConnectPanel = item.ConnectPanel;
                        player1In._playerScenceParents = item._playerScenceParents;
                        player1In.Connect("input_event", this, "_on_core_input_event", new Godot.Collections.Array(player1In));
                        ItemDic.Add(player1In);
                        ItemDic.Remove(item);
                        if(_coreItem == item)
                        {
                            _coreItem = player1In;
                        }

                        item.changePart(player1In);
                        gameDate().playerUsingPaced = _coreItem.setList();
                        _scrollContainer.updateData(gameDate().playerUsingPaced);
                        foreach (Node2D one in _playerBodyNode.GetChildren())
                        {
                            if (one == _coreItem)
                                continue;
                            _scrollContainer.updateData((baseBulidPart)one);
                        }
                        _bulidUILayer.updateWithData(_coreItem, gameDate().languageNow);
                        player1In.checkinterfaceChannel();
                        player1In.shipCheckAssembledAgain(player1In.setList());
                        return;

                    }
                    
                }

            }
            
        }

    }
    //删除部件
    private void deletePart(int id)
    {
        if (id == 0)
            return;
        //GD.Print("id" + id);
        foreach (var item in gameDate().playerBulidPacked)
        {
            if(item.Key == id)
            {
                foreach (var itemCfg in cfg.playerCoreCfg.GetAllList())
                {
                    if (itemCfg.id == item.Value)
                    {
                        gameDate().goldChange(+itemCfg.salePrice);
                    }
                }
            }

        }
        _bulidUILayer.updateGold(gameDate().PlayersGold);
        if (!gameDate().playerBulidPacked.Remove(id))
            //GD.Print("gameDate().playerBulidPacked" +id);
        _scrollContainer.deleteData(id);

    }

    public override void _PhysicsProcess(float delta)
    {
        if (_dragging)
        {

            _draggingItem.GlobalPosition = GetGlobalMousePosition();
            _shipPartDiscLayer.SetGlobalPosition(_draggingItem.GlobalPosition + new Vector2(100, 20));
            //GD.Print(paneldata.GlobalPosition);
        }
    }

    private void _on_startButton_pressed()
    {
        gameDate().playerBulidData = _coreItem.copy();

        mainRoot().CallDeferred("startGame", gameDate().playerBulidData);

    }




    //商店界面
    private void _ShowShopping()
    {
        _shoppingUIPanel.open();
    }

    private List<int> freshShipGold = new List<int>() { 0,10,30,50,75,100,125,150,175,200};

    //刷新商品
    public bool refreshGood()
    {
        
        if(_shoppingUIPanel.refreshTime < freshShipGold.Count)
        {
            if (gameDate().PlayersGold >= freshShipGold[_shoppingUIPanel.refreshTime])
            {
                gameDate().goldChange(-freshShipGold[_shoppingUIPanel.refreshTime]);
                _bulidUILayer.updateGold(gameDate().PlayersGold);
                if(_shoppingUIPanel.refreshTime + 1 < freshShipGold.Count)
                {
                    _shoppingUIPanel.changePirceGold(freshShipGold[_shoppingUIPanel.refreshTime + 1]);
                }else
                {
                    _shoppingUIPanel.changePirceGold(freshShipGold[_shoppingUIPanel.refreshTime]);
                }
                
                return true;
            }
            else
            {
                return false;
            }
        }else
        {
            if (gameDate().PlayersGold >= freshShipGold.Last())
            {
                gameDate().goldChange(-freshShipGold.Last());
                _bulidUILayer.updateGold(gameDate().PlayersGold);
                return true;
            }
            else
            {
                return false;
            }
        }



        
    }

    private int _dicNumber = 1;
    //购买商品
    public bool addGoods(int goodindex)
    {
        if(gameDate().playerBulidPacked.Count >= 30)
        {
            if(gameDate().languageNow == GameData.language.english)
                _visibileNode.initWithWord("Can not buy more");
            if (gameDate().languageNow == GameData.language.chinese)
                _visibileNode.initWithWord("无法购买更多");
            return false;
        }

        foreach (var item in cfg.playerCoreCfg.GetAllList())
        {
            if(item.id == goodindex)
            {
                if (gameDate().PlayersGold >= item.price)
                {
                    gameDate().goldChange(-item.price);
                }                   
                else
                {
                    GD.Print("need more gold");
                    return false;
                }

            }
        }
        _bulidUILayer.updateGold(gameDate().PlayersGold);
        _dicNumber = gameDate().playerBulidPacked.Count;
        gameDate().playerBulidPacked.Add(_dicNumber, goodindex) ;
        _dicNumber++;


        if (!gameDate().unlockedPackedForever.Contains(goodindex))
        {
            if(goodindex%10 == 0)
            {
                if (!gameDate().unlockedPackedForever.Contains(goodindex))
                {
                    gameDate().unlockedPackedForever.Add(goodindex);
                }
                    
            }
        }
            

        List<string> path = new List<string>();
        List<cfg.playerCoreCfg.shipPartDataType> pathtype = new List<cfg.playerCoreCfg.shipPartDataType>();
        List<int> sequenceId = new List<int>();
        List<int> bulidId = new List<int>();
        List<int> updateId = new List<int>();
        List<int> gold = new List<int>();
        List<int> qualitys = new List<int>();

        foreach (var item in gameDate().playerBulidPacked)
        {
            foreach (var itemCfg in cfg.playerCoreCfg.GetAllList())
            {
                if (itemCfg.id == item.Value)
                {
                    path.Add(itemCfg.path);
                    pathtype.Add(itemCfg.type);
                    sequenceId.Add(item.Key);
                    bulidId.Add(itemCfg.id);
                    updateId.Add(itemCfg.updateId);
                    gold.Add(itemCfg.updateGold);
                    qualitys.Add(itemCfg.quality);
                }
            }
        }
        _scrollContainer.initWithData(path, pathtype, sequenceId, bulidId, updateId, gold, qualitys);
        gameDate().playerUsingPaced = _coreItem.setList();
        _scrollContainer.updateData(gameDate().playerUsingPaced);

        return true;
    }

}
