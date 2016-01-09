using UnityEngine;
using UnityEditor;
namespace RPGEditor
{
    [CustomEditor(typeof(EnemyDef))]
    public class EnemyDefEditor : CharacterDefEditor
    {
        EnemyDef enemy;
        public void OnEnable()
        {
            enemy = target as EnemyDef;
            base.InitTarget(enemy);
        }

        public const string DIRECTORY_PATH = "Assets/RPG Data/Character/Enemy";
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
            enemy.ActionAI = (EnumEnemyActionAI)EditorGUILayout.EnumPopup("行动策略", enemy.ActionAI);
            enemy.AttackInRange = EditorGUILayout.Toggle("攻击范围内攻击", enemy.AttackInRange);
            enemy.CureSelf =(EnumEnemyCureSelfCondition) EditorGUILayout.EnumPopup("治疗自身", enemy.CureSelf);
        }
    }
}