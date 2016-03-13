using UnityEngine;
using System.Collections.Generic;
/// <summary>
/// 单击右键创建该物体,赋值给SLGMap上的_MapTileData;
/// </summary>
[CreateAssetMenu]
public class BattleMapData : ExtendScriptableObject
{
    public TileData[] Data;
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
        Data = new TileData[width * height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                int index = i * height + j;
                Data[index].Tile_x = i;
                Data[index].Tile_y = j;
                Data[index].Tile_height = TerrainHeights[i, j];
                Data[index].Tile_type = type[i, j];
            }
        }
    }
    public TileData GetTileData(int x, int y)
    {
        return Data[x * MapHeight + y];
    }
}
