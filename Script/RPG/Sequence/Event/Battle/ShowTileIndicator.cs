using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Sequence
{
    [AddComponentMenu("Sequence/Show Tile Indicator")]
    public class ShowTileIndicator : SequenceEvent
    {
        public bool flash = true;
        public Vector2Int position;
        public float interval = 1.5f;
        public override void OnEnter()
        {
            gameMode.mapIndicator.ShowTileIndicator(position, flash);
            DelayContinue(interval);
        }
        public override void Continue()
        {
            gameMode.mapIndicator.HideTileIndicator();
            base.Continue();
        }
        public override bool OnStopExecuting()
        {
            return true;
        }
    }
}