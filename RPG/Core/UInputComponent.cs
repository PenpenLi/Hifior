using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public enum InputActionType
{
    IE_Clicked,
    IE_Pressed,
    IE_Released
}
public class UInputComponent
{
    public string Name
    {
        get;
        private set;
    }
    private Dictionary<string, UnityAction> InputClickedAction;
    private Dictionary<string, UnityAction> InputPressedAction;
    private Dictionary<string, UnityAction> InputReleasedAction;
    private Dictionary<string, UnityAction<float>> InputAxis;
    public UInputComponent(string InputName)
    {
        Name = InputName;
        InputClickedAction = new Dictionary<string, UnityAction>();
        InputPressedAction = new Dictionary<string, UnityAction>();
        InputReleasedAction = new Dictionary<string, UnityAction>();
        InputAxis = new Dictionary<string, UnityAction<float>>();
    }
    ~UInputComponent()
    {
        Clear();
    }
    public void Clear()
    {
        InputClickedAction.Clear();
        InputPressedAction.Clear();
        InputReleasedAction.Clear();
        InputAxis.Clear();
    }
    public void BindAction(string KeyName, InputActionType ActionType, UnityAction ActionDelegate)
    {
        switch (ActionType)
        {
            case InputActionType.IE_Clicked:
                InputClickedAction.Add(KeyName, ActionDelegate);
                break;
            case InputActionType.IE_Pressed:
                InputPressedAction.Add(KeyName, ActionDelegate);
                break;
            case InputActionType.IE_Released:
                InputReleasedAction.Add(KeyName, ActionDelegate);
                break;
        }
    }
    public void BindAxis(string KeyName, UnityAction<float> ActionDelegate)
    {
        InputAxis.Add(KeyName, ActionDelegate);
    }
    public void TickInput()
    {
        foreach (string KeyAction in InputClickedAction.Keys)
        {
            if (Input.GetButton(KeyAction))
                InputClickedAction[KeyAction]();
        }
        foreach (string KeyAction in InputPressedAction.Keys)
        {
            if (Input.GetButtonDown(KeyAction))
                InputPressedAction[KeyAction]();
        }
        foreach (string KeyAction in InputReleasedAction.Keys)
        {
            if (Input.GetButtonUp(KeyAction))
                InputReleasedAction[KeyAction]();
        }
        foreach (string KeyAxis in InputAxis.Keys)
        {
            float axis = Input.GetAxis(KeyAxis);
            InputAxis[KeyAxis](axis);
        }
    }
}