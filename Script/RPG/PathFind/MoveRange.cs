using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Assertions;
using System;

[System.Serializable]
public struct Range2D
{
    public int x, y, width, height;
    public static bool InRange(int x, int y, Range2D Range)
    {
        return (x >= Range.x && x <= Range.x + Range.width && y >= Range.y && y <= Range.y + Range.height);
    }
}
public class TileCoord
{
    private List<VInt2> tileCoords = new List<VInt2>();
    private VInt2 LeftTop = VInt2.ZeroPoint;
    private VInt2 RightTop = VInt2.ZeroPoint;
    private VInt2 LeftBottom = VInt2.ZeroPoint;
    private VInt2 RightBottom = VInt2.ZeroPoint;
    public TileCoord(VInt2 Point)
    {
        tileCoords.Add(Point);
        LeftTop = RightTop = LeftBottom = RightBottom = Point;
    }
    public TileCoord(VInt2 LT, VInt2 RT, VInt2 LB, VInt2 RB)
    {
        Assert.IsTrue(LT.x > RT.x || LB.x > RB.x, "左边不能大于右边的坐标");
        Assert.IsTrue(LT.y < LB.y || RT.y < RB.y, "上边不能大于下边的坐标");
        Assert.IsTrue(LT.y != RT.y || LB.y != RB.y, "y轴上下坐标不一致");
        Assert.IsTrue(LT.x != LB.x || RT.x != RB.x, "x轴左右坐标不一致");
        LeftTop = LT;
        RightTop = RT;
        LeftBottom = LB;
        RightBottom = RB;
        for (int y = LB.y; y < LT.y; y++)
        {
            for (int x = LT.x; x < RT.x; x++)
            {
                tileCoords.Add(new VInt2(x, y));
            }
        }
    }
    public bool ContainCoord(VInt2 p)
    {
        return tileCoords.Contains(p);
    }
    public bool OnePoint()
    {
        return tileCoords.Count == 1;
    }
}