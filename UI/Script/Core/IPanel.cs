using UnityEngine;
using System.Collections;
using UnityEngine.UI;
[RequireComponent(typeof(Image))]
public abstract class IPanel : UActor
{
    public Image Background;
    public bool bShowAtStart=false;
    public virtual void Show()
    {
        gameObject.SetActive(true);
    }
    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
    public IPanel GetPanel() 
    {
        return this;
    }
    public override void BeginPlay()
    {
        Background = GetComponent<Image>();
        if (!bShowAtStart)
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
