using UnityEngine;
using System.Collections;

public class BattleMapData : MonoBehaviour
{
#if UNITY_EDITOR

    void ComputeHeight()
    {
        _heights = new float[(TileHeight + 1), (TileWidth + 1)];
        RaycastHit hitInfo;
        Vector3 origin;
        int terrainLayerMask = LayerMask.GetMask("Terrain");
        for (int z = 0; z < TileHeight + 1; z++)
        {
            for (int x = 0; x < TileWidth + 1; x++)
            {
                origin = new Vector3(x * CELLSIZE, 200, z * CELLSIZE);
                Physics.Raycast(transform.TransformPoint(origin), Vector3.down, out hitInfo, Mathf.Infinity, terrainLayerMask);

                _heights[x, z] = hitInfo.point.y;
            }
        }
    }
    [ContextMenu("添加子物体")]
    void AddChild()
    {
        ComputeHeight();
        for (int i = 0; i < TileHeight; i++)
        {
            for (int j = 0; j < TileWidth; j++)
            {
                GameObject g = Instantiate<GameObject>(TilePrefab);
                g.layer = 0;
                g.isStatic = true;
                g.name = i + "," + j;
                g.transform.SetParent(transform);
                g.transform.localPosition = new Vector3(i * CELLSIZE, HEIGHTOFFSET, j * CELLSIZE);
                PositionGrid pg = g.GetComponent<PositionGrid>();
                pg.Init(CELLSIZE, i, j);
                DestroyImmediate(pg);
            }
        }
    }
    public GameObject TilePrefab;
#endif
    public int TileWidth = 30;
    public int TileHeight = 30;
    public const float CELLSIZE = 10f;
    public const float HEIGHTOFFSET = 2f;
    private float[,] _heights;
    public float[,] Heights
    {
        get { return _heights; }
    }
    private int[,] originalTileData;//最初的图块
    private float[,] originalHeightData;//最初的高度数据
    public int[,] TileData;
    public float[,] HeightData;
    private static int[,] OccupiedTile;

    private int[,] MapData = new int[18, 14]{//每一行为列
		{15,15,15,0,0,0,0,0,0,0,0,0,0,0},
        {15,15,15,0,0,0,0,0,0,0,0,0,0,0},
        {15,15,15,0,0,0,0,0,0,0,0,0,0,0},
        {0,15,15,15,15,15,15,15,15,15,15,0,0,0},
        {0,15,15,15,15,15,15,15,15,15,15,0,0,0},
        {0,15,2,15,15,15,15,15,15,15,15,0,0,0},
        {15,15,2,15,15,15,15,15,15,15,15,0,0,0},
        {15,15,15,15,15,15,0,15,15,15,15,0,0,0},
        {15,15,15,15,15,15,0,15,15,15,15,0,0,0},
        {0,0,0,0,0,0,0,15,15,15,15,15,15,15},//10
		{0,0,0,0,0,0,0,15,15,15,15,15,15,15},
        {0,0,0,0,0,0,0,15,15,15,15,15,15,15},
        {0,0,0,0,0,0,0,15,15,15,0,0,0,15},
        {0,15,15,15,15,15,15,15,15,0,0,0,15,15},
        {0,15,15,15,15,15,15,15,15,0,0,0,15,15},
        {15,15,15,15,15,15,15,15,15,15,15,15,15,15},
        {0,0,0,0,15,15,15,15,15,15,15,15,15,15},
        {0,0,0,0,15,15,15,15,15,15,0,0,0,0},
    };

    void Awake()
    {
        InitMapData();
    }
    public void InitMapData()
    {
        originalTileData = MapData;
        originalHeightData = _heights;
        TileWidth = originalTileData.GetLength(0);
        TileHeight = originalTileData.GetLength(1);
        TileData = (int[,])originalTileData.Clone();
        HeightData = (float[,])originalHeightData.Clone();
        OccupiedTile = new int[TileWidth, TileHeight];
    }

    public float GetHeight(int x, int y)
    {
        return Heights[x, y];
    }
    public void setHeight(int x, int y, float height)
    {
        HeightData[x, y] = height;
    }
    public int getTile(int x, int y)
    {
        return TileData[x, y];
    }
    public int GetTileOccupyState(int x, int y)
    {
        return OccupiedTile[x, y];
    }
    public void setTileOccupyState(int x, int y, EnumTileOccupy TileOccupyState)
    {
        OccupiedTile[x, y] = (int)TileOccupyState;
    }
    public int getOriginalTile(int x, int y)
    {
        return originalTileData[x, y];
    }
    public void setTile(int x, int y, int TileType)
    {
        if (x >= TileWidth || y >= TileHeight || x < 0 || y < 0)
            return;
        TileData[x, y] = TileType;
    }
    public void resetTileOccupyState(int x, int y)
    {
        OccupiedTile[x, y] = 0;
    }
}
