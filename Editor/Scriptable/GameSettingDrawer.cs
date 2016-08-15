using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

namespace RPGEditor
{
    public class GameSettingDrawer : EditorWindowDrawer
    {

        public static MiscCoefficientSetting MiscSetting;

        public void OnLoad()
        {
            string absolutePath = DataBaseConst.DataBase_GameSetting_File;
            if (!ScriptableObjectUtility.FileExists(absolutePath))
            {
                MiscSetting = ScriptableObjectUtility.CreateAsset<MiscCoefficientSetting>(
                   System.IO.Path.GetDirectoryName(absolutePath),
                    System.IO.Path.GetFileNameWithoutExtension(absolutePath),
                    true
                );
            }
            MiscSetting = AssetDatabase.LoadAssetAtPath(absolutePath, typeof(MiscCoefficientSetting)) as MiscCoefficientSetting;

        }

        public void OnGUI()
        {
            DrawDefaultEditor.DrawInspector<MiscCoefficientSetting>(MiscSetting);
        }
    }
}
