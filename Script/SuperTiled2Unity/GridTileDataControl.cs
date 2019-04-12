using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class GridTileDataControl : MonoBehaviour
{
    // Start is called before the first frame update
    public Tilemap tileMap;
    public Grid grid;
    public PathShower moveShower;
    public Vector2Int mapSize;
    public ETileType[,] mapTileType;

    public void InitInputEvent()
    {
        GameMode.Instance.InputManager.GetMouseInput = () =>//添加GetMouseInput Callback
        {
            var v = new InputManager.MouseInputState();
            v.active = false;
            RaycastTilePosition(ref v.localPos, ref v.tilePos);

            if (Input.GetMouseButtonUp(0))
            {
                v.active = true;
                v.key = InputManager.EMouseKey.Left;
            }
            if (Input.GetMouseButtonUp(1))
            {
                v.active = true;
                v.key = InputManager.EMouseKey.Right;
            }
            if (Input.GetMouseButtonUp(2))
            {
                v.active = true;
                v.key = InputManager.EMouseKey.Mid;
            }
            return v;
        };
        GameMode.Instance.InputManager.GetNoInput = () =>
        {
            return Input.GetKeyUp(KeyCode.Escape);
        };

        GameMode.Instance.InputManager.GetYesInput = () =>
        {
            return Input.GetKeyUp(KeyCode.Space);
        };
    }
    public void InitTileTypeData()
    {
        var superMapInfo = GetComponentInParent<SuperTiled2Unity.SuperMap>();
        mapSize = new Vector2Int(superMapInfo.m_Width, superMapInfo.m_Height);
        mapTileType = new ETileType[mapSize.x, mapSize.y];

        for (int i = 0; i < mapSize.x; i++)
        {
            for (int j = 0; j < mapSize.y; j++)
            {
                var t = tileMap.GetTile(PositionMath.TilePositionToGridPosition(new Vector2Int(i, j))) as SuperTiled2Unity.SuperTile;
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
        var t = tileMap.GetTile(position) as SuperTiled2Unity.SuperTile;
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
    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonUp(0))
        //{
        //    Vector3Int position = Vector3Int.zero;
        //    Vector2Int tilePos = Vector2Int.zero;
        //    RaycastTilePosition(ref position, ref tilePos);
        //    var t = tileMap.GetTile(position) as SuperTiled2Unity.SuperTile;
        //    if (t == null) return;
        //    var tileType = FeTileInfo.FromString(t.m_Type);
        //    int moveCost = FeTileData.TileInfos[tileType].GetMoveCost(EMoveClassType.Savege);
        //    Debug.Log("id=" + t.m_TileId + " tile=" + tileType.ToString() + "cost=" + moveCost);

        //    PositionMath.SetTileEnemyOccupied(3, 1);
        //    PositionMath.SetTileEnemyOccupied(3, 0);
        //    PositionMath.SetTilePlayerOccupied(6, 0);
        //    PositionMath.InitActionScope(EnumCharacterCamp.Player, EMoveClassType.Savege, 6, tilePos, EnumWeaponType.光明, Vector2Int.one);
        //    moveShower.ShowTiles(PathShower.EPathShowerType.Move, PositionMath.MoveableAreaPoints);
        //    moveShower.ShowTiles(PathShower.EPathShowerType.Damage, PositionMath.AttackAreaPoints, true, false);
        //}

    }
}
