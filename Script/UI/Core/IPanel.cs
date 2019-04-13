using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace RPG.UI
{
    [RequireComponent(typeof(Image))]
    public abstract class IPanel : AbstractUI
    {
        [Header("IPanel 基类参数")]
        public Image Background;
        public bool ShowAtStart = false;
        
        protected override void Awake()
        {
            base.Awake();

            Background = GetComponent<Image>();
            if (!ShowAtStart)
                Hide();
        }

    }
}
