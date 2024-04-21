using Godot;
using System;
using System.Collections.Generic;

[Tool]
public class SE_DyLoadSetting_Button : Button
{
    public override void _EnterTree()
    {

        Connect("pressed", this, "clicked");

    }

    public void clicked()
    {
        //

        GD.Print(GetTree().EditedSceneRoot.Filename);

        string fullpath = GetTree().EditedSceneRoot.Filename;
        string filename = GlobalConstant.getFileName(fullpath,1);

        //GD.Print(" ++++++++++" +IsInstanceValid(GetTree().EditedSceneRoot));

        //StageScence scence = (GetTree().EditedSceneRoot) as StageScence;
        Godot.Collections.Dictionary<string, object> saveData = new Godot.Collections.Dictionary<string, object>();
        saveData.Add(filename, exportData());
        commonData.SaveData(filename, saveData);
        GD.Print("saveData!  map:" + GetTree().EditedSceneRoot.GetChild(0));
    }









    public Godot.Collections.Dictionary<string, object> exportData()
    {
        GD.Print("saveData!  exportData()" + this);
        Godot.Collections.Dictionary<string, object> saveData = new Godot.Collections.Dictionary<string, object>();

        for (int i = 0; i < GetTree().EditedSceneRoot.GetChildCount(); i++)
        {
            if (GetTree().EditedSceneRoot.GetChild(i).GetType() == typeof(ColorRect))
            {
                
                ColorRect colorRect = (ColorRect)GetTree().EditedSceneRoot.GetChild(i);

                saveData.Add(i.ToString(), commonData.exportData(colorRect));
            }
            if (GetTree().EditedSceneRoot.GetChild(i).GetType() == typeof(AnimatedSprite))
            {
                
                AnimatedSprite animatedSprite = (AnimatedSprite)GetTree().EditedSceneRoot.GetChild(i);

                saveData.Add(i.ToString(), commonData.exportData(animatedSprite));
            }
            if (GetTree().EditedSceneRoot.GetChild(i).GetType() == typeof(TextureRect))
            {

                TextureRect textureRect = (TextureRect)GetTree().EditedSceneRoot.GetChild(i);

                saveData.Add(i.ToString(), commonData.exportData(textureRect));
            }
        }
        return saveData;

    }
}
