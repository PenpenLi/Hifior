using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.Audio;
namespace RPGEditor
{
    public class AudioPreviewWindow : EditorWindow
    {
        private Vector2[] scroll = { Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero };
        public static List<string> bgmNameList = new List<string>();
        public static List<string> bgsNameList = new List<string>();
        public static List<string> meNameList = new List<string>();
        public static List<string> seNameList = new List<string>();
        public static string curSong;
        public const string BGM_FILEPATH = "Assets/Audio/BGM/";
        public const string BGS_FILEPATH = "Assets/Audio/BGS/";
        public const string ME_FILEPATH = "Assets/Audio/ME/";
        public const string SE_FILEPATH = "Assets/Audio/SE/";
        private static readonly string[] MenuItems = { "BGM", "BGS", "ME", "SE" };
        private int selected = 0;
        public static void InitAudioClipList(List<string> clips, string folderPath)
        {
            clips.AddRange(ScriptableObjectUtility.GetFiles(folderPath, "mp3", "ogg", "wav"));
        }
        [MenuItem("RPGEditor/AudioPreview", false, 200)]
        public static void OpenEditor()
        {
            InitAudioClipList(bgmNameList, BGM_FILEPATH);
            InitAudioClipList(bgsNameList, BGS_FILEPATH);
            InitAudioClipList(meNameList, ME_FILEPATH);
            InitAudioClipList(seNameList, SE_FILEPATH);

            AudioPreviewWindow audioEditor = EditorWindow.GetWindow<AudioPreviewWindow>();
            audioEditor.ShowPopup();
        }
        public void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            {
                selected = RPGEditorGUI.MenuHorizontal(selected, MenuItems);
            }
            EditorGUILayout.EndHorizontal();

            scroll[1] = EditorGUILayout.BeginScrollView(scroll[1]);
            {
                switch (selected)
                {
                    case 0: ShowContent(bgmNameList); break;
                    case 1: ShowContent(bgsNameList); break;
                    case 2: ShowContent(meNameList); break;
                    case 3: ShowContent(seNameList); break;
                }
                GUILayout.FlexibleSpace();
            }
            EditorGUILayout.EndScrollView();
        }
        public void ShowContent(List<string> clips)
        {
            EditorGUILayout.BeginHorizontal();
            {
                ShowPlayList(clips);
                ShowPlayControl();
            }
            EditorGUILayout.EndHorizontal();
        }
        public void ShowPlayList(List<string> clips)
        {
            scroll[0] = RPGEditorGUI.BeginScrollView(scroll[0], GUILayout.Width(150));
            {
                for (int i = 0; i < clips.Count; i++)
                {
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(clips[i]);
                    if (GUILayout.Button(fileName, RPGEditorGUI.RichLabelStyle, GUILayout.Width(120)))
                    {
                        curSong = fileName;
                        AudioUtil.StopAllClips();
                        AudioUtil.PlayClip(AssetDatabase.LoadAssetAtPath<AudioClip>(clips[i]));
                    }
                }
            }
            RPGEditorGUI.EndScrollView();
        }
        public void ShowPlayControl()
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("当前播放的歌曲: " + curSong);
            EditorGUILayout.Space();
            if (GUILayout.Button("停止", GUILayout.Width(100)))
            {
                UnityEditor.AudioUtil.StopAllClips();
            }
            EditorGUILayout.EndVertical();
        }
    }
}
