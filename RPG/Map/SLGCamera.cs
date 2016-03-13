using UnityEngine;
using System.Collections;
public class SLGCamera : MonoBehaviour
{
    private const float DEFAULT_CAMERA_HEIGHT = 17.0f;
    public float smoothTime;                // how smooth the camera movement is

    public float moveSpeed = 7.0f;


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
    private float widthBorder = Screen.width / 10;
    private float heightBorder = Screen.height / 10;
    void FixedUpdate()
    {

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (Camera.main.fieldOfView <= 45.0f)
                Camera.main.fieldOfView += 3.0f;
            if (Camera.main.orthographicSize <= 40.0f)
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
}

