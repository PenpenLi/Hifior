using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace RPGEditor
{
    [CustomEditor(typeof(TileType))]
    [CanEditMultipleObjects]
    public class TileTypeEditor : Editor
    {
        public TileType Target;
        public override void OnInspectorGUI()
        {
            List<string> d = MapTilePropertyWindow.GetNames();
            Target.TypeOfTile = EditorGUILayout.IntPopup("图块类型", Target.TypeOfTile, d.ToArray(), EnumTables.GetSequentialArray(d.Count));
            Target.gameObject.GetComponent<MeshRenderer>().material.SetTexture("_Texture", MapTilePropertyWindow.GetTexture(Target.TypeOfTile));
            Target.transform.GetComponentInParent<SLGMap>().MapTileData.Data[Target.transform.GetSiblingIndex()].Type = (Target.TypeOfTile);
            TileAttribute ta = MapTilePropertyWindow.TileDataDef[Target.TypeOfTile];
            EditorGUILayout.LabelField("战场背景ID", ta.BattleBackgroundID.ToString());
            EditorGUILayout.LabelField("回避", ta.Avoid.ToString());
            EditorGUILayout.LabelField("物理防御", ta.PhysicalDefense.ToString());
            EditorGUILayout.LabelField("魔法防御", ta.MagicalDefense.ToString());
            EditorGUILayout.LabelField("生命值恢复比例", ta.Recover.ToString());

            for (int i = 0; i < MapTilePropertyWindow.TileSeries.Length; i++)
            {
                EditorGUILayout.LabelField(MapTilePropertyWindow.TileSeries[i], ta.MovementConsume[i].ToString());
            }

            EditorGUILayout.Toggle("是否可以向左通行", ta.PassLeft);
            EditorGUILayout.Toggle("是否可以向右通行", ta.PassRight);
            EditorGUILayout.Toggle("是否可以向上通行", ta.PassUp);
            EditorGUILayout.Toggle("是否可以向下通行", ta.PassDown);
        }
        public void OnEnable()
        {
            Target = target as TileType;
            Target.TypeOfTile = Target.transform.GetComponentInParent<SLGMap>().MapTileData.Data[Target.transform.GetSiblingIndex()].Type;
        }
    }
}
