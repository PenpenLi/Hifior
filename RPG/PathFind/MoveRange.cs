﻿using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
[System.Serializable]
public struct Point2D
{
    public int x;
    public int y;
    public Point2D(int X,int Y)
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
        return this==(Point2D)obj;
    }
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
public static class MoveRange
{
    /*
    [DllImport("CoreAlgorithm")]
    public static extern void ComputeMoveScope(int mapX, int mapY, int charX, int charY, int movement, int[,] mapData, int[,] occupyData);

    [DllImport("CoreAlgorithm")]
    public static extern void _FindDistance(int movement);

    [DllImport("CoreAlgorithm")]
    public static extern void DirectionScan(Point2D lastcord, Point2D cord, int surplusConsum, int tileComsum = 1, bool isOccupiedBySameParty = false);

    [DllImport("CoreAlgorithm")]
    public static extern bool IsEffectivelyCoordinate(int x, int y);

    [DllImport("CoreAlgorithm")]
    public static extern void InitMapData(ref int[,] mapData);

    [DllImport("CoreAlgorithm")]
    public static extern int GetMapData(int x, int y);

    [DllImport("CoreAlgorithm")]
    public static extern int GetOccupyState(int x, int y);

    [DllImport("CoreAlgorithm")]
    public static extern unsafe  int Test(int*[] mapData, int x, int y);*/
}