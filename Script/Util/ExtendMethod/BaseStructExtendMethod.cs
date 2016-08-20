using UnityEngine;
using System.Collections;

public static class BaseStructExtendMethod
{
    public static Vector2 ToVector2XY(this Vector3 vector3)
    {
        return new Vector2(vector3.x, vector3.y);
    }
    public static Vector2 ToVector2XZ(this Vector3 vector3)
    {
        return new Vector2(vector3.x, vector3.z);
    }
    public static Vector3 ToVector3XY(this Vector2 vector2,float z=0f)
    {
        return new Vector3(vector2.x, vector2.y,z);
    }
    public static Vector3 ToVector2XZ(this Vector3 vector2,float y=0f)
    {
        return new Vector3(vector2.x,y, vector2.z);
    }
}
