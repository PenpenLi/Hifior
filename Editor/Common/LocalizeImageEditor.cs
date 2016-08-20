using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using UnityEngine.UI;
[CustomEditor(typeof(LocalizeImage), true)]
public class LocalizeImageEditor : Editor
{
    public string LocalizeImagePath
    {
        get
        {
            return "Assets/Resources/LocalizedImage";
        }
    }
    private static string[] languageType;
    private LocalizeImage t;
    private Sprite selectSprite;
    private int currentSelect;
    private List<Sprite> languageSprites;
    private int lastSelect;
    Image image;
    void OnEnable()
    {
        lastSelect = -1;
        t = target as LocalizeImage;
        image = t.GetComponent<UnityEngine.UI.Image>();


        if (languageType != null && languageType.Length > 0)
        {
            CheckInitSelect();
            return;
        }
        string[] lanDir = Directory.GetDirectories(LocalizeImagePath);
        languageType = new string[lanDir.Length];

        for (int i = 0; i < lanDir.Length; i++)
        {
            languageType[i] = Path.GetFileNameWithoutExtension(lanDir[i]);
        }

        CheckInitSelect();


        if (lanDir.Length == 0)
        {
            Debug.Log(LocalizeImagePath + "路径不存在多语言图片文件夹");
            return;
        }

        //if (!filesCount.All(sarray => { return sarray == filesCount[0]; }))
        //{
        //    EditorUtility.DisplayDialog("错误", "不同语言的文件夹数目不相同\n ", ("/"), "OK");
        //}
    }
    private void CheckInitSelect()
    {
        if (image.sprite != null)
        {
            string spritePath = AssetDatabase.GetAssetPath(image.sprite);
            for (int i = 0; i < languageType.Length; i++)
                if (spritePath.Contains(LocalizeImagePath + "/" + languageType[i]))
                {
                    currentSelect = i;
                    break;
                }
        }
    }
    public override void OnInspectorGUI()
    {
        selectSprite = image.sprite;
        string assetPath = AssetDatabase.GetAssetPath(selectSprite);
        string assetName = Path.GetFileNameWithoutExtension(assetPath);

        currentSelect = EditorGUILayout.IntPopup("语言", currentSelect, languageType, EnumTables.GetSequentialArray(languageType.Length));

        if (assetPath.Contains(LocalizeImagePath))
        {
            t.key = assetName;
            EditorGUILayout.LabelField("Key", t.key);
        }
        else
        {
            EditorGUILayout.HelpBox("请从" + LocalizeImagePath + "文件夹里选择相应的文件", MessageType.Error);
        }

        if (lastSelect == -1)
        {
            lastSelect = currentSelect;
        }
        if (lastSelect != currentSelect)
        {
            lastSelect = currentSelect;
            //selectSprite = AssetDatabase.LoadAssetAtPath<Sprite>(LocalizeImagePath +"/"+ languageType[currentSelect] + "/" + assetName);
            selectSprite = Resources.Load<Sprite>("LocalizedImage/" + languageType[currentSelect] + "/" + assetName);
        }
        image.sprite = EditorGUILayout.ObjectField("图片", selectSprite, typeof(Sprite), false) as Sprite;
        EditorUtility.SetDirty(image);
    }
}
