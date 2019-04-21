using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : ManagerBase
{
    public enum EMouseKey
    {
        Left,
        Mid,
        Right,
    }
    public enum EKeyArrow
    {
        None,
        Left,
        Right,
        Up,
        Down
    }
    public struct MouseInputState
    {
        public bool active;
        public bool clickedUI;
        public EMouseKey key;
        public Vector2Int oldTilePos;
        public Vector2Int tilePos;
        public Vector3Int localPos;
        public bool IsClickedTile() { return active && key == EMouseKey.Left && !clickedUI; }
        public bool IsMouseTilePosChanged() { return oldTilePos != tilePos; }
    }
    public System.Func<MouseInputState> GetMouseInput;
    public System.Func<bool> GetYesInput;
    public System.Func<bool> GetNoInput;
    public System.Func<bool> GetStartInput;
    public System.Func<EKeyArrow> GetArrowInput;
}
