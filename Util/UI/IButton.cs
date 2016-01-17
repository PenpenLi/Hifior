using UnityEngine;
using UnityEngine.UI;
public abstract class IButton : MonoBehaviour
{
    public void OnDestroy()
    {
        UnRegister(GetComponent<Button>());
    }

    /// <summary>
    /// 抽象函数，用于按钮点击后执行的事件
    /// </summary>
    public abstract void OnClick();
    /// <summary>
    /// 注册到OnClick事件
    /// </summary>
    /// <param name="button"></param>
    public void Register(Button button)
    {
        if (button == null) { }
        // Log.Write("空的Button引用");
        else
            button.onClick.AddListener(OnClick);
    }
    public void UnRegister(Button button)
    {
        if (button != null)
            button.onClick.RemoveAllListeners();
    }
    public virtual void Awake()
    {
        Register(GetComponent<UnityEngine.UI.Button>());
    }
}
