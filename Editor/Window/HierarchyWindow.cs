using UnityEditor;
using UnityEngine;
[InitializeOnLoad]
public class HierarchyWindow : EditorWindow
{
    private static Texture2D room, manager, store, prefab;
    private static Texture2D gamemode, gameinstance, music, sound, config, portrait, sequence;
    private static Texture2D player, enemy, npc;
    static HierarchyWindow()
    {
        EditorApplication.hierarchyWindowItemOnGUI += DrawThing;
        var path = "Assets/Editor/Res/Hierarchy/";
        room = AssetDatabase.LoadAssetAtPath(path + "Room.tga", typeof(Texture2D)) as Texture2D;
        store = AssetDatabase.LoadAssetAtPath(path + "Store.tga", typeof(Texture2D)) as Texture2D;
        prefab = AssetDatabase.LoadAssetAtPath(path + "Prefab.tga", typeof(Texture2D)) as Texture2D;
        manager = AssetDatabase.LoadAssetAtPath(path + "Manager.tga", typeof(Texture2D)) as Texture2D;
        gamemode = AssetDatabase.LoadAssetAtPath(path + "GameMode.png", typeof(Texture2D)) as Texture2D;
        gameinstance = AssetDatabase.LoadAssetAtPath(path + "GameInstance.png", typeof(Texture2D)) as Texture2D;
        music = AssetDatabase.LoadAssetAtPath(path + "Music.png", typeof(Texture2D)) as Texture2D;
        sound = AssetDatabase.LoadAssetAtPath(path + "Sound.jpg", typeof(Texture2D)) as Texture2D;
        config = AssetDatabase.LoadAssetAtPath(path + "Config.png", typeof(Texture2D)) as Texture2D;
        portrait = AssetDatabase.LoadAssetAtPath(path + "Portrait.png", typeof(Texture2D)) as Texture2D;
        sequence = AssetDatabase.LoadAssetAtPath(path + "Sequence.png", typeof(Texture2D)) as Texture2D;
        player = AssetDatabase.LoadAssetAtPath("Assets/Gizmos/Player.jpg", typeof(Texture2D)) as Texture2D;
        enemy = AssetDatabase.LoadAssetAtPath("Assets/Gizmos/Enemy.png", typeof(Texture2D)) as Texture2D;
        player = AssetDatabase.LoadAssetAtPath("Assets/Gizmos/NPC.jpg", typeof(Texture2D)) as Texture2D;
    }

    private static float width;
    private static void SetDrawPosition(ref Rect area, int i = 0)
    {
        int x = Mathf.Clamp(i, 0, 3);
        area.x = width - (3 - x) * 16;
    }
    static void DrawThing(int id, Rect area)
    {
        var go = EditorUtility.InstanceIDToObject(id) as GameObject;
        if (go == null)
            return;
        if (go.transform.parent == null)
            width = area.width;

        area.width = 16;
        area.height = 16;

        if (go.GetComponent<UGameInstance>())
        {
            SetDrawPosition(ref area, 0);
            GUI.DrawTexture(area, gameinstance);
        }
        if (go.GetComponent<UGameMode>())
        {
            SetDrawPosition(ref area, 0);
            GUI.DrawTexture(area, gamemode);
        }
        if (go.GetComponent<Sequence.Sequence>() != null && room != null)
        {
            SetDrawPosition(ref area, 1);
            GUI.DrawTexture(area, sequence);
        }
        ///SequenceEvent
        var c0 = go.GetComponent<Sequence.SequenceEvent>();
        if (c0 != null)
        {
            SetDrawPosition(ref area, 2);
            if (c0.GetType() == typeof(Sequence.PlayMusic))
            {
                GUI.DrawTexture(area, music);
            }

            if (c0.GetType() == typeof(Sequence.PlaySound))
            {
                GUI.DrawTexture(area, sound);
            }
        }
        var c1 = go.GetComponent<RPGCharacter>();
        if (c1 != null)
        {
            SetDrawPosition(ref area, 2);
            if (c1.GetType() == typeof(RPGPlayer))
            {
                GUI.DrawTexture(area, player);
            }

            if (c1.GetType() == typeof(RPGEnemy))
            {
                GUI.DrawTexture(area, enemy);
            }
        }
    }
}
