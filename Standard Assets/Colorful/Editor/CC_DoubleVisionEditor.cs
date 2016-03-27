using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CC_DoubleVision))]
public class CC_DoubleVisionEditor : Editor
{
	SerializedObject srcObj;

	SerializedProperty displace;

	void OnEnable()
	{
		srcObj = new SerializedObject(target);

		displace = srcObj.FindProperty("displace");
	}

	public override void OnInspectorGUI()
	{
		displace.vector2Value = EditorGUILayout.Vector2Field("Displace", displace.vector2Value);

		srcObj.ApplyModifiedProperties();
    }
}
