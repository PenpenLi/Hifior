using UnityEngine;
using UnityEditor;
using RPG.UI;
[CustomEditor(typeof(AbstractUI),true)]
public class AbstructUIEditor : Editor {
    AbstractUI Target;
    public void OnEnable()
    {
        Target = target as AbstractUI;
    }
    public override void OnInspectorGUI()
    {
        if(GUILayout.Button("Play Fade", GUILayout.Width(100)))
        {
            if (Application.isPlaying)
            {
                Target.ShowFade();
            }
            else
            {
                Debug.Log("只能在播放模式下播放");
            }
        }
        DrawDefaultInspector();
    }

}
