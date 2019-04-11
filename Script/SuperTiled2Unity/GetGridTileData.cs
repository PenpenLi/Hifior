using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class GetGridTileData : MonoBehaviour
{
    // Start is called before the first frame update
    public Tilemap tileMap;
    public Grid grid;
    public PathShower moveShower;
    public Vector2Int mapSize;
    public ETileType[,] mapTileType;
    void Start()
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
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // get the collision point of the ray with the z = 0 plane
            Vector3 worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);
            Vector3Int position = grid.WorldToCell(worldPoint);
            Vector2Int tilePos = PositionMath.GridPositionToTilePosition(position);


            var t = tileMap.GetTile(position) as SuperTiled2Unity.SuperTile;
            var tileType = FeTileInfo.FromString(t.m_Type);
            int moveCost = FeTileData.TileInfos[tileType].GetMoveCost(EMoveClassType.Savege);
            Debug.Log("id=" + t.m_TileId + " tile=" + tileType.ToString() + "cost=" + moveCost);

            PositionMath.SetTileTypeData(mapTileType);
            PositionMath.SetTileEnemyOccupied(3, 1);
            PositionMath.SetTileEnemyOccupied(3, 0);
            PositionMath.SetTilePlayerOccupied(6, 0);
            PositionMath.InitActionScope(EnumCharacterCamp.Player, EMoveClassType.Savege, 6, tilePos, EnumWeaponType.光明, Vector2Int.one);
            moveShower.ShowTiles(PathShower.EPathShowerType.Move, PositionMath.MoveableAreaPoints);
            moveShower.ShowTiles(PathShower.EPathShowerType.Damage, PositionMath.AttackAreaPoints, true, false);
        }

    }
}
