using UnityEngine;
using UnityEditor;
namespace RPGEditor
{
    [CustomEditor(typeof(Sequence.Sequence))]
    public class SequenceEditor : Editor
    {
        private static int weaponCount;
        private static bool foldOut = true;
        Sequence.Sequence _target;
        void OnEnable()
        {
            _target = target as Sequence.Sequence;
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            _target.transform.name = _target.SequenceName;
        }
    }
}
