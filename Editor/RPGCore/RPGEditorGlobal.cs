using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
namespace RPGEditor
{
    public class RPGEditorGlobal
    {
        public const int MAX_ADDATTIBUTE = 100;
        public const int MAX_ADDGROW = 100;
        public const int MAX_ATTACK_SELECT_RANGE = 10;
        public const int MAX_ATTACK_EFFECT_RANGE = 10;
        public const int MAX_ATTRIBUTE_HP = 10000;
        public const int MAX_ATTRIBUTE_MISC = 1000;
        public const int MAX_ATTRIBUTE_MOVEMENT = 12;
        public const int MAX_ACTION_COST = 100;
        public static void MarkSceneDirty()
        {
            EditorSceneManager.MarkAllScenesDirty();
        }

        public static bool OpenScene(string name)
        {
            if (string.IsNullOrEmpty(name)) return false;
            EditorSceneManager.OpenScene(name, OpenSceneMode.Single);
            return true;
        }
        public static bool SaveCurrentSceneIfUserWantsTo()
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            return true;
        }
        public static string CurrentScene()
        {
            return EditorSceneManager.GetActiveScene().path;
        }

        public static void SaveScene()
        {
            EditorSceneManager.SaveOpenScenes();
        }

        public static void NewScene()
        {
            EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
        }
    }
}