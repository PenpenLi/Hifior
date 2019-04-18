using UnityEngine;
using UnityEditor;
namespace RPGEditor
{
    [CustomEditor(typeof(EnemyDef))]
    public class EnemyDefEditor : Editor
    {
        EnemyDef enemy;
        public void OnEnable()
        {
            enemy = target as EnemyDef;
        }

        public const string DIRECTORY_PATH = DataBaseConst.DataBase_Enemy_Folder;
        [MenuItem("RPGEditor/Create Character/Enemy", false, 1)]
        public static EnemyDef CreateProps()
        {
            int count = ScriptableObjectUtility.GetFoldFileCount(DIRECTORY_PATH);

            EnemyDef enemy = ScriptableObjectUtility.CreateAsset<EnemyDef>(
                count.ToString(),
                DIRECTORY_PATH,
                true
            );
            enemy.CommonProperty.ID = count;
            return enemy;
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            //enemy.ActionAI = (EnumEnemyActionAI)EditorGUILayout.EnumPopup("行动策略", enemy.ActionAI);
            //enemy.CureSelf = (EnumEnemyCureSelfCondition)EditorGUILayout.EnumPopup("治疗自身", enemy.CureSelf);
        }
    }


    public class EnemyEditorProp : EditorProp<EnemyDef>
    {
        public override string AssetFolder
        {
            get
            {
                return DataBaseConst.DataBase_Enemy_Folder;
            }
        }

        public override string ListName(int index)
        {
            string Name = scriptableObjects[index].CommonProperty.Name;
            if (string.IsNullOrEmpty(Name.Trim()))
                return base.NO_NAME;
            return Name;
        }

        public override void OnGUI(EnemyDef Data)
        {
        }
    }
}