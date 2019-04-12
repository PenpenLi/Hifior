using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager :ManagerBase
{
    public enum EMouseKey
    {
        Left,
        Mid,
        Right,
    }
    public struct MouseInputState
    {
        public bool active;
        public EMouseKey key;
        public Vector2Int tilePos;
        public Vector3Int localPos;
    }
    public System.Func<MouseInputState> GetMouseInput;
    public System.Func<bool> GetYesInput;
    public System.Func<bool> GetNoInput;
}
