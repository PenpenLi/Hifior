/*
  Created by Juan Sebastian Munoz Arango 2013
  naruse@gmail.com
  all rights reservedDESCRIPTION:
ProPivot modifier package is an editor utility that lets you modify your
pivot points that come from any 3rd party modeling software and re-organize
them so you can work with the pivots correctly instead using the "hack" of adding Empty
GameObjects to organize the pivot orientation of your meshes.

Pivots can be rotated, moved and centered by average or bounding box.

Rotate: Rotates the selected mesh around the global axis of the pivot.

Move: Moves the selected mesh around the selected pivot.

Average Center: Finds the average center of all the vertexes and moves the
mesh around the average center where the pivot gets placed

Bounding Box Center: Centers the pivot around the bounding box of the mesh.

For better understanding check the images or watch the demo video on how to
use the ProPivot modifier editor utility at:

http://www.youtube.com/watch?v=PA3gG2t0X5c

USAGE:
To open the menu window for ProPivotModifier go to 
Window --> ProPivotModifier

Check my other projects!
Pro Draw Call optimizer: Reduce drastically your drawcalls with a couple of clicks
-- https://www.assetstore.unity3d.com/#/content/16538

- Pro Mouse: Set the cursor position wherever you want
-- https://www.assetstore.unity3d.com/en/#!/content/8910

 */
using UnityEngine;
using UnityEditor;

public sealed class PivotModifier : EditorWindow
{

    private Vector3 pivotRotation = Vector3.zero;
    private Vector3 oldPivotRotation = Vector3.zero;

    private Vector3 pivotPosition = Vector3.zero;
    private Vector3 oldPivotPosition = Vector3.zero;

    private GameObject currentSelectedObject = null;
    private GameObject oldSelectedObject = null;

    private GUIStyle errorGUIStyle = new GUIStyle();
    private Mesh lastSelectedMesh = null;
    private static Texture2D helpTexture;

    //used when you want to displace with the keyboard
    private bool exactMovementFlag = false;

    [MenuItem("Window/ProPivotModifier")]
    private static void Init()
    {
        PivotModifier window = (PivotModifier)EditorWindow.GetWindow(typeof(PivotModifier));
        window.Show();
        Tools.pivotRotation = PivotRotation.Local;
        //Debug.Log("Set to local pivot rotation");
    }

    void OnGUI()
    {
        if (currentSelectedObject && Selection.activeTransform)
            if (currentSelectedObject.GetInstanceID() != Selection.activeTransform.gameObject.GetInstanceID())
                ResetToInitialValues();
        currentSelectedObject = Selection.activeTransform != null ? (Selection.activeTransform.GetComponent<MeshFilter>() ? Selection.activeTransform.gameObject : null) : null;
        if (!currentSelectedObject)
        {
            errorGUIStyle.normal.textColor = Color.red;
            GUILayout.Label("Select a GameObject that has a MeshFilter component", errorGUIStyle);
        }
        else if (IsMeshGenerated(currentSelectedObject))
        {
            errorGUIStyle.normal.textColor = Color.red;
            GUILayout.Label("The selected MeshFilter is a generated MeshFilter.\nSelect a mesh that resides in the ProjectView.", errorGUIStyle);
        }

        if (currentSelectedObject != null && oldSelectedObject != null)
            if (oldSelectedObject.GetInstanceID() != currentSelectedObject.GetInstanceID())
            {
                ResetToInitialValues();
                oldSelectedObject = currentSelectedObject;
            }

        GUI.enabled = (currentSelectedObject != null && !IsMeshGenerated(currentSelectedObject));

        exactMovementFlag = EditorGUILayout.Toggle("Exact movement:", exactMovementFlag);
        pivotRotation = EditorGUILayout.Vector3Field("Pivot Rotation:", pivotRotation);
        pivotPosition = EditorGUILayout.Vector3Field("Pivot Position:", pivotPosition);

        if (exactMovementFlag)
        {
            if (GUILayout.Button("Apply modifications"))
            {
                ProPivotModifier.RotateSelectedMesh(currentSelectedObject.GetComponent<MeshFilter>().sharedMesh, pivotRotation - oldPivotRotation);
                oldPivotRotation = pivotRotation;
                lastSelectedMesh = currentSelectedObject.GetComponent<MeshFilter>().sharedMesh;

                ProPivotModifier.MoveSelectedMesh(currentSelectedObject.GetComponent<MeshFilter>().sharedMesh, pivotPosition - oldPivotPosition);
                oldPivotPosition = pivotPosition;
                lastSelectedMesh = currentSelectedObject.GetComponent<MeshFilter>().sharedMesh;
            }
        }

        if (!exactMovementFlag)
        {
            if (oldPivotRotation != pivotRotation)
            {
                //as of Unity4.3, saving the state of an object is really slow, hence for the time being, this has to be deactivated
                //as this cant be done frame by frame
                //Undo.RecordObject(currentSelectedObject.GetComponent<MeshFilter>().sharedMesh, "Rotate around pivot");
                ProPivotModifier.RotateSelectedMesh(currentSelectedObject.GetComponent<MeshFilter>().sharedMesh, pivotRotation - oldPivotRotation);
                oldPivotRotation = pivotRotation;
                lastSelectedMesh = currentSelectedObject.GetComponent<MeshFilter>().sharedMesh;
            }

            if (oldPivotPosition != pivotPosition)
            {
                //as of Unity4.3, saving the state of an object is really slow, hence for the time being, this has to be deactivated
                //as this cant be done frame by frame
                //Undo.RecordObject(currentSelectedObject.GetComponent<MeshFilter>().sharedMesh, "Move Pivot");
                ProPivotModifier.MoveSelectedMesh(currentSelectedObject.GetComponent<MeshFilter>().sharedMesh, pivotPosition - oldPivotPosition);
                oldPivotPosition = pivotPosition;
                lastSelectedMesh = currentSelectedObject.GetComponent<MeshFilter>().sharedMesh;
            }
        }

        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Reset All Pivots"))
        {
            Undo.RecordObject(currentSelectedObject.GetComponent<MeshFilter>().sharedMesh, "Reset All Pivots");
            ReimportAsset(currentSelectedObject.GetComponent<MeshFilter>().sharedMesh);
            ResetToInitialValues();
            lastSelectedMesh = currentSelectedObject.GetComponent<MeshFilter>().sharedMesh;
        }
        if (GUILayout.Button("Revert To Original Mesh"))
        {
            Undo.RecordObject(currentSelectedObject.GetComponent<MeshFilter>().sharedMesh, "Revert to original Mesh");
            ReimportAsset(currentSelectedObject.GetComponent<MeshFilter>().sharedMesh);
            RevertPrefabInstance(currentSelectedObject);
            ResetToInitialValues();
            lastSelectedMesh = currentSelectedObject.GetComponent<MeshFilter>().sharedMesh;

        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Average Center"))
        {
            Undo.RecordObject(currentSelectedObject.GetComponent<MeshFilter>().sharedMesh, "Set Pivot on average center");
            ProPivotModifier.AverageCenterPivot(currentSelectedObject.GetComponent<MeshFilter>().sharedMesh, currentSelectedObject.transform.position);
            ResetToInitialValues();
            lastSelectedMesh = currentSelectedObject.GetComponent<MeshFilter>().sharedMesh;
        }
        if (GUILayout.Button("Center Pivot"))
        {
            Undo.RecordObject(currentSelectedObject.GetComponent<MeshFilter>().sharedMesh, "Center pivot");
            ProPivotModifier.CenterPivot(currentSelectedObject.GetComponent<MeshFilter>().sharedMesh, currentSelectedObject.transform);
            ResetToInitialValues();
            lastSelectedMesh = currentSelectedObject.GetComponent<MeshFilter>().sharedMesh;
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();

        GUI.enabled = true;
        if (helpTexture)
            GUILayout.Label(helpTexture, GUILayout.MaxHeight(20), GUILayout.MaxWidth(512));
        else
            GUILayout.Label("Couldnt find 'Help.png',Place ProPivotModifier folder under 'Assets/'", errorGUIStyle);

        /*if (Event.current.type == EventType.ValidateCommand) {
            switch (Event.current.commandName) {
            case "undoRedoPerformed":
                Debug.Log("TEST");
                RefreshMesh(lastSelectedMesh);
                ResetToInitialValues();
                break;
            }
        }*/

        if (Event.current.commandName == "UndoRedoPerformed")
        {
            RefreshMesh(lastSelectedMesh);
            ResetToInitialValues();
        }

        GUI.enabled = (currentSelectedObject != null && !IsMeshGenerated(currentSelectedObject));
        // This is used for Unity4.2+ as it was a bug introduced in this version; basically the MeshFilter component on the sharedMesh atribute doesnt preserve the
        // modifications across editor sessions (as in making changes, closing and opening the editor back again).
        // this can be removed when unity decides to fix the bug :(.
        // A ticket has already been submited for this issue, nothing heard from  UTech
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Save and make prefab"))
        {
            SaveObject(true/*Create prefab*/);
        }

        if (GUILayout.Button("Save"))
        {
            SaveObject();
        }
        GUILayout.EndHorizontal();
    }

    void SaveObject(bool createPrefabFromModifiedPivot = false)
    {
        Debug.Log("Saving pivot to game object: " + currentSelectedObject.name + "ModifiedPivot" + currentSelectedObject.GetInstanceID());

        Undo.RecordObject(currentSelectedObject, "Save Object" + currentSelectedObject.name);
        //create a copy of the modified object.
        Quaternion originalRot = currentSelectedObject.transform.localRotation;
        Vector3 originalPos = currentSelectedObject.transform.position;
        Vector3 originalScale = currentSelectedObject.transform.localScale;

        currentSelectedObject.transform.position = Vector3.zero;
        currentSelectedObject.transform.rotation = Quaternion.identity;
        currentSelectedObject.transform.localScale = new Vector3(1, 1, 1);

        if (GameObject.Find(currentSelectedObject.name + "ModifiedPivot" + currentSelectedObject.GetInstanceID()) != null)
        {
            //Destroy the old GameObject
            DestroyImmediate(GameObject.Find(currentSelectedObject.name + "ModifiedPivot" + currentSelectedObject.GetInstanceID()));
        }
        //unity4.2-
        //Undo.RegisterSceneUndo("Create Object " + currentSelectedObject.name + "ModifiedPivot"+currentSelectedObject.GetInstanceID());
        GameObject instance = Instantiate(currentSelectedObject, Vector3.zero, Quaternion.identity) as GameObject;
        Undo.RegisterCreatedObjectUndo(instance, "Create Object " + currentSelectedObject.name + "ModifiedPivot" + currentSelectedObject.GetInstanceID());
        instance.transform.parent = currentSelectedObject.transform.parent;
        instance.name = currentSelectedObject.name + "ModifiedPivot" + currentSelectedObject.GetInstanceID();
        //Deactivate the original object
        currentSelectedObject.SetActive(false);
        instance.SetActive(true);

        CombineInstance c = new CombineInstance();
        c.mesh = currentSelectedObject.GetComponent<MeshFilter>().sharedMesh;
        c.transform = currentSelectedObject.transform.localToWorldMatrix;
        CombineInstance[] arr = new CombineInstance[1];
        arr[0] = c;
        Mesh comb = new Mesh();
        comb.CombineMeshes(arr);
        instance.GetComponent<MeshFilter>().sharedMesh = comb;


        currentSelectedObject.transform.position = originalPos;
        currentSelectedObject.transform.rotation = originalRot;
        currentSelectedObject.transform.localScale = originalScale;
        instance.transform.position = originalPos;
        instance.transform.rotation = originalRot;
        instance.transform.localScale = originalScale;

        if (createPrefabFromModifiedPivot)
        {
            string meshPath = "Assets/" + instance.name;
            AssetDatabase.CreateAsset(instance.GetComponent<MeshFilter>().sharedMesh, meshPath + ".asset");
            PrefabUtility.CreatePrefab(meshPath + ".prefab", instance, ReplacePrefabOptions.ConnectToPrefab);
            Selection.activeGameObject = instance;
        }
    }


    void OnInspectorUpdate()
    {
        helpTexture = AssetDatabase.LoadAssetAtPath("Assets/Editor/Res/PivotModifyHelp.png", typeof(Texture2D)) as Texture2D;
        Repaint();
        //Only way to Repaint primitives
    }

    private bool IsMeshGenerated(GameObject g)
    {
        return (AssetDatabase.GetAssetPath(g.GetComponent<MeshFilter>().sharedMesh) == "");
    }

    private void ReimportAsset(Mesh meshToReimport)
    {
        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(meshToReimport));
    }

    private void RevertPrefabInstance(GameObject g)
    {
        PrefabUtility.RevertPrefabInstance(g);
    }

    private void ResetToInitialValues()
    {
        pivotPosition = oldPivotPosition = pivotRotation = oldPivotRotation = Vector3.zero;
    }

    //make the mesh repaint itself
    private void RefreshMesh(Mesh m)
    {
        if (m != null)
            m.vertices = m.vertices;
    }
}