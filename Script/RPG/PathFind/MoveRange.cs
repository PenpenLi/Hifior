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