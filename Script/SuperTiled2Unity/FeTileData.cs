using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeTileInfo
{
    public ETileType type;
    public string name;
    public int avoid;
    public int phyDef;
    public int fireDef;
    public int iceDef;
    public int thunderDef;
    public int[] moveCost;
    public FeTileInfo(ETileType _type)
    {
        type = _type;
        name = type.ToString();
        avoid = phyDef = fireDef = iceDef = thunderDef = 0;
        moveCost = new int[(int)EMoveClassType.Count] { 1, 1, 1, 1, 1, 1 };

    }
    public static ETileType FromString(string _typename)
    {
        return (ETileType)System.Enum.Parse(typeof(ETileType), _typename);
    }
    public int GetMoveCost(EMoveClassType t)
    {
        return moveCost[(int)t];
    }
    public FeTileInfo DenyMove()
    {
        for (int i = 0; i < moveCost.Length; i++)
        {
            moveCost[i] = 255;
        }
        return this;
    }
    public FeTileInfo ChangeInfantryMove(int cost)
    {
        moveCost[(int)EMoveClassType.Infantry] = cost;
        return this;
    }
    public FeTileInfo ChangeSavegeMove(int cost)
    {
        moveCost[(int)EMoveClassType.Savege] = cost;
        return this;
    }
    public FeTileInfo ChangeCavalryMove(int cost)
    {
        moveCost[(int)EMoveClassType.Cavalry] = cost;
        return this;
    }
    public FeTileInfo ChangeAircraftMove(int cost)
    {
        moveCost[(int)EMoveClassType.Aircraft] = cost;
        return this;
    }
    public FeTileInfo ChangeAquaticMove(int cost)
    {
        moveCost[(int)EMoveClassType.Aquatic] = cost;
        return this;
    }
    public FeTileInfo ChangeSpecterMove(int cost)
    {
        moveCost[(int)EMoveClassType.Specter] = cost;
        return this;
    }
}
public static class FeTileData
{
    public const int DenyMoveCost = 255;
    public static Dictionary<ETileType, FeTileInfo> TileInfos;
    private static FeTileInfo CreateTileInfo(ETileType type, int avoid = 0, int phyDef = 0, int fireDef = 0, int iceDef = 0, int thunderDef = 0)
    {
        FeTileInfo r = new FeTileInfo(type);
        r.avoid = avoid;
        r.phyDef = phyDef;
        r.fireDef = fireDef;
        r.iceDef = iceDef;
        r.thunderDef = thunderDef;
        return r;
    }
    static FeTileData()
    {
        TileInfos = new Dictionary<ETileType, FeTileInfo>();
        TileInfos.Add(ETileType.Unreachable, CreateTileInfo(ETileType.Unreachable));
        TileInfos.Add(ETileType.Plain, CreateTileInfo(ETileType.Plain));
        TileInfos.Add(ETileType.Floor, CreateTileInfo(ETileType.Floor));
        TileInfos.Add(ETileType.TreasureChest, CreateTileInfo(ETileType.TreasureChest));
        TileInfos.Add(ETileType.Ruined, CreateTileInfo(ETileType.Ruined));
        TileInfos.Add(ETileType.Tunnel, CreateTileInfo(ETileType.Tunnel));
        TileInfos.Add(ETileType.Grass, CreateTileInfo(ETileType.Grass, 20, 1, -5, 2, 2).ChangeInfantryMove(2).ChangeAquaticMove(2).ChangeCavalryMove(3));
        TileInfos.Add(ETileType.WoodenBridge, CreateTileInfo(ETileType.Grass, -20, -1, -5, 0, 0));
        TileInfos.Add(ETileType.Fort, CreateTileInfo(ETileType.Fort, 30, 3, 3, 3, 3));
        TileInfos.Add(ETileType.Hill, CreateTileInfo(ETileType.Hill, 30, 2, -2, 2, -2).ChangeInfantryMove(3).ChangeCavalryMove(4).ChangeAquaticMove(DenyMoveCost));
        TileInfos.Add(ETileType.HighHill, CreateTileInfo(ETileType.HighHill, 40, 4, -4, 4, -4).ChangeInfantryMove(DenyMoveCost).ChangeAquaticMove(DenyMoveCost).ChangeCavalryMove(DenyMoveCost));
        TileInfos.Add(ETileType.Ocean, CreateTileInfo(ETileType.Ocean, -20, 0, 5, -5, -3).ChangeInfantryMove(DenyMoveCost).ChangeCavalryMove(DenyMoveCost).ChangeSavegeMove(2));
        TileInfos.Add(ETileType.Wall, CreateTileInfo(ETileType.Wall).DenyMove());
        TileInfos.Add(ETileType.Door, CreateTileInfo(ETileType.Door).DenyMove());
        TileInfos.Add(ETileType.Gate, CreateTileInfo(ETileType.Gate, 30, 3, 3, 3, 3));
        TileInfos.Add(ETileType.Throne, CreateTileInfo(ETileType.Throne, 30, 3, 3, 3, 3));
        TileInfos.Add(ETileType.Village, CreateTileInfo(ETileType.Village, 20, 1,1,1,1));
        TileInfos.Add(ETileType.House, CreateTileInfo(ETileType.House, 10, 1, 1, 1, 1));
        TileInfos.Add(ETileType.WeaponShop, CreateTileInfo(ETileType.WeaponShop, 10, 1, 1, 1, 1));
        TileInfos.Add(ETileType.ItemShop, CreateTileInfo(ETileType.ItemShop, 10, 1, 1, 1, 1));
        TileInfos.Add(ETileType.SecretShop, CreateTileInfo(ETileType.SecretShop, 10, 1, 1, 1, 1));
    }
}
