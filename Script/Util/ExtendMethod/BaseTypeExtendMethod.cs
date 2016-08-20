using UnityEngine;
using System.Collections;

public static class BaseTypeExtendMethod{

    public static int ToInt32(this string str)
    {
        return int.Parse(str);
    }
    public static float ToFloat(this string str)
    {
        return float.Parse(str);
    }
}
