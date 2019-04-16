using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Sequence
{
    [AddComponentMenu("Sequence/Load Tilemap")]
    public class LoadTileMap : SequenceEvent
    {
        public int MapId;
        public override void OnEnter()
        {
            gameMode.LoadTileMap(MapId);
            Continue();
        }
    }
}