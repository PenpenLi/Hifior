using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Sequence
{
    [HierarchyIcon("FadeScreen.png", 2)]
    [AddComponentMenu("Sequence/Camera MoveTo Tile")]
    public class CameraMoveToTile : SequenceEvent
    {
        public Vector2Int TilePos;
        public float MoveTime;
        public bool Accelerate;
        public bool WaitUntilFinished;
        public override void OnEnter()
        {
            if (WaitUntilFinished)
                gameMode.CameraMoveTo(TilePos, Continue, MoveTime, Accelerate);
            else
            {
                gameMode.CameraMoveTo(TilePos, null, MoveTime, Accelerate);
                Continue();
            }
        }
        public override bool OnStopExecuting()
        {
            gameMode.CameraMoveTo(TilePos, null);
            return true;
        }
        public override string GetSummary()
        {
            return "Camera move to tile " + TilePos;
        }
    }
}