using Godot;
using System;
using System.Collections.Generic;
using System.Drawing;


public class commonData
{


    //地图存储辅助类


    private const string SaveData_Ty_typed = "SaveData_Ty_typed";
    private const string SaveData_prop_Size_X = "prop_Size_x";
    private const string SaveData_prop_Size_Y = "prop_Size_y";
    private const string SaveData_prop_Position_X = "prop_Position_x";
    private const string SaveData_prop_Position_Y = "prop_Position_y";
    private const string SaveData_prop_Scale_X = "prop_Scale_X";
    private const string SaveData_prop_Scale_Y = "prop_Scale_Y";
    private const string SaveData_prop_Color = "prop_Color"; 
    private const string SaveData_prop_ResourcePath = "prop_ResourcePath";
    // private const string SaveData_prop_Animation = "prop_Animation";

    private static int commonDataType_ColorRect = 0;
    private static int commonDataType_AnimatedSprite = 1;
    private static int commonDataType_TextureRect = 2;








    public static Godot.Collections.Dictionary<string, object> exportData<T>(T data)
    {
        Godot.Collections.Dictionary<string, object> saveData = new Godot.Collections.Dictionary<string, object>();
        if (data.GetType() == typeof(ColorRect))
        {
            saveData.Add(SaveData_Ty_typed, commonDataType_ColorRect);
            saveData.Add(SaveData_prop_Size_X, (data as ColorRect).RectSize.x);
            saveData.Add(SaveData_prop_Size_Y, (data as ColorRect).RectSize.y);
            saveData.Add(SaveData_prop_Position_X, (data as ColorRect).RectPosition.x);
            saveData.Add(SaveData_prop_Position_Y, (data as ColorRect).RectPosition.y);
            saveData.Add(SaveData_prop_Color, (data as ColorRect).Color.ToHtml());
        }
        if (data.GetType() == typeof(AnimatedSprite))
        {
            saveData.Add(SaveData_Ty_typed, commonDataType_AnimatedSprite);
            saveData.Add(SaveData_prop_Position_X, (data as AnimatedSprite).Position.x);
            saveData.Add(SaveData_prop_Position_Y, (data as AnimatedSprite).Position.y);
            saveData.Add(SaveData_prop_Scale_X, (data as AnimatedSprite).Scale.x);
            saveData.Add(SaveData_prop_Scale_Y, (data as AnimatedSprite).Scale.y);
            saveData.Add(SaveData_prop_ResourcePath, (data as AnimatedSprite).Frames.ResourcePath);
                       
        }
        if (data.GetType() == typeof(TextureRect))
        {
            saveData.Add(SaveData_Ty_typed, commonDataType_TextureRect);
            saveData.Add(SaveData_prop_Position_X, (data as TextureRect).RectPosition.x);
            saveData.Add(SaveData_prop_Position_Y, (data as TextureRect).RectPosition.y);
            saveData.Add(SaveData_prop_Size_X, (data as TextureRect).RectSize.x);
            saveData.Add(SaveData_prop_Size_Y, (data as TextureRect).RectSize.y);
            saveData.Add(SaveData_prop_ResourcePath, (data as TextureRect).Texture.ResourcePath);

        }

        return saveData;
    }



    public static void SaveData(string fileName, Godot.Collections.Dictionary<string, object> data)
    {


        var saveGame = new File();
        saveGame.Open(GlobalConstant.mapData_loadPath + fileName + GlobalConstant.mapData_Extension, File.ModeFlags.Write);
        // Store the save dictionary as a new line in the save file.
        saveGame.StoreLine(JSON.Print(data));
        saveGame.Close();
    }







    //数据类
    public int id;
    public Vector2 prop_Size;
    public Vector2 prop_Position;
    public Vector2 prop_Scale;
    public Godot.Color prop_Color;
    public int prop_Typed;
    public string prop_ResourcePath;
    public Node objectRe;


    public static List<commonData> commonMapDatas = new List<commonData>();





    public static void LoadData(string fileName, Node2D rootNode)
    {
        if (commonMapDatas.Count > 0)
            commonMapDatas.Clear();


        var saveGame = new File();
        if (!saveGame.FileExists(GlobalConstant.mapData_loadPath + fileName + GlobalConstant.mapData_Extension))
            return; // Error! We don't have a save to load.

        // Load the file line by line and process that dictionary to restore the object
        // it represents.
        saveGame.Open(GlobalConstant.mapData_loadPath + fileName + GlobalConstant.mapData_Extension, File.ModeFlags.Read);
        if (saveGame.GetLen() < 3)
            return;
        while (saveGame.GetPosition() < saveGame.GetLen())
        {
            // Get the saved dictionary from the next line in the save file
            var allData = new Godot.Collections.Dictionary<string, object>((Godot.Collections.Dictionary)JSON.Parse(saveGame.GetLine()).Result);

            foreach (KeyValuePair<string, object> entry in allData)
            {
                string key = entry.Key.ToString();
                if(key == fileName)
                {
                    GD.Print("  LoadSucess   " + fileName);
                    foreach (KeyValuePair<string, object> entryData in new Godot.Collections.Dictionary<string, object>((Godot.Collections.Dictionary)entry.Value))
                    {
                        int entryDataKey = entryData.Key.ToInt();

                        var propData = new Godot.Collections.Dictionary<string, object>((Godot.Collections.Dictionary)entryData.Value);
                        
                        //int propDataType;
                        propData.TryGetValue(SaveData_Ty_typed, out object propDataType);
                        if(propDataType.ToString().ToInt() == commonDataType_ColorRect)
                        {

                            commonData data = new commonData();
                            data.prop_Typed = commonDataType_ColorRect;
                            data.prop_Size = new Vector2(propData[SaveData_prop_Size_X].ToString().ToFloat(), propData[SaveData_prop_Size_Y].ToString().ToFloat());
                            data.prop_Position = new Vector2(propData[SaveData_prop_Position_X].ToString().ToFloat(), propData[SaveData_prop_Position_Y].ToString().ToFloat());
                            data.prop_Color = new Godot.Color(propData[SaveData_prop_Color].ToString());
                            data.id = entryDataKey;                            
                            commonData.commonMapDatas.Add(data);


                        }
                        if (propDataType.ToString().ToInt() == commonDataType_AnimatedSprite)
                        {                           

                            commonData data = new commonData();
                            SpriteFrames spriteFrames = GD.Load<SpriteFrames>(propData[SaveData_prop_ResourcePath].ToString());
                            data.prop_Size = spriteFrames.GetFrame(spriteFrames.GetAnimationNames()[0], 0).GetSize();
                            data.prop_Typed = commonDataType_AnimatedSprite;
                            data.prop_Scale = new Vector2(propData[SaveData_prop_Scale_X].ToString().ToFloat(), propData[SaveData_prop_Scale_Y].ToString().ToFloat());
                            data.prop_Position = new Vector2(propData[SaveData_prop_Position_X].ToString().ToFloat(), propData[SaveData_prop_Position_Y].ToString().ToFloat());
                            data.prop_ResourcePath = propData[SaveData_prop_ResourcePath].ToString();
                            data.id = entryDataKey;
                            
                            commonData.commonMapDatas.Add(data);

                            //animatedSprite.Animation = 

                        }
                        if (propDataType.ToString().ToInt() == commonDataType_TextureRect)
                        {

                            commonData data = new commonData();
                            data.prop_Size = new Vector2(propData[SaveData_prop_Size_X].ToString().ToFloat(), propData[SaveData_prop_Size_Y].ToString().ToFloat());
                            data.prop_Typed = commonDataType_TextureRect;
                            data.prop_Position = new Vector2(propData[SaveData_prop_Position_X].ToString().ToFloat(), propData[SaveData_prop_Position_Y].ToString().ToFloat());
                            data.prop_ResourcePath = propData[SaveData_prop_ResourcePath].ToString();
                            data.id = entryDataKey;

                            commonData.commonMapDatas.Add(data);

                            //animatedSprite.Animation = 

                        }

                    }

                }
                else
                {
                    GD.Print("  LoadFailue   " + fileName);
                }
                   
            }

        }

        saveGame.Close();
    }





    public static void LoadData(commonData data, Node2D rootNode)
    {
        if (data.prop_Typed == commonDataType_ColorRect)
        {
            ColorRect colorRect = new ColorRect();
            rootNode.AddChild(colorRect);
            rootNode.MoveChild(colorRect, 0);
            colorRect.RectSize = data.prop_Size;
            colorRect.RectPosition = data.prop_Position;
            colorRect.Color = data.prop_Color;
            data.objectRe = colorRect;
        }
        if (data.prop_Typed == commonDataType_AnimatedSprite)
        {
            AnimatedSprite animatedSprite = new AnimatedSprite();
            rootNode.AddChild(animatedSprite);
            animatedSprite.Scale = data.prop_Scale;
            animatedSprite.Position = data.prop_Position;
            SpriteFrames spriteFrames = GD.Load<SpriteFrames>(data.prop_ResourcePath);
            animatedSprite.Frames = spriteFrames;
            animatedSprite.Animation = spriteFrames.GetAnimationNames()[0];
            animatedSprite.Playing = true;
            data.objectRe = animatedSprite;
        }
        if (data.prop_Typed == commonDataType_TextureRect)
        {
            TextureRect textureRect = new TextureRect();
            rootNode.AddChild(textureRect);
            textureRect.RectPosition = data.prop_Position;
            textureRect.RectSize = data.prop_Size;
            Texture texture = GD.Load<Texture>(data.prop_ResourcePath);
            textureRect.Texture = texture;
            textureRect.StretchMode = TextureRect.StretchModeEnum.Tile;

            data.objectRe = textureRect;
        }

    }






}
