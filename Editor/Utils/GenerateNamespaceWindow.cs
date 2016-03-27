using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

[System.Serializable]
public struct History
{
    public List<string> HistoryNamespace;
}
public class GenerateNamespaceWindow : EditorWindow
{
    private static Vector2 scrollVector = Vector2.zero;
    private static string NewNamespace = "";
    private static int SelectValue;
    private static bool bUseSelectValue;
    private static History history;
    [MenuItem("Window/Generate Namespace")]
    static void Window()
    {
        Rect wr = new Rect(0, 0, 500, 500);
        GenerateNamespaceWindow window = (GenerateNamespaceWindow)EditorWindow.GetWindowWithRect(typeof(GenerateNamespaceWindow), wr, true, "Generate Namespace Tool");

        window.ShowUtility();
        window.Focus();
    }
    public const string EDITORPREFS_KEY = "GenerateNamespace";
    public void Awake()
    {
        if (EditorPrefs.HasKey(EDITORPREFS_KEY))
        {
            string s = EditorPrefs.GetString(EDITORPREFS_KEY);
            history = JsonUtility.FromJson<History>(s);
        }
        if (history.HistoryNamespace == null)
        {
            history = new History();
            history.HistoryNamespace = new List<string>();
        }
    }
    public static int[] GetSequentialArray(int length)
    {
        int[] x = new int[length];
        for (int i = 0; i < length; i++)
        {
            x[i] = i;
        }
        return x;
    }
    void OnDestroy()
    {
        string json = JsonUtility.ToJson(history);
        EditorPrefs.SetString(EDITORPREFS_KEY, json);
    }
    void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(" History namespace:" + history.HistoryNamespace.Count);
        if (GUILayout.Button("Clear History"))
        {
            EditorPrefs.DeleteKey(EDITORPREFS_KEY);
            history.HistoryNamespace.Clear();
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();

        bUseSelectValue = EditorGUILayout.Toggle("Select From History", bUseSelectValue);
        if (bUseSelectValue)
        {
            if (history.HistoryNamespace.Count > 0)
            {
                SelectValue = EditorGUILayout.IntPopup(SelectValue, history.HistoryNamespace.ToArray(), GetSequentialArray(history.HistoryNamespace.Count));
                NewNamespace = history.HistoryNamespace[SelectValue];
            }
        }

        Object[] SelectedObj = Selection.objects;
        if (SelectedObj.Length > 0)
        {
            scrollVector = EditorGUILayout.BeginScrollView(scrollVector, false, true, GUILayout.Width(320), GUILayout.Height(320));
            foreach (Object o in SelectedObj)
            {
                EditorGUILayout.LabelField("Selected Object:  " + o.name);
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("New Namespace:");
            NewNamespace = EditorGUILayout.TextField("Namespace: ", NewNamespace);
            if (NewNamespace.Length > 0 && char.IsLetter(NewNamespace[0]))
            {
                if (GUILayout.Button("Change namespace", GUILayout.Width(120)))
                {
                    history.HistoryNamespace.Add(NewNamespace);
                    ChangeNamespace(SelectedObj, NewNamespace);
                    Close();
                    AssetDatabase.Refresh();
                }
            }
        }
        else
        {
            EditorGUILayout.LabelField("Select a cs file in the project window");
        }
    }
    void ChangeNamespace(Object[] files, string spaceName)
    {
        foreach (Object o in files)
        {
            string path = AssetDatabase.GetAssetPath(o);
            string extension = Path.GetExtension(path);
            FileInfo info = new FileInfo(path);
            if (extension.Equals(".cs"))
            {
                StreamReader sr = info.OpenText();
                string csContent = sr.ReadToEnd();
                if (csContent.Contains("namespace "))
                {
                    int index = csContent.IndexOf("namespace ");
                    {
                        int enterIndex = csContent.IndexOf("{", index);
                        csContent = csContent.Remove(index, enterIndex - index);
                        csContent = csContent.Insert(index, "namespace " + spaceName);
                    }
                }
                else
                {
                    int index = csContent.IndexOf("public");
                    csContent = csContent.Insert(index, "namespace " + spaceName + " {\n");
                    csContent += "}";
                }
                sr.Close();
                FileStream fs = info.OpenWrite();
                fs.Seek(0, SeekOrigin.Begin);
                fs.SetLength(0);
                fs.Close();

                StreamWriter sw = info.AppendText();
                sw.Write(csContent);
                sw.Close();

            }
        }
    }
}