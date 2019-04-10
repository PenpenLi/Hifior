using UnityEngine;
using System.Collections.Generic;
/// <summary>
/// 单击右键创建该物体,赋值给SLGMap上的_MapTileData;
/// </summary>
[CreateAssetMenu]
public class BattleMapData : ExtendScriptableObject
{
    public TileDataOld[] Data;
    public int MapWidth;
    public int MapHeight;
    public void InitMapData(int width, int height, float[,] TerrainHeights, int[,] type)
    {
        if (TerrainHeights.GetLength(0) != width || TerrainHeights.GetLength(1) != height || type.GetLength(0) != width || type.GetLength(1) != height)
        {
            Debug.LogError("传入的地图数据错误");
            return;
        }
        MapWidth = width;
        MapHeight = height;
        Data = new TileDataOld[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int index = y * MapWidth + x;
                Data[index] = new TileDataOld(x, y, type[x, y], TerrainHeights[x, y]);

            }
        }
    }
    public TileDataOld GetTileData(int x, int y)
    {
        return Data[y * MapWidth + x];
    }
}
