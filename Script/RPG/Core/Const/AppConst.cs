using UnityEngine;

public class AppConst
{
    public const string AppName = "6l";
    public const string AppPrefix = "6l_";
    public const bool AutoWrapMode = true;
    public const bool DebugMode = true;
    public const int GameFrameRate = 30;
    public const string WebUrl = "http://localhost:6688/";

    public static string LuaBasePath
    {
        get
        {
            return (Application.dataPath + "/Resources/Pandora/Lua/uLua/Source/");
        }
    }

    public static string LuaWrapPath
    {
        get
        {
            return (LuaBasePath + "LuaWrap/");
        }
    }
}
