using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Sequence;
namespace RPGEditor
{
    [CustomEditor(typeof(AddCharacter))]
    public class AddCharacterEditor : Editor
    {
        private static int weaponCount;
        private static bool foldOut = true;
        AddCharacter _target;
        void OnEnable()
        {
            _target = target as AddCharacter;
            if (_target.Items == null) _target.Items = new List<int>();
            weaponCount = _target.Items.Count;
        }
        public override void OnInspectorGUI()
        {
            _target.Camp = (EnumCharacterCamp)EditorGUILayout.EnumPopup("阵营", _target.Camp);
            _target.ID = EditorGUILayout.IntField("人物ID", _target.ID);
            _target.Coord = EditorGUILayout.Vector2IntField("坐标", _target.Coord);
            if (_target.Camp == EnumCharacterCamp.Enemy)
            {
                RPGEditorGUI.DynamicArrayView(ref weaponCount, ref _target.Items, "初始武器", "武器", RPGData.WeaponNameList.ToArray(), EnumTables.GetSequentialArray(RPGData.WeaponNameList.Count), 5);
            }
            _target.UseDefaultAttribute = EditorGUILayout.Toggle("使用默认属性", _target.UseDefaultAttribute);
            if (!_target.UseDefaultAttribute)
            {
                CharacterDefEditor.CharacterAttributeInspector(ref _target.Attribute, ref foldOut);
            }
            if (GUI.changed)
            {
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
