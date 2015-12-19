using UnityEngine;
using UnityEditor;
namespace RPGEditor
{
    public class CareerMultiSelectionPopWindow : PopupWindowContent
    {
        public bool toggle1 = true;
        public bool toggle2 = true;
        public bool toggle3 = true;

        public override Vector2 GetWindowSize()
        {
            return new Vector2(200, 150);
        }

        public override void OnGUI(Rect rect)
        {
            GUILayout.Label("Popup Options Example", EditorStyles.boldLabel);
            toggle1 = EditorGUILayout.Toggle("Toggle 1", toggle1);
            toggle2 = EditorGUILayout.Toggle("Toggle 2", toggle2);
            toggle3 = EditorGUILayout.Toggle("Toggle 3", toggle3);
        }

        public override void OnOpen()
        {
        }
        public override void OnClose()
        {
            tempValue = (toggle1 ? 1 : 0) + (toggle2 ? 1 : 0) * 2 + (toggle3 ? 1 : 0) * 4;
        }
        UnityEngine.Events.UnityAction callBackOnClose;
        public void SetParams(int enumValue)
        {
            toggle1 = ((enumValue & 1 << 0) == 1 << 0);
            toggle2 = ((enumValue & 1 << 1) == 1 << 1);
            toggle3 = ((enumValue & 1 << 2) == 1 << 2);
        }
        private int tempValue;
        public int ReturnValue
        {
            get
            {
                return tempValue;
            }
        }
    }
}