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
            Target.TypeOfTile= EditorGUILayout.IntPopup("图块类型", Target.TypeOfTile, d.ToArray(), EnumTables.GetSequentialArray(d.Count));
            Target.gameObject.GetComponent<MeshRenderer>().material.SetTexture("_Texture", MapTilePropertyWindow.GetTexture(Target.TypeOfTile));
            Target.transform.GetComponentInParent<SLGMap>().MapTileData.Data[Target.transform.GetSiblingIndex()].SetType(Target.TypeOfTile);
        }
        public void OnEnable()
        {
            Target = target as TileType;
            Target.TypeOfTile = Target.transform.GetComponentInParent<SLGMap>().MapTileData.Data[Target.transform.GetSiblingIndex()].GetTileType();
        }
    }
}
