using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using System;

public class GridTileManager : ManagerBase
{
    public const string TILE_LAYER_PREFIX = "Tile Layer";
    public const string MAP_CHANGE_PREFIX = "Map Change";
    // Start is called before the first frame update
    public Transform mapRoot;
    public Tilemap tileLayer;
    public List<Tilemap> mapChange;
    public Grid grid;
    public Vector2Int mapSize;
    public ETileType[,] mapTileType;
    public void LoadNewMap(int id)
    {
        GameObject mapPrefab = Resources.Load<GameObject>("fe/fe" + id);
        mapPrefab = GameObject.Instantiate(mapPrefab);
        mapPrefab.transform.localScale = new Vector3(10, 10, 1);
        InitScript(mapPrefab.transform);
        InitTileTypeData();
    }
    public void InitScript(Transform t)
    {
        mapRoot = t;
        mapChange = new List<Tilemap>();
        grid = t.GetComponent<Grid>();
        foreach (Transform v in mapRoot)
        {
            var tilemap = v.GetComponent<Tilemap>();
            if (tilemap == null) { Debug.LogError(v.name + " tile map is null"); continue; }
            if (v.name.StartsWith(TILE_LAYER_PREFIX))
            {
                tileLayer = tilemap;
            }
            if (v.name.StartsWith(MAP_CHANGE_PREFIX))
            {
                mapChange.Add(tilemap);
            }
        }
    }
    public void InitMouseInputEvent()
    {
        Vector2Int oldPos = Vector2Int.one;
        gameMode.InputManager.GetMouseInput = () =>//添加GetMouseInput Callback
        {
            var v = new InputManager.MouseInputState();
            v.active = false;
            v.oldTilePos = oldPos;
            RaycastTilePosition(ref v.localPos, ref v.tilePos);
            oldPos = v.tilePos;
            if (Input.GetMouseButtonUp(0))
            {
                v.active = true;
                v.clickedUI = EventSystem.current.IsPointerOverGameObject();
                v.key = InputManager.EMouseKey.Left;
            }
            if (Input.GetMouseButtonUp(1))
            {
                v.active = true;
                v.clickedUI = EventSystem.current.IsPointerOverGameObject();
                v.key = InputManager.EMouseKey.Right;
            }
            if (Input.GetMouseButtonUp(2))
            {
                v.active = true;
                v.clickedUI = EventSystem.current.IsPointerOverGameObject();
                v.key = InputManager.EMouseKey.Mid;
            }
            return v;
        };
    }
    public void InitTileTypeData()
    {
        var superMapInfo = mapRoot.GetComponent<SuperTiled2Unity.SuperMap>();
        mapSize = new Vector2Int(superMapInfo.m_Width, superMapInfo.m_Height);
        mapTileType = new ETileType[mapSize.x, mapSize.y];

        for (int i = 0; i < mapSize.x; i++)
        {
            for (int j = 0; j < mapSize.y; j++)
            {
                var t = tileLayer.GetTile(PositionMath.TilePositionToGridPosition(new Vector2Int(i, j))) as SuperTiled2Unity.SuperTile;
                mapTileType[i, j] = FeTileInfo.FromString(t.m_Type);
            }
        }
        PositionMath.SetTileTypeData(mapTileType);
    }
    void RaycastTilePosition(ref Vector3Int worldPos, ref Vector2Int tilePos)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // get the collision point of the ray with the z = 0 plane
        Vector3 worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);
        worldPos = grid.WorldToCell(worldPoint);
        tilePos = PositionMath.GridPositionToTilePosition(worldPos);
    }
    public ETileType GetTileType(Vector3Int position)
    {
        var t = tileLayer.GetTile(position) as SuperTiled2Unity.SuperTile;
        if (t == null) return ETileType.Unreachable;
        var tileType = FeTileInfo.FromString(t.m_Type);
        return tileType;
    }
    public ETileType GetTileType(Vector2Int tilePos)
    {
        return GetTileType(PositionMath.TilePositionToGridPosition(tilePos));
    }
    public FeTileInfo GetTileInfo(Vector3Int position)
    {
        var tileType = GetTileType(position);
        return FeTileData.TileInfos[tileType];
    }
    public FeTileInfo GetTileInfo(Vector2Int tilePos)
    {
        var tileType = GetTileType(tilePos);
        return FeTileData.TileInfos[tileType];
    }

    public void OpenDoor(Vector2Int tilePos)
    {
        Debug.Log(tilePos + " 处开门"+ " 修改某处Tile");
    }
}
