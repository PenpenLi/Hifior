[System.Serializable]
public struct TileData
{
    public int X;
    public int Y;
    /// <summary>
    /// 图块类型
    /// </summary>
    public int Type;
    /// <summary>
    /// 当前块的高度
    /// </summary>
    public float Height;

    public TileData(int x, int y, int type, float height = 0f)
    {
        X = x;
        Y = y;
        Type = type;
        Height = height;
    }
}