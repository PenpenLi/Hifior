using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace RPG.UI
{
    [RequireComponent(typeof(Image))]
    public abstract class IPanel : UActor
    {
        private static IPanel m_Panel;

        public Image Background;
        public bool ShowAtStart = false;

        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }

        public static T Instance<T>() where T : IPanel
        {
            if (!m_Panel)
            {
                m_Panel = FindObjectOfType(typeof(T)) as T;
                if (!m_Panel)
                    Debug.LogError("场景中未找到类型为" + typeof(T).GetType().ToString() + "激活的物体");
            }

            return m_Panel as T;
        }

        public override void BeginPlay()
        {
            Background = GetComponent<Image>();
            if (!ShowAtStart)
                Hide();
        }

        public virtual void SetupUIInputComponent()
        {
            if (InputComponent == null)
            {
                InputComponent = new UInputComponent(this, "UI_InputComponent0");
            }
        }
    }
}
