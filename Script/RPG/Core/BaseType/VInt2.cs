using System;
using UnityEngine;
using System.Runtime.InteropServices;

[Serializable, StructLayout(LayoutKind.Sequential)]
public struct VInt2
{
    public int x;
    public int y;
    public static VInt2 InvalidPoint
    {
        get
        {
            return new VInt2(-1, -1);
        }
    }
    public static VInt2 ZeroPoint
    {
        get
        {
            return new VInt2(0, 0);
        }
    }
    public static VInt2 Up
    {
        get
        {
            return new VInt2(0, 1);
        }
    }
    public static VInt2 Down
    {
        get
        {
            return new VInt2(0, -1);
        }
    }
    public static VInt2 Left
    {
        get
        {
            return new VInt2(-1, 0);
        }
    }
    public static VInt2 Right
    {
        get
        {
            return new VInt2(1, 0);
        }
    }
    public VInt2(int X, int Y)
    {
        x = X;
        y = Y;
    }
    public override string ToString()
    {
        return "x:" + x + " y:" + y;
    }
    public int sqrMagnitude
    {
        get
        {
            return ((this.x * this.x) + (this.y * this.y));
        }
    }
    public long sqrMagnitudeLong
    {
        get
        {
            long x = this.x;
            long y = this.y;
            return ((x * x) + (y * y));
        }
    }
    public int magnitude
    {
        get
        {
            long x = this.x;
            long y = this.y;
            return (int)Mathf.Sqrt((x * x) + (y * y));
        }
    }
    public static int Dot(VInt2 a, VInt2 b)
    {
        return ((a.x * b.x) + (a.y * b.y));
    }

    public static VInt2 operator +(VInt2 p1, VInt2 p2)
    {
        return new VInt2(p1.x + p2.x, p1.y + p2.y);
    }
    public static VInt2 operator -(VInt2 p1, VInt2 p2)
    {
        return new VInt2(p1.x - p2.x, p1.y - p2.y);
    }
    public static bool operator ==(VInt2 p1, VInt2 p2)
    {
        return p1.x == p2.x && p1.y == p2.y;
    }
    public static bool operator !=(VInt2 p1, VInt2 p2)
    {
        return p1.x != p2.x || p1.y != p2.y;
    }
    public void Normalize()
    {
        long num = this.x * 100;
        long num2 = this.y * 100;
        long a = (num * num) + (num2 * num2);
        if (a != 0)
        {
            this.x /= magnitude;
            this.y /= magnitude;
        }
    }

    public VInt2 normalized
    {
        get
        {
            VInt2 num = new VInt2(this.x, this.y);
            num.Normalize();
            return num;
        }
    }

    public static explicit operator Vector2(VInt2 ob)
    {
        return new Vector2(ob.x * 0.001f, ob.y * 0.001f);
    }

    public static explicit operator VInt2(Vector2 ob)
    {
        return new VInt2((int)Math.Round((double)(ob.x * 1000f)), (int)Math.Round((double)(ob.y * 1000f)));
    }

    public static VInt2 operator *(VInt2 lhs, int rhs)
    {
        lhs.x *= rhs;
        lhs.y *= rhs;
        return lhs;
    }

    public override bool Equals(object obj)
    {
        return (this.x == ((VInt2)obj).x) && (this.y == ((VInt2)obj).y);
    }

    public override int GetHashCode()
    {
        return ((this.x * 0xc005) + (this.y * 0x1800d));
    }
    public static VInt2 Min(VInt2 a, VInt2 b)
    {
        return new VInt2(Math.Min(a.x, b.x), Math.Min(a.y, b.y));
    }

    public static VInt2 Max(VInt2 a, VInt2 b)
    {
        return new VInt2(Math.Max(a.x, b.x), Math.Max(a.y, b.y));
    }




    public static VInt2 Vector3ToPoint2D(Vector3 vector)
    {
        return new VInt2((int)vector.x / 10, (int)vector.z / 10);
    }

    public static VInt2 Vector2ToPoint2D(Vector2 vector)
    {
        return new VInt2((int)vector.x / 10, (int)vector.y / 10);
    }
    /// <summary>
    /// 地图相对坐标坐标转换为大地图渲染位置
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="h"></param>
    /// <param name="Shift">是否有半个单位的偏移</param>
    /// <returns></returns>
    public static Vector3 VInt2ToVector3(float x, float y, float h, bool Shift)
    {
        float _x = x * 10;
        float _z = y * 10;

        if (Shift)
        {
            _x += 5;
            _z += 5;
        }
        return new Vector3(_x, h, _z);
    }
    /// <summary>
    /// 地图相对坐标坐标转换为大地图渲染位置
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="Shift">是否有半个单位的偏移</param>
    /// <returns></returns>
    public static Vector3 VInt2ToVector3(int x, int y, bool Shift)
    {
        float _x = x * 10;
        float _z = y * 10;

        if (Shift)
        {
            _x += 5;
            _z += 5;
        }
        return new Vector3(_x, 0, _z);
    }
    public static Vector3 VInt2ToVector3(VInt2 p, bool Shift)
    {
        return VInt2ToVector3(p.x, p.y, Shift);
    }
    public static int GetDistance(int x1, int y1, int x2, int y2)
    {
        return Mathf.Abs(x1 - x2) + Mathf.Abs(y1 - y2);
    }
    public static int GetDistance(VInt2 Point1, VInt2 Point2)
    {
        return Mathf.Abs(Point1.x - Point2.x) + Mathf.Abs(Point1.y - Point2.y);
    }
    public int Distance(VInt2 point)
    {
        return GetDistance(this, point);
    }
}