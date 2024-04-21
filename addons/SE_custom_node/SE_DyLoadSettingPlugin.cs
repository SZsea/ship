#if TOOLS
using Godot;
using System;


[Tool]
public class SE_DyLoadSettingPlugin : EditorPlugin
{
    Control dock;
    public override void _EnterTree()
    {
        if (Engine.EditorHint)
        {
            var script = GD.Load<Script>("addons/SE_custom_node/SE_DyLoadSetting_Button.cs");
            var texture = GD.Load<Texture>("addons/SE_custom_node/customButton.png");
            AddCustomType("SE_DyLoadSetting_Button", "Button", script, texture);

            dock = (Control)GD.Load<PackedScene>("addons/SE_custom_node/SE_DyLoadSettingInter.tscn").Instance();
            AddControlToDock(DockSlot.LeftUl, dock);

        }
        // Initialization of the plugin goes here.

    }

    public override void _ExitTree()
    {
        if (Engine.EditorHint)
        {
            RemoveCustomType("SE_DyLoadSetting_Button");

            // Clean-up of the plugin goes here.
            // Remove the dock.
            RemoveControlFromDocks(dock);
            // Erase the control from the memory.
            dock.Free();

        }// Clean-up of the plugin goes here.


            
    }
}



#endif
