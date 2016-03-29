using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using Sequence;
/// <summary>
/// This is the custom editor script for the QuickTakeCutsceneController.
/// 
/// This script creates a custom inspector GUI for the QuickTakeCutsceneController
/// </summary>

[CustomEditor(typeof(SmoothMoveCamera))]
[CanEditMultipleObjects]
public class SmoothMoveCameraEditor : Editor
{
    //Variables start===============================================================

    //Quick references
    int numberOfEvents;
    bool[] showEvent;
    int[] choiceIndex;
    Transform[] midPoints;
    Transform[] cubicPoints;
    Transform[] points;
    bool isValid = true;
    bool hasQCC = false;

    GUIContent cameraContent = new GUIContent("Cutscene camera", "The main camera to be moved during the cutscene. No other cameras will be moved.");
    GUIContent nodesContent = new GUIContent("   >Path nodes", "Number of points on the bezier path to generate. Use a low value if you want a fast transition.");
    GUIContent curveSpeedContent = new GUIContent("   >Time", "How long should it take (seconds) for the camera to move along the path - impacted by path node amount");
    GUIContent delayContent = new GUIContent("Delay (sec)", "Time to wait (in seconds) before performing any actions in this event");
    GUIContent moveContent = new GUIContent("Movement ", "Camera positional movement speed during this event. (Units)");
    GUIContent midPointContent = new GUIContent("   >Midpoint 1: ", "The transform that will be used as a mid-point for this bezier curve. Should be a child of the preceding camera point in this transition.");
    GUIContent midPointTwoContent = new GUIContent("      >Midpoint 2: ", "The transform that will be used as a second mid-point for this bezier curve. Should be a child of the preceding mid point in this transition.");
    GUIContent rotationContent = new GUIContent("Rotation speed ", "Camera rotational speed during this event. (Degrees)");
    GUIContent zoomContent = new GUIContent("Zoom camera ", "Gradually changes the cameras FoV to zoom in/out");
    GUIContent broadcastContent = new GUIContent("Broadcast message", "Calls a desired method on the chosen GameObject");
    GUIContent lerpCurveContent = new GUIContent("Lerp", "Guarantees that this transition will finish in a set amount of time");

    //
    //====These below variables are camera point # dependant arrays===
    //

    //Messages to broadcast and array to check if we want to broadcast them 
    SerializedProperty broadcastMessageChoice;
    SerializedProperty broadcastMessageString;
    SerializedProperty broadcastMessageTarget;

    //Curve nodepoints/speed options
    SerializedProperty curveNodeCount;
    SerializedProperty customCurveMovementSpeed;

    //Targets for smooth follow
    SerializedProperty smoothFollowTarget;
    SerializedProperty smoothFollow;

    //Zoom amount for each event
    SerializedProperty cutsceneEventZoom;
    SerializedProperty cutsceneEventZoomAmount;
    SerializedProperty cutsceneEventZoomSpeed;

    /// The cutscene event key time.
    SerializedProperty cutsceneEventKeyTime;

    //The timescale event modifier
    SerializedProperty cutsceneEventTimescale;

    /// The camera shake option for each event.
    SerializedProperty doShake;

    /// The cutscene camera speed options.
    SerializedProperty cutsceneCameraSpeedOptions;
    SerializedProperty customRotationSpeed;
    SerializedProperty customMovementSpeed;

    /// The cutscene camera rotation speed options.
    SerializedProperty cutsceneCameraRotationSpeedOptions;

    // The cutscene path midpoints
    SerializedProperty cutsceneMidPoints;
    SerializedProperty cutsceneCubicMidPoints;

    //Choice of whether we want to curve during that transition
    SerializedProperty curveChoice;

    //And lerp
    SerializedProperty lerpCurveChoice;

    //Camera shake amount
    SerializedProperty cameraShakeAmount;

    //Variables end=====================================================================


    //Error handling and reference 'gettting' 
    void OnEnable()
    {

        //Gets the number of events/transitions
        if (Selection.activeTransform)
        {
            Transform t = Selection.activeTransform;

            List<Transform> cPoints = new List<Transform>();
            cPoints.Add(t);

            foreach (Transform trans in Selection.activeGameObject.transform)
            {
                cPoints.Add(trans);
            }

            points = cPoints.ToArray();
            numberOfEvents = points.Length;


            //And create the grand-child list of transforms (bezier curve midpoints)
            List<Transform> mPoints = new List<Transform>();
            for (int i = 0; i < numberOfEvents; i++)
            {
                foreach (Transform cT in points[i])
                {
                    mPoints.Insert(i, cT);
                }
            }

            midPoints = mPoints.ToArray();

            //Get cubic (grand-grand-child) midpoints
            List<Transform> cubePoints = new List<Transform>();
            foreach (Transform tran in midPoints)
            {
                if (tran.parent != t)
                {
                    if (tran.childCount > 0)
                    {
                        cubePoints.Add(tran.GetChild(0));
                    }
                }
                else {
                    cubePoints.Add(tran);
                }
            }

            cubicPoints = cubePoints.ToArray();

            if (t.GetComponent<SmoothMoveCamera>() == null)
            {
                hasQCC = false;
                Debug.LogWarning("When editing the cutscene, ensure that you always have the parent transform (with the Quick Cutscene Controller component on it) selected.");
            }
            else {
                hasQCC = true;
            }

        }
        else {
            Debug.Log("You can only edit the Cutscene as an in-scene object");
        }

        if (numberOfEvents < 3 && hasQCC == true)
        {
            Debug.Log("Not enough camera points found, adding camera points.");
            isValid = false;

            for (int i = numberOfEvents; i < 3; i++)
            {
                GameObject g = new GameObject();
                g.transform.position = (Selection.activeTransform.position + Random.insideUnitSphere * 5f);
                g.transform.rotation = Selection.activeTransform.rotation;
                g.transform.parent = Selection.activeTransform;
                string n = ("CameraPoint_" + i.ToString());
                g.name = n;
            }

            this.OnEnable();
        }
        else {
            if (hasQCC == true)
            {
                isValid = true;
            }
        }

        //Get proper references for variables primarily for use in setting array sizes 

        lerpCurveChoice = serializedObject.FindProperty("lerpCurveChoice");
        cutsceneCubicMidPoints = serializedObject.FindProperty("cutsceneCubicMidPoints");
        customCurveMovementSpeed = serializedObject.FindProperty("customCurveMovementSpeed");
        customMovementSpeed = serializedObject.FindProperty("customMovementSpeed");
        customRotationSpeed = serializedObject.FindProperty("customRotationSpeed");
        cameraShakeAmount = serializedObject.FindProperty("cameraShakeAmount");
        cutsceneEventZoomSpeed = serializedObject.FindProperty("cutsceneEventZoomSpeed");
        cutsceneEventZoomAmount = serializedObject.FindProperty("cutsceneEventZoomAmount");
        cutsceneEventZoom = serializedObject.FindProperty("cutsceneEventZoom");
        curveNodeCount = serializedObject.FindProperty("curveNodeCount");
        curveChoice = serializedObject.FindProperty("curveChoice");
        cutsceneMidPoints = serializedObject.FindProperty("cutsceneMidPoints");
        broadcastMessageChoice = serializedObject.FindProperty("broadcastMessageChoice");

        broadcastMessageString = serializedObject.FindProperty("broadcastMessageString");

        broadcastMessageTarget = serializedObject.FindProperty("broadcastMessageTarget");

        smoothFollow = serializedObject.FindProperty("smoothFollow");

        smoothFollowTarget = serializedObject.FindProperty("smoothFollowTarget");

        cutsceneEventKeyTime = serializedObject.FindProperty("cutsceneEventKeyTime");

        doShake = serializedObject.FindProperty("doShake");

        cutsceneCameraSpeedOptions = serializedObject.FindProperty("cutsceneCameraSpeedOptions");

        cutsceneCameraRotationSpeedOptions = serializedObject.FindProperty("cutsceneCameraRotationSpeedOptions");

        cutsceneEventTimescale = serializedObject.FindProperty("cutsceneEventTimescale");

        //Error handling
        if (cutsceneEventKeyTime.isArray == false || doShake.isArray == false || cutsceneCameraSpeedOptions.isArray == false
           || cutsceneCameraRotationSpeedOptions.isArray == false || smoothFollowTarget.isArray == false || cutsceneMidPoints.isArray == false || curveNodeCount.isArray == false
           )
        {
            // You shouldn't expect to see this.
            Debug.LogError("A property is not an array!");
        }

        //Array size handling/creation
        List<bool> boolList = new List<bool>();
        for (int i = 0; i < (numberOfEvents - 1); i++)
        {
            boolList.Add(false);
            SetArraySize(numberOfEvents - 2);
            serializedObject.ApplyModifiedProperties();
            //Debug.Log(cutsceneCameraSpeedOptions.arraySize);
        }

        //Create the bool array that handles showing each dropdown transition
        showEvent = boolList.ToArray();
    }

    void OnSceneGUI()
    {
        if (isValid)
        {
            //Quick reference to the Quicktakecutscenecontroller script
            SmoothMoveCamera q = target as SmoothMoveCamera;
            
            q.SetCameraPointReferences();

            //The size of the gizmos to draw for each camera point
            float gizmoSize = 0.4f;

            EditorGUI.BeginChangeCheck();

            //Only draw gizmos if we want to
            if (q.showPathType == PathType.PathOnly || q.showPathType == PathType.PathAndFrustum)
            {
                //Draw circles for each main cam point
                for (int i = 1; i < q.cutsceneCameraPoints.Length; i++)
                {
                    Handles.color = Color.green;
                    Undo.RecordObject(q.cutsceneCameraPoints[i], "Move");

                    q.cutsceneCameraPoints[i].position = Handles.FreeMoveHandle(q.cutsceneCameraPoints[i].position, Quaternion.identity,
                                                                                gizmoSize,
                                                                                Vector3.zero,
                                                                                Handles.SphereCap);
                }

                //Draw for each mid point
                for (int i = 0; i < q.cutsceneCameraPoints.Length - 2; i++)
                {
                    Handles.color = Color.yellow;

                    if (q.curveChoice[i] == true)
                    {
                        Undo.RecordObject(q.cutsceneMidPoints[i + 1], "Move");
                        q.cutsceneMidPoints[i + 1].position = Handles.FreeMoveHandle(q.cutsceneMidPoints[i + 1].position, Quaternion.identity,
                                                                                   gizmoSize * 0.9f,
                                                                                   Vector3.zero,
                                                                                   Handles.SphereCap);
                    }
                }

                //And draw for each cubic mid point, and a path
                for (int i = 0; i < q.cutsceneCameraPoints.Length - 2; i++)
                {
                    Handles.color = Color.magenta;

                    if (q.curveChoice[i] == true && q.cutsceneCameraSpeedOptions[i] == CameraSpeedOptions.Curve)//Only draw this midpoint if it is a cubic curve
                    {
                        Undo.RecordObject(q.cutsceneCubicMidPoints[i + 1], "Move");
                        q.cutsceneCubicMidPoints[i + 1].position = Handles.FreeMoveHandle(q.cutsceneCubicMidPoints[i + 1].position, Quaternion.identity,
                                                                                        gizmoSize * 0.8f,
                                                                                        Vector3.zero,
                                                                                        Handles.SphereCap);
                    }

                    Handles.color = Color.green;
                }
            }

        }

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Changes");

            EditorUtility.SetDirty(target);
        }

    }

    public RuntimeAnimatorController GetEffectiveController(Animator animator)
    {
        RuntimeAnimatorController controller = animator.runtimeAnimatorController;

        AnimatorOverrideController overrideController = controller as AnimatorOverrideController;
        while (overrideController != null)
        {
            controller = overrideController.runtimeAnimatorController;
            overrideController = controller as AnimatorOverrideController;
        }

        return controller;
    }

    public override void OnInspectorGUI()
    {
        if (isValid)
        {
            EditorGUILayout.HelpBox("Please ensure your camera not controled by other scripts", MessageType.Warning,true);
            //Updates the object we are editing
            serializedObject.Update();

            //Quick reference to the Quicktakecutscenecontroller script
            SmoothMoveCamera q = target as SmoothMoveCamera;
            q.waitUntilFinished = EditorGUILayout.Toggle("等待播放结束开始下一个事件", q.waitUntilFinished);

            EditorGUILayout.BeginHorizontal();
            //Button to manually call the StartCutscene function, only if the game is in play mode
            if (GUILayout.Button("Play"))
            {

                if (Application.isPlaying)
                {
                    q.ActivateCutscene();
                }

                if (!Application.isPlaying)
                {
                    Debug.Log("You can only play the cutscene when the game is running");
                }
            }
            //Button to manually call the StartCutscene function, only if the game is in play mode
            if (GUILayout.Button("Stop"))
            {

                if (Application.isPlaying)
                {
                    q.EndCutscene();
                }

                if (!Application.isPlaying)
                {
                    Debug.Log("You can only play/stop the cutscene when the game is running");
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Toggle Path (" + q.showPathType.GetHashCode() + ")"))
            {
                q.ToggleShowPath();
            }

            if (GUILayout.Button("Add camera point"))
            {
                GameObject g = new GameObject();
                g.transform.position = (Selection.activeTransform.position + Random.insideUnitSphere * 5f);
                g.transform.rotation = Selection.activeTransform.rotation;
                g.transform.parent = Selection.activeTransform;
                string n = ("CameraPoint_" + numberOfEvents.ToString());
                g.name = n;
                Undo.RegisterCreatedObjectUndo(g, n);
                this.OnEnable();
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            //Camera selector
            EditorGUILayout.LabelField(cameraContent);
            q.mainCutsceneCam = EditorGUILayout.ObjectField(q.mainCutsceneCam, typeof(Camera), true) as Camera;
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            //Use delta time
            //EditorGUILayout.BeginHorizontal();
            //q.useDeltaTime = EditorGUILayout.Toggle(deltaTimeContent, q.useDeltaTime);
            //EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            //This for loop controls the display of each camera transition event, and the variables relating to it.
            for (int i = 0; i < (numberOfEvents - 2); i++)
            {
                //Handle error which occurs when we undo a deleted CP
                if (!points[i + 2])
                {
                    this.OnEnable();
                }
                else {
                    GUIContent popoutContent = new GUIContent("Camera transition " + (i + 1) + " --> " + (i + 2) + "     (" + points[i + 1].name + " -> " + points[i + 2].name + ")");
                    showEvent[i] = EditorGUILayout.Foldout(showEvent[i], popoutContent);//"Camera transition " + (i+1) + " --> " + (i+2));
                }

                if (showEvent[i] == true)
                {
                    if (i >= q.cutsceneEventKeyTime.Length)
                    {
                        //Debug.Log("Refreshing Editor GUI");
                        //EditorGUIUtility.ExitGUI();
                        this.OnEnable();
                    }
                    q.cutsceneEventKeyTime[i] = EditorGUILayout.FloatField(delayContent, q.cutsceneEventKeyTime[i]);

                    //EditorGUILayout.Space();

                    q.cutsceneCameraSpeedOptions[i] = (CameraSpeedOptions)EditorGUILayout.EnumPopup(moveContent, q.cutsceneCameraSpeedOptions[i]);

                    if (q.cutsceneCameraSpeedOptions[i] == CameraSpeedOptions.Curve || q.cutsceneCameraSpeedOptions[i] == CameraSpeedOptions.MobileCurve)
                    {
                        q.curveChoice[i] = true;

                        EditorGUILayout.BeginHorizontal();

                        q.lerpCurveChoice[i] = EditorGUILayout.Toggle(lerpCurveContent, q.lerpCurveChoice[i]);

                        EditorGUILayout.EndHorizontal();

                        if (q.lerpCurveChoice[i])
                        {
                            q.customCurveMovementSpeed[i] = EditorGUILayout.Slider("   >Time (sec)", q.customCurveMovementSpeed[i], 0.0001f, 120f);
                        }
                        else {
                            moveContent = new GUIContent("Movement ", "Movement speed options. Mobile curves require 1 mid-point, normal curves require 2 mid-points.");
                            EditorGUILayout.BeginHorizontal();
                            q.curveNodeCount[i] = EditorGUILayout.IntSlider(nodesContent, q.curveNodeCount[i], 10, 1000);
                            EditorGUILayout.EndHorizontal();
                            q.customCurveMovementSpeed[i] = EditorGUILayout.Slider(curveSpeedContent, q.customCurveMovementSpeed[i], 0.0001f, 120f);
                        }

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(midPointContent);
                        EditorGUILayout.LabelField(midPoints[i + 1].name);
                        EditorGUILayout.EndHorizontal();
                        if (q.cutsceneCameraSpeedOptions[i] == CameraSpeedOptions.Curve)
                        {
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField(midPointTwoContent);
                            EditorGUILayout.LabelField(cubicPoints[i + 1].name);
                            EditorGUILayout.EndHorizontal();
                        }
                    }
                    else {
                        q.curveChoice[i] = false;
                        moveContent = new GUIContent("Movement ", "Camera positional movement speed during this event. (Units)");
                    }

                    //Custom movement speed
                    if (q.cutsceneCameraSpeedOptions[i] == CameraSpeedOptions.Custom)
                    {
                        q.customMovementSpeed[i] = EditorGUILayout.FloatField("   >Movement speed", q.customMovementSpeed[i]);
                    }

                    if (q.cutsceneCameraSpeedOptions[i] == CameraSpeedOptions.Lerp)
                    {
                        q.customMovementSpeed[i] = EditorGUILayout.FloatField("   >Movement time", q.customMovementSpeed[i]);
                    }

                    //EditorGUILayout.Space();

                    //Rotation speed
                    q.cutsceneCameraRotationSpeedOptions[i] = (CameraRotationSpeedOptions)EditorGUILayout.EnumPopup(rotationContent, q.cutsceneCameraRotationSpeedOptions[i]);
                    if (q.cutsceneCameraRotationSpeedOptions[i] == CameraRotationSpeedOptions.FollowTarget)
                    {
                        q.smoothFollowTarget[i] = (Transform)EditorGUILayout.ObjectField("   >Follow target", q.smoothFollowTarget[i], typeof(Transform), true) as Transform;
                    }

                    //Custom rotation speed
                    if (q.cutsceneCameraRotationSpeedOptions[i] == CameraRotationSpeedOptions.Custom)
                    {
                        q.customRotationSpeed[i] = EditorGUILayout.FloatField("   >Rotation speed", q.customRotationSpeed[i]);
                    }

                    //Custom rotation speed
                    if (q.cutsceneCameraRotationSpeedOptions[i] == CameraRotationSpeedOptions.Lerp)
                    {
                        q.customRotationSpeed[i] = EditorGUILayout.FloatField("   >Rotation time", q.customRotationSpeed[i]);
                    }

                    //EditorGUILayout.Space();

                    //Camera shake
                    q.doShake[i] = EditorGUILayout.Toggle("Shake camera ", q.doShake[i]);
                    if (q.doShake[i])
                    {
                        q.cameraShakeAmount[i] = EditorGUILayout.Slider("   >Shake intensity", q.cameraShakeAmount[i], 0.1f, 5f);
                    }

                    //EditorGUILayout.Space();

                    //time scale and broadcastmessage
                    q.cutsceneEventTimescale[i] = EditorGUILayout.Slider("Time scale", q.cutsceneEventTimescale[i], 0f, 2f);

                    //EditorGUILayout.Space();

                    q.broadcastMessageChoice[i] = EditorGUILayout.Toggle(broadcastContent, q.broadcastMessageChoice[i]);
                    if (q.broadcastMessageChoice[i] == true)
                    {
                        EditorGUILayout.BeginVertical();
                        q.broadcastMessageString[i] = EditorGUILayout.TextField("   >Method name", q.broadcastMessageString[i]);
                        q.broadcastMessageTarget[i] = EditorGUILayout.ObjectField("   >Target", q.broadcastMessageTarget[i], typeof(GameObject), true) as GameObject;
                        EditorGUILayout.EndVertical();
                    }

                    //EditorGUILayout.Space();

                    q.cutsceneEventZoom[i] = EditorGUILayout.Toggle(zoomContent, q.cutsceneEventZoom[i]);
                    if (q.cutsceneEventZoom[i] == true)
                    {
                        q.cutsceneEventZoomAmount[i] = EditorGUILayout.Slider("   >Field of View", q.cutsceneEventZoomAmount[i], 1f, 144f);
                        q.cutsceneEventZoomSpeed[i] = EditorGUILayout.Slider("   >Zoom speed", q.cutsceneEventZoomSpeed[i], 0.001f, 40f);
                    }


                    //EditorGUILayout.EndVertical();

                }
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }

            //And apply
            serializedObject.ApplyModifiedProperties();
        }
    }
    //Set the camera-point dependant arrays to their correct sizes
    void SetArraySize(int size)
    {
        lerpCurveChoice.arraySize = size;
        cutsceneCubicMidPoints.arraySize = size;
        customCurveMovementSpeed.arraySize = size;
        customRotationSpeed.arraySize = size;
        customMovementSpeed.arraySize = size;
        cameraShakeAmount.arraySize = size;
        curveNodeCount.arraySize = size;
        curveChoice.arraySize = size;
        cutsceneCameraSpeedOptions.arraySize = size;
        cutsceneCameraRotationSpeedOptions.arraySize = size;
        doShake.arraySize = size;
        cutsceneEventTimescale.arraySize = size;
        cutsceneEventKeyTime.arraySize = size;
        smoothFollowTarget.arraySize = size;
        smoothFollow.arraySize = size;
        broadcastMessageChoice.arraySize = size;
        broadcastMessageString.arraySize = size;
        broadcastMessageTarget.arraySize = size;
        cutsceneEventZoom.arraySize = size;
        cutsceneEventZoomAmount.arraySize = size;
        cutsceneEventZoomSpeed.arraySize = size;

        serializedObject.ApplyModifiedProperties();
    }

}