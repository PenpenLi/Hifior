using UnityEngine;
using System.Collections;

public class ShowFPS : MonoBehaviour
{
    public float f_UpdateInterval = 0.3F;
    private float f_LastInterval;
    private int i_Frames = 0;
    private float f_Fps;
    private float f_ScreenScale;

    void Start()
    {
        //Application.targetFrameRate=60;
        f_LastInterval = Time.realtimeSinceStartup;
        i_Frames = 0;
        f_ScreenScale = UnityEngine.Screen.height / 640.0f;
    }

    void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.normal.background = null;
        style.normal.textColor = Color.green;
        style.fontSize = (int)(20 * f_ScreenScale);
        GUI.Label(new Rect(0, 0, (int)(200 * f_ScreenScale), (int)(200 * f_ScreenScale)), "FPS:" + f_Fps.ToString("f2"), style);
    }

    void Update()
    {
        ++i_Frames;
        if (Time.realtimeSinceStartup > f_LastInterval + f_UpdateInterval)
        {
            f_Fps = i_Frames / (Time.realtimeSinceStartup - f_LastInterval);
            i_Frames = 0;
            f_LastInterval = Time.realtimeSinceStartup;
        }
    }
}