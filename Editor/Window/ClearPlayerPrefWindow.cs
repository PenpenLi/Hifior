using UnityEngine;
using UnityEditor;
using System.Collections;

public class ClearPlayerPrefWindow : EditorWindow
{
    [MenuItem("Tools/Clear Pref", false, 500)]
    public static void OpenEditor()
    {
        ClearPlayerPrefWindow audioEditor = EditorWindow.GetWindow<ClearPlayerPrefWindow>();
        audioEditor.ShowPopup();
    }
    private static string prefKey;
    private static string editorPrefKey;
    public void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        prefKey = EditorGUILayout.TextField("Key", prefKey);
        if (GUILayout.Button("Clear"))
            PlayerPrefs.DeleteKey(prefKey);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        editorPrefKey = EditorGUILayout.TextField("EditorKey", editorPrefKey);
        if (GUILayout.Button("Clear"))
            PlayerPrefs.DeleteKey(editorPrefKey);
        EditorGUILayout.EndHorizontal();
        
        if (GUILayout.Button("Clear All Player Pref"))
            PlayerPrefs.DeleteAll();
    }
}