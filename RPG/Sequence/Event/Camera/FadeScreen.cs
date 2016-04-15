﻿using UnityEngine;
using System.Collections;

namespace Sequence
{
    [HierarchyIcon("FadeScreen.png", 2)]
    [AddComponentMenu("Sequence/Fade Screen")]
    public class FadeScreen : SequenceEvent
    {
        public enum 渐变类型
        {
            正常变黑,
            黑变正常,
            正常变白,
            白变正常,
        }
        [Tooltip("多久完成")]
        public float duration = 1f;

        [Tooltip("渐变类型")]
        public 渐变类型 FadeType;

        public bool waitUntilFinished = true;

        public override void OnEnter()
        {
            switch (FadeType)
            {
                case 渐变类型.正常变黑:
                    UIController.ScreenNormalToDark(duration, false);
                    break;
                case 渐变类型.黑变正常:
                    UIController.ScreenDarkToNormal(duration);
                    break;
                case 渐变类型.正常变白:
                    UIController.ScreenNormalToWhite(duration, false);
                    break;
                case 渐变类型.白变正常:
                    UIController.ScreenWhiteToNormal(duration);
                    break;
            }
            if (waitUntilFinished)
            {
                Utils.GameUtil.DelayFunc(() => Continue(), duration);
            }
            else
            {
                Continue();
            }
        }
    }
}
