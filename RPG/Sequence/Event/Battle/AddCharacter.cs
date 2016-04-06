using UnityEngine;
using System.Collections;
namespace Sequence
{
    [AddComponentMenu("Sequence/Add Character")]
    public class AddCharacter : SequenceEvent
    {
        public enum ECamp
        {
            我方,
            敌方
        }
        public ECamp Camp;
        public int ID;
        public Point2D Coord;
        public bool UseDefaultAttribute = true;
        public CharacterAttribute Attribute;
        public override void OnEnter()
        {
            GM_Battle GameMode = GetGameMode<GM_Battle>();
            if (Camp == ECamp.我方)
            {
                if (UseDefaultAttribute)
                {
                    GameMode.AddPlayer(ID, Coord.x, Coord.y);
                }
                else
                {
                    GameMode.AddPlayer(ID, Coord.x, Coord.y, false, Attribute);
                }
            }
            else
            {
                if (UseDefaultAttribute)
                {
                    GameMode.AddEnemy(ID, Coord.x, Coord.y);
                }
                else
                {
                    GameMode.AddEnemy(ID, Coord.x, Coord.y, false, Attribute);
                }
            }
            Continue();
        }
    }
}