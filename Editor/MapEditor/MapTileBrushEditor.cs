using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
namespace RPGEditor
{
    [CustomEditor(typeof(MapTileBrush))]
    public class MapTileBrushEditor : Editor
    {
        private static int CurrentX;
        private static int CurrentY;
        public int width;
        public int height;
        MapTileBrush _target;
        
        void OnEnable()
        {
            _target = (MapTileBrush)target;
            MapTilePropertyWindow.LoadTileTexture();
            width = _target.GetComponent<GetTerrainHeight>().gridWidth;
            height = _target.GetComponent<GetTerrainHeight>().gridHeight;
        }
        public override void OnInspectorGUI()
        {
            _target.EditorMode = EditorGUILayout.Toggle(_target.EditorMode);
        }
        void OnSceneGUI()
        {
            if (_target.EditorMode)
            {
                Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                RaycastHit rayHit;
                if (Event.current.control)
                {
                    if (Physics.Raycast(ray, out rayHit))
                    {
                        Point2D p = SLGMap.Vector3ToPoint2D(rayHit.point);

                        int x = Mathf.Clamp(p.x, 0, width - 1);
                        int y = Mathf.Clamp(p.y, 0, height - 1);
                        Debug.Log("x:" + x + " y:" + y);
                    }
                }
            }
        }

    }
}