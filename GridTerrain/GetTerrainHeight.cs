using UnityEngine;
using System.Collections;

public class GetTerrainHeight : MonoBehaviour
{
#if UNITY_EDITOR
    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        for (int x = 0; x < gridWidth; x++)
        {
            Gizmos.DrawLine(new Vector3(x * cellSize, 0, 0), new Vector3(x * cellSize, 0, gridHeight));
        }
        for (int z = 0; z < gridHeight; z++)
        {
            Gizmos.DrawLine(new Vector3(0, 0, z * cellSize), new Vector3(gridWidth, 0, z * cellSize));
        }
    }

    void ComputeHeight()
    {
        _heights = new float[(gridHeight + 1), (gridWidth + 1)];
        RaycastHit hitInfo;
        Vector3 origin;
        int terrainLayerMask = LayerMask.GetMask("Terrain");
        for (int z = 0; z < gridHeight + 1; z++)
        {
            for (int x = 0; x < gridWidth + 1; x++)
            {
                origin = new Vector3(x * cellSize, 200, z * cellSize);
                Physics.Raycast(transform.TransformPoint(origin), Vector3.down, out hitInfo, Mathf.Infinity, terrainLayerMask);

                _heights[x, z] = hitInfo.point.y;
            }
        }
    }
    [ContextMenu("添加子物体")]
    void AddChild()
    {
        ComputeHeight();
        for (int i = 0; i < gridHeight; i++)
        {
            for (int j = 0; j < gridWidth; j++)
            {
                GameObject g = Instantiate<GameObject>(prefab);
                g.layer = 0;
                g.isStatic = true;
                g.name = i + "," + j;
                g.transform.SetParent(transform);
                g.transform.localPosition = new Vector3(i * cellSize, yOffset, j * cellSize);
                PositionGrid pg = g.GetComponent<PositionGrid>();
                pg.Init(cellSize, i, j);
                DestroyImmediate(pg);
            }
        }
    }
#endif
    public GameObject prefab;
    public int gridWidth = 30;
    public int gridHeight = 30;
    public float cellSize = 10f;
    public float yOffset = 2f;
    private float[,] _heights;
    public float[,] Heights
    {
        get { return _heights; }
    }
}
