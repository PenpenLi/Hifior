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
        public override void OnEnter()
        {
            gameMode.CameraMoveTo(TilePos, MoveTime, Accelerate);
        }
    }
}