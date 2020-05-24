using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class AnimationManager:ManagerBase
{
   private void LoadActionAnimation()
    {
        Attribute.GetCustomAttributes(typeof(AnimationTypeAttribute));
    }
}