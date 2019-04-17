using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace RPG.UI
{
    public class UI_TurnIndicate : IPanel
    {
        public Text text;

        public RectTransform[] rt;
        private RectTransform Top { get { return rt[0]; } }
        private RectTransform Bottom { get { return rt[1]; } }
        private Vector2 TopFromPos { get { return new Vector2(-defaultPosSize[0].Size.x, defaultPosSize[0].Pos.y); } }
        private Vector2 TopToPos { get { return defaultPosSize[0].Pos; } }
        private Vector2 BottomFromPos { get { return new Vector2(defaultPosSize[1].Size.x, defaultPosSize[1].Pos.y); } }
        private Vector2 BottomToPos { get { return defaultPosSize[1].Pos; } }
        public float duration;
        public float waitTime;
        public struct AnchoredSizePostion
        {
            public Vector2 Pos;
            public Vector2 Size;
            public AnchoredSizePostion(Vector2 pos, Vector2 size) { Pos = pos; Size = size; }
        }
        List<AnchoredSizePostion> defaultPosSize;
        protected override void Awake()
        {
            base.Awake();
            duration = ConstTable.CONST_TURN_IMAGE_ANIMATION_DURATION;
            waitTime = ConstTable.CONST_TURN_IMAGE_ANIMATION_WAITTIME;
            defaultPosSize = new List<AnchoredSizePostion>();
            foreach (var v in rt)
            {
                defaultPosSize.Add(new AnchoredSizePostion(v.anchoredPosition, v.sizeDelta));
            }
        }

        public void Show(EnumCharacterCamp camp, int Turn, UnityAction onHide)
        {
            base.Show();
            OnHideDelegate = onHide;
            text.text = camp.ToString() + "  Turn " + Turn;
            text.color = ConstTable.CAMP_COLOR(camp);
            AnimatePos(true, MoveOut);
        }
        private void MoveOut()
        {
            Utils.GameUtil.DelayFunc(() => AnimatePos(false, () => gameObject.SetActive(false)), waitTime);
        }
        private void AnimatePos(bool moveIn, UnityAction onFinish)
        {
            if (moveIn)
            {
                StartCoroutine(IAnimatorAnchoredPos(Top, TopFromPos, TopToPos, duration));
                StartCoroutine(IAnimatorAnchoredPos(Bottom, BottomFromPos, BottomToPos, duration, onFinish));
            }
            else
            {
                StartCoroutine(IAnimatorAnchoredPos(Top, TopToPos, TopFromPos, duration));
                StartCoroutine(IAnimatorAnchoredPos(Bottom, BottomToPos, BottomFromPos, duration, onFinish));
            }
        }
    }
}