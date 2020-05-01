using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class OptionConfig : Singleton<OptionConfig>
{
    /// <summary>
    /// 以下保存设置界面的数据，0是默认设置
    /// </summary>
    public static int CONFIG_ANIMATION;//0.切换  1.地图  2. 关
    public static int CONFIG_VIEWPORT; //0.通常  1.固定
    public static int CONFIG_GAMESPEED;//0.普通  1.快速
    public static int CONFIG_INFOSPEED;//0.慢    1.普通  2.快速   3.非常快
    public static int CONFIG_TERRAININFO;//0.开  1.关
    public static int CONFIG_HPBARSHOW;  //0.开  1.关
    public static int CONFIG_AUTOFINISHTURN;//0.开  1.关
    public static int CONFIG_INFOEFFECTSOUND;//0.开  1.关
    public static int CONFIG_LATTICE;  //VALUE代表各自浓度
    public static int CONFIG_BGMSOUNDVOLUME;//VALUE代表音量
    public static int CONFIG_EFFECTSOUNDVOLUME;   //VALUE代表音量
    public static int CONFIG_VOICESOUNDVOLUME;    //VALUE代表音量
    public static string GetConfigFilePath
    {
        get
        {
            return GameRecord.PersistentRootPath() + "//config";
        }
    }
    public static void SaveConfig()
    {
        /* string filePath = GetConfigFilePath;
         FileInfo t = new FileInfo(filePath);
         if (t.Exists) //如果文件存在的话，清空里面的内容先
         {
             FileStream stream = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.Write);
             stream.Seek(0, SeekOrigin.Begin);
             stream.SetLength(0);
             stream.Close();
         }
         StreamWriter sw;

         sw = t.AppendText();//以追加的方式打开
         JsonData data = GeneratorSaveJsonData();
         string sHeader = data.ToJson();
         sw.WriteLine(sHeader);//第一行包含章节和 运输队里的信息， 后面的包含我方人物信息
         sw.Close();*/
    }
    /*
        private static JsonData GeneratorSaveJsonData()
        {
            JsonData data = new JsonData();
            data["ANIMATION"] = OptionConfig.CONFIG_ANIMATION;
            data["VIEWPORT"] = OptionConfig.CONFIG_VIEWPORT; //0.通常  1.固定
            data["GAMESPEED"] = OptionConfig.CONFIG_GAMESPEED;//0.普通  1.快速
            data["INFOSPEED"] = OptionConfig.CONFIG_INFOSPEED;//0.慢    1.普通  2.快速   3.非常快
            data["TERRAININFO"] = OptionConfig.CONFIG_TERRAININFO;//0.开  1.关
            data["HPBARSHOW"] = OptionConfig.CONFIG_HPBARSHOW;  //0.开  1.关
            data["AUTOFINISHTURN"] = OptionConfig.CONFIG_AUTOFINISHTURN;//0.开  1.关
            data["INFOEFFECTSOUND"] = OptionConfig.CONFIG_INFOEFFECTSOUND;//0.开  1.关
            data["LATTICE"] = OptionConfig.CONFIG_LATTICE;  //VALUE代表各自浓度
            data["BGMSOUNDVOLUME"] = OptionConfig.CONFIG_BGMSOUNDVOLUME;//VALUE代表音量
            data["EFFECTSOUNDVOLUME"] = OptionConfig.CONFIG_EFFECTSOUNDVOLUME;   //VALUE代表音量
            data["VOICESOUNDVOLUME"] = OptionConfig.CONFIG_VOICESOUNDVOLUME;    //VALUE代表音量
            return data;
        }

        private static void ReadSaveJsonData(JsonData data)
        {
            OptionConfig.CONFIG_ANIMATION = (int)data["ANIMATION"];
            OptionConfig.CONFIG_VIEWPORT = (int)data["VIEWPORT"]; //0.通常  1.固定
            OptionConfig.CONFIG_GAMESPEED = (int)data["GAMESPEED"];//0.普通  1.快速
            OptionConfig.CONFIG_INFOSPEED = (int)data["INFOSPEED"];//0.慢    1.普通  2.快速   3.非常快
            OptionConfig.CONFIG_TERRAININFO = (int)data["TERRAININFO"];//0.开  1.关
            OptionConfig.CONFIG_HPBARSHOW = (int)data["HPBARSHOW"];  //0.开  1.关
            OptionConfig.CONFIG_AUTOFINISHTURN = (int)data["AUTOFINISHTURN"];//0.开  1.关
            OptionConfig.CONFIG_INFOEFFECTSOUND = (int)data["INFOEFFECTSOUND"];//0.开  1.关
            OptionConfig.CONFIG_LATTICE = (int)data["LATTICE"];  //VALUE代表各自浓度
            OptionConfig.CONFIG_BGMSOUNDVOLUME = (int)data["BGMSOUNDVOLUME"];//VALUE代表音量
            OptionConfig.CONFIG_EFFECTSOUNDVOLUME = (int)data["EFFECTSOUNDVOLUME"];   //VALUE代表音量
            OptionConfig.CONFIG_VOICESOUNDVOLUME = (int)data["VOICESOUNDVOLUME"];    //VALUE代表音量
        }
        */
    public static bool ReadConfig()
    {
        /*string filePath = GetConfigFilePath;
        FileInfo t = new FileInfo(filePath);
        if (t.Exists)
        {
            StreamReader sr = new StreamReader(filePath);
            JsonData data =JsonDecode.LoadToJson(sr);
            ReadSaveJsonData(data);
            sr.Close();
            return true;
        }
        else
        {
            return false;
        }*/
        return true;
    }
}