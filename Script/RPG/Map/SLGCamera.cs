using UnityEngine;
using System.Collections;
public enum CameraControlMode
{
    DisableControl,
    FollowArrowCenter,
    FollowArrowEdge,
    FollowCharacter,
    FreeMove
}
public class SLGCamera : MonoBehaviour
{
    private const float DEFAULT_CAMERA_HEIGHT = 180f;
    public float smoothTime = 1.5f;                // how smooth the camera movement is

    public float moveSpeed = 7.0f;
    public Vector2Int maxMoveable;

    public CameraControlMode ControlMode;

    private Vector3 TargetPosition;
    private Transform TargetTransform;
    public float Height = 180;
    public Vector3 ShiftVector = Vector3.zero;

    private Camera m_currentCamera;
    public VInt2 LastArrowPoint;

    private float widthBorder = Screen.height / 10;
    private float heightBorder = Screen.height / 10;
    void Awake()
    {
        m_currentCamera = GetComponent<Camera>();
    }
    void Update()
    {
        if (ControlMode == CameraControlMode.DisableControl)
            return;
        //if (LastArrowPoint != ArrowPoint)
        {
            //LastArrowPoint = ArrowPoint;
            if (ControlMode == CameraControlMode.FollowArrowCenter)
            {
                //    SetTargetPosition(ArrowPoint);
                MoveCameraToTargetPosition();
            }
            if (ControlMode == CameraControlMode.FollowArrowEdge)
            {
                if (ShouldMoveCamera())
                {
                    MoveCameraToTargetPosition();
                }
            }
        }
    }
    /// <summary>
    /// 跟随光标边缘模式是否需要移动摄像机
    /// </summary>
    /// <returns></returns>
    private bool ShouldMoveCamera()
    {
        Vector3 subPos = transform.position;
        bool shouldMove = false;
        float left = subPos.x - 40;
        if (left > 0)
        {
            TargetPosition = transform.position + Vector3.left * left;
            shouldMove = true;
        }
        float right = subPos.x + 40;
        if (right < 0)
        {
            TargetPosition = transform.position + Vector3.left * right;
            shouldMove = true;
        }
        float top = subPos.z - 30;
        if (top > 0)
        {

            TargetPosition = transform.position + Vector3.back * top;
            shouldMove = true;
        }
        float bot = subPos.z + 30;
        if (bot < 0)
        {
            TargetPosition = transform.position + Vector3.back * bot;
            shouldMove = true;
        }
        if (!shouldMove)
            TargetPosition = transform.position;
        return shouldMove;
    }
    public void MoveCameraToTargetPosition()
    {
        iTween.Stop();
        iTween.MoveTo(gameObject, TargetPosition, smoothTime);
    }


    void LateUpdate()
    {
        Vector3 mousePos = Input.mousePosition;
        var localPos = PositionMath.TilePositionToLocalPosition(maxMoveable);
        if (mousePos.x > Screen.height - widthBorder && transform.localPosition.x < localPos.x)
            transform.Translate(Vector3.right * PositionMath.TileLength, Space.Self);
        if (mousePos.y > Screen.height - heightBorder  && transform.localPosition.y < 0)
            transform.Translate(Vector3.up * PositionMath.TileLength, Space.Self);
        if (mousePos.x < widthBorder && transform.localPosition.x > 0)
            transform.Translate(Vector3.left * PositionMath.TileLength, Space.Self);
        if (mousePos.y < heightBorder&& transform.localPosition.y >= localPos.y)
            transform.Translate(Vector3.down * PositionMath.TileLength, Space.Self);
    }
}

