using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Assertions;
[System.Serializable]
public struct Point2D
{
    public int x;
    public int y;
    public static Point2D InvalidPoint
    {
        get
        {
            return new Point2D(-1, -1);
        }
    }
    public static Point2D ZeroPoint
    {
        get
        {
            return new Point2D(0, 0);
        }
    }
    public Point2D(int X, int Y)
    {
        x = X;
        y = Y;
    }
    public override string ToString()
    {
        return "x:" + x + " y:" + y;
    }
    public static bool operator ==(Point2D p1, Point2D p2)
    {
        return p1.x == p2.x && p1.y == p2.y;
    }
    public static bool operator !=(Point2D p1, Point2D p2)
    {
        return p1.x != p2.x || p1.y != p2.y;
    }
    public override bool Equals(object obj)
    {
        return this == (Point2D)obj;
    }
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
    public static Point2D Vector3ToPoint2D(Vector3 vector)
    {
        return new Point2D((int)vector.x / 10, (int)vector.z / 10);
    }

    public static Point2D Vector2ToPoint2D(Vector2 vector)
    {
        return new Point2D((int)vector.x / 10, (int)vector.y / 10);
    }

    public static Vector3 Point2DToVector3(float x, float y, float h)
    { //地图相对坐标坐标转换为大地图渲染位置
        float _x = x*10;
        float _z = y*10;
        return new Vector3(_x, h, _z);
    }
    public static Vector3 Point2DToVector3(int x, int y)
    { //地图相对坐标坐标转换为大地图渲染位置
        float _x = (x + 0.5f);
        float _z = -(y + 0.5f);
        return new Vector3(_x, 0, _z);
    }


}
public class TileCoord
{
    private List<Point2D> tileCoords = new List<Point2D>();
    private Point2D LeftTop;
    private Point2D RightTop;
    private Point2D LeftBottom;
    private Point2D RightBottom;
    public TileCoord(Point2D Point)
    {
        tileCoords.Add(Point);
        LeftTop = RightTop = LeftBottom = RightBottom = Point;
    }
    public TileCoord(Point2D LT, Point2D RT, Point2D LB, Point2D RB)
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
                tileCoords.Add(new Point2D(x, y));
            }
        }
    }
    public bool ContainCoord(Point2D p)
    {
        return tileCoords.Contains(p);
    }
    public bool OnePoint()
    {
        return tileCoords.Count == 1;
    }
}