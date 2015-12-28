public class TileData
{
    public int Tile_x;
    public int Tile_y;
    /// <summary>
    /// 图块类型
    /// </summary>
    public int Tile_type;
    /// <summary>
    /// 前一个图块类型
    /// </summary>
    public int Tile_type_prev;
    public TileData(int x, int y, int _Type)
    {
        Tile_x = x;
        Tile_y = y;
        Tile_type = _Type;
        Tile_type_prev = -1;
    }
    public void ChangeType(int _Type)
    {
        Tile_type_prev = Tile_type;
        Tile_type = _Type;
    }
}