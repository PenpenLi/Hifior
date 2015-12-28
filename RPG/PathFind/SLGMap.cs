using UnityEngine;
using System.Collections;

public class SLGMap : MonoBehaviour {
    public static Point2D Vector3ToPoint2D(Vector3 vector)
    {
        return new Point2D((int)vector.x / 10, (int)vector.z / 10);
    }

    public static Point2D Vector2ToPoint2D(Vector2 vector)
    {
        return new Point2D((int)vector.x / 10, (int)vector.y / 10);
    }
}
