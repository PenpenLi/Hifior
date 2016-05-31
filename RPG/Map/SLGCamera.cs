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
public class SLGCamera : UActor
{
    private const float DEFAULT_CAMERA_HEIGHT = 180f;
    public float smoothTime = 1.5f;                // how smooth the camera movement is

    public float moveSpeed = 7.0f;


    public CameraControlMode ControlMode;

    private Vector3 TargetPosition;
    private Transform TargetTransform;
    public float Height = 180;
    public Vector3 ShiftVector = Vector3.zero;
    /*
    public float maxDistance = 30f;
    public float minDistance = 10f;
    private int zoomRate = 40;
    private float panSpeed = 0.3f;
    private float zoomDampening = 5.0f;
     private float autoRotate = 1f;
     private float autoRotateSpeed = 0.1f;

    private float currentDistance;
    private float desiredDistance;
    private Quaternion currentRotation;
    private Quaternion desiredRotation;
    private Quaternion rotation;
    private Vector3 position;
    // private float idleTimer = 0.0f;
    // private float idleSmooth = 0.0f;
	*/
    //边界最小值  

    public Point2D ArrowPoint
    {
        get { return GetPlayerPawn<Pawn_BattleArrow>().Position; }
    }
    private Camera m_currentCamera;
    public Point2D LastArrowPoint;
    public Vector3 ArrowPosition
    {
        get
        {
            return GetPlayerPawn<Pawn_BattleArrow>().transform.position;
        }
    }
    private float widthBorder = Screen.width / 10;
    private float heightBorder = Screen.height / 10;
    void Awake()
    {
        m_currentCamera = GetComponent<Camera>();
    }
    void Update()
    {
        if (ControlMode == CameraControlMode.DisableControl)
            return;
        if (LastArrowPoint != ArrowPoint)
        {
            LastArrowPoint = ArrowPoint;
            if (ControlMode == CameraControlMode.FollowArrowCenter)
            {
                SetTargetPosition(ArrowPoint);
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
        Vector3 subPos = transform.position - ArrowPosition;
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
        /* Vector3 ve= m_currentCamera.ViewportToScreenPoint(ArrowPosition);
             float perX = ve.x / Screen.width;
             if ( perX< 0.2f)
             {
                start transform.TransformVector(Vector3.left);
             }
         if (perX > 0.8f)
         {
             transform.TransformVector(Vector3.left);
         }*/
    }
    public void MoveCameraToTargetPosition()
    {
        iTween.Stop();
        iTween.MoveTo(gameObject, TargetPosition, smoothTime);
    }
    private void SetTargetPosition(Point2D p)
    {
        SetTargetPosition(p.x, p.y);
    }
    private void SetTargetPosition(int x, int y)
    {
        TargetPosition = Point2D.Point2DToVector3(x, y, true) + Vector3.up * Height + ShiftVector;
    }
    private void SetTargetPosition(Transform t)
    {
        TargetPosition = t.position + Vector3.up * Height + ShiftVector;
    }
    void HandleZoom()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (Camera.main.fieldOfView <= 85.0f)
                Camera.main.fieldOfView += 3.0f;
            if (Camera.main.orthographicSize <= 50.0f)
                Camera.main.orthographicSize += 0.5f;
        }
        //Zoom in
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (Camera.main.fieldOfView > 15.0f)
                Camera.main.fieldOfView -= 3.0f;
            if (Camera.main.orthographicSize >= 20f)
                Camera.main.orthographicSize -= 0.5f;
        }
    }
    void LateUpdate()
    {

        /* if (Input.GetKeyUp(KeyCode.E))//绕镜头旋转
         {
             if (currentDirection == 0 || currentDirection == -1)
             {
                 currentDirection++;
                 RotateCamera(1);
             }
         }
         if (Input.GetKeyUp(KeyCode.Q))//绕镜头旋转
         {
             if (currentDirection == 0 || currentDirection == 1)
             {
                 currentDirection--;
                 RotateCamera(-1);
             }
         }*/

        if (Input.GetKey(KeyCode.W) && transform.rotation.eulerAngles.x > 45f)
        {
            transform.Rotate(Vector3.left, 0.1f);
        }
        if (Input.GetKey(KeyCode.S) && transform.rotation.eulerAngles.x < 70f)
        {
            transform.Rotate(Vector3.right, 0.1f);
        }
        /*if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(Vector3.forward, 0.2f);
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(Vector3.back, 0.2f);
        }*/
        Vector3 mousePos = Input.mousePosition;
        if (mousePos.y > Screen.height - heightBorder)
            transform.Translate(Vector3.up * 10 * Time.deltaTime, Space.Self);
        if (mousePos.x < widthBorder)
            transform.Translate(Vector3.left * 10 * Time.deltaTime, Space.Self);
        if (mousePos.y < heightBorder)
            transform.Translate(Vector3.down * 10 * Time.deltaTime, Space.Self);
        if (mousePos.x > Screen.width - widthBorder)
            transform.Translate(Vector3.right * 10 * Time.deltaTime, Space.Self);
    }
}

