using UnityEngine;
using System.Collections;
public enum CameraControlMode
{
    DisableControl,
    FollowArrowCenter,
    FollowArrowEdge,
    FollowTransform,
    FreeMove
}
public class SLGCamera : MonoBehaviour
{
    private const float DEFAULT_CAMERA_HEIGHT = 180f;
    public float smoothTime = 1.5f;                // how smooth the camera movement is

    public float moveSpeed = 7.0f;
    public Vector2Int maxMoveable;

    public CameraControlMode ControlMode;
    private CameraControlMode oldControlMode;
    public Vector2Int FocusTilePos;
    private Vector3 targetPosition;
    private Transform targetTransform;
    public float Height = 180;
    public Vector3 ShiftVector = Vector3.zero;

    private Camera m_currentCamera;
    public Vector2Int LastArrowPoint;

    private float widthBorder = Screen.height / 10;
    private float heightBorder = Screen.height / 10;
    void Awake()
    {
        m_currentCamera = GetComponent<Camera>();
    }
    bool mouseStayInMoveZone = false;
    float mouseStayTime = 0.0f;
    void Update()
    {
        if (ControlMode == CameraControlMode.DisableControl)
            return;

        if (ControlMode == CameraControlMode.FreeMove)
        {
            if (mouseStayInMoveZone)
            {
                mouseStayTime += Time.deltaTime;
                if (mouseStayTime < 0.1f) return;
            }
            mouseStayInMoveZone = true;
            Vector3 mousePos = Input.mousePosition;
            var localPos = PositionMath.TilePositionToTileLocalPosition(maxMoveable - Vector2Int.one);
            if (mousePos.x > Screen.height - widthBorder && transform.localPosition.x < localPos.x)
            { transform.Translate(Vector3.right * PositionMath.TileLength, Space.Self);mouseStayTime = 0; }
            else if (mousePos.y > Screen.height - heightBorder && transform.localPosition.y < 0)
            { transform.Translate(Vector3.up * PositionMath.TileLength, Space.Self); mouseStayTime = 0; }
            else if (mousePos.x < widthBorder && transform.localPosition.x > 0)
            { transform.Translate(Vector3.left * PositionMath.TileLength, Space.Self); mouseStayTime = 0; }
            else if (mousePos.y < heightBorder && transform.localPosition.y >= localPos.y)
            { transform.Translate(Vector3.down * PositionMath.TileLength, Space.Self); mouseStayTime = 0; }
            else
            {
                mouseStayInMoveZone = false;
                mouseStayTime = 0.0f;
            }
            return;
        }
        {
            if (ControlMode == CameraControlMode.FollowTransform)
            {
                CameraFollowTargetPosition();
            }
            //LastArrowPoint = ArrowPoint;
            if (ControlMode == CameraControlMode.FollowArrowCenter)
            {
            }
            if (ControlMode == CameraControlMode.FollowArrowEdge)
            {

            }
        }
    }
    public void SetOldControlMode()
    {
        var temp = ControlMode;
        ControlMode = oldControlMode;
        oldControlMode = temp;
    }
    public void SetControlMode(CameraControlMode mode)
    {
        oldControlMode = ControlMode;
        ControlMode = mode;
    }
    public void StartFollowTransform(Transform t)
    {
        targetTransform = t;
        SetControlMode(CameraControlMode.FollowTransform);
    }
    public void CameraFollowTargetPosition()
    {
        Vector3 p = PositionMath.CameraLocalPositionFollowUnitLocalPosition(targetTransform.localPosition);
        transform.localPosition = p;
    }
}

