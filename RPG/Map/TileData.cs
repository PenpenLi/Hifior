[System.Serializable]
public struct TileData
{
    public enum EnumOccupyState
    {
        None,
        Player,
        Enemy
    }
    public int Tile_x;
    public int Tile_y;
    /// <summary>
    /// 图块类型
    /// </summary>
    public int Tile_type;
    /// <summary>
    /// 前一个图块类型
    /// </summary>
    private int Tile_type_prev;
    /// <summary>
    /// 当前块的高度
    /// </summary>
    public float Tile_height;
    private EnumOccupyState Tile_occupy;
    public TileData(int x, int y, int type, float height = 0f)
    {
        Tile_x = x;
        Tile_y = y;
        Tile_type = type;
        Tile_height = height;
        Tile_type_prev = type;
        Tile_occupy = 0;
    }
    public void ChangeToPreviousType()
    {
        int tempType = Tile_type_prev;
        Tile_type_prev = Tile_type;
        Tile_type = tempType;
    }
    public void SetType(int _Type)
    {
        Tile_type_prev = Tile_type;
        Tile_type = _Type;
    }
    public int GetTileType()
    {
        return Tile_type;
    }
    /// <summary>
    /// 被敌方占用
    /// </summary>
    public void OccupyEnemy()
    {
        Tile_occupy = EnumOccupyState.Enemy;
    }
    /// <summary>
    /// 被我方占用
    /// </summary>
    public void OccupyPlayer()
    {
        Tile_occupy = EnumOccupyState.Player;
    }
    /// <summary>
    /// 解除Tile占用状态
    /// </summary>
    public void OccupyNone()
    {
        Tile_occupy = EnumOccupyState.None;
    }
    /// <summary>
    /// 是否被我方占用该Tile
    /// </summary>
    /// <returns></returns>
    public bool IsOccupiedByPlayer()
    {
        return Tile_occupy == EnumOccupyState.Player;
    }    /// <summary>
         /// 被敌方占用
         /// </summary>
    public bool IsOccupiedByEnemy()
    {
        return Tile_occupy == EnumOccupyState.Enemy;
    }
}