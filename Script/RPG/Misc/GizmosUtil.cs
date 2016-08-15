using UnityEngine;
using System.Collections.Generic;

public class GizmosUtil
{
    /// <summary>
    /// 绘制以x,y为中心点的Gizmos矩形
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="c"></param>
    public static void GizmosDrawRect(float x, float y,float h, float width, float height, Color c)
    {
        Gizmos.color = c;
        float sizeOfX = width / 2.0f;
        float sizeOfY = height / 2.0f;
        Gizmos.DrawLine(new Vector3(x - sizeOfX, h ,y - sizeOfY), new Vector3(x + sizeOfX, h ,y - sizeOfY));
        Gizmos.DrawLine(new Vector3(x - sizeOfX, h ,y + sizeOfY), new Vector3(x + sizeOfX, h, y + sizeOfY));
        Gizmos.DrawLine(new Vector3(x - sizeOfX,  h,y - sizeOfY), new Vector3(x - sizeOfX, h, y + sizeOfY));
        Gizmos.DrawLine(new Vector3(x + sizeOfX,h, y - sizeOfY), new Vector3(x + sizeOfX, h, y + sizeOfY));
    }
}
