using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class UInputComponent
{
    public static bool A_Clicked
    {
        get {
            return Input.GetButton("A");
    }
    }
    public static bool B_Clicked
    {
        get
        {
            return Input.GetButton("B");
        }
    }
    public static bool X_Clicked
    {
        get
        {
            return Input.GetButton("X");
        }
    }
    public static bool Y_Clicked
    {
        get
        {
            return Input.GetButton("Y");
        }
    }
    public static bool Start_Clicked
    {
        get
        {
            return Input.GetButton("Start");
        }
    }
    public string Name
    {
        get;
        private set;
    }
    public int Priority;
    public bool bBlockInput;

    private string LastClickAction;
    private int ClickInterval;
    private int FirstClickInterval;
    private int TempIntervalCount;
    private int ActionRepeatCount;
    UActor Actor;
    private Dictionary<string, UnityAction> InputClickedAction;
    private Dictionary<string, UnityAction> InputPressedAction;
    private Dictionary<string, UnityAction> InputReleasedAction;
    private Dictionary<string, UnityAction<float>> InputAxis;

    public UInputComponent(UActor InActor, string InputName)
    {
        Actor = InActor;
        Name = InputName;
        bBlockInput = false;
        InputClickedAction = new Dictionary<string, UnityAction>();
        InputPressedAction = new Dictionary<string, UnityAction>();
        InputReleasedAction = new Dictionary<string, UnityAction>();
        InputAxis = new Dictionary<string, UnityAction<float>>();

        LastClickAction = "";
        ClickInterval = 10;
        FirstClickInterval = 20;
        TempIntervalCount = 0;
        ActionRepeatCount = 0;
    }

    ~UInputComponent()
    {
        ClearBindingValues();
    }

    public void ClearBindingValues()
    {
        InputClickedAction.Clear();
        InputPressedAction.Clear();
        InputReleasedAction.Clear();
        InputAxis.Clear();
    }

    public bool HasBindings()
    {
        return (InputClickedAction.Count > 0
            || InputPressedAction.Count > 0
            || InputReleasedAction.Count > 0
            || InputAxis.Count > 0);
    }

    public float GetAxisValue(string AxisName)
    {
        return Input.GetAxis(AxisName);
    }

    public void BindAction(string KeyName, EInputActionType ActionType, UnityAction ActionDelegate)
    {
        switch (ActionType)
        {
            case EInputActionType.IE_Clicked:
                InputClickedAction.Add(KeyName, ActionDelegate);
                break;
            case EInputActionType.IE_Pressed:
                InputPressedAction.Add(KeyName, ActionDelegate);
                break;
            case EInputActionType.IE_Released:
                InputReleasedAction.Add(KeyName, ActionDelegate);
                break;
        }
    }

    public void BindAxis(string KeyName, UnityAction<float> ActionDelegate)
    {
        InputAxis.Add(KeyName, ActionDelegate);
    }

    public void TickPlayerInput()
    {
        if (bBlockInput)
            return;
        foreach (string KeyAction in InputClickedAction.Keys)
        {
            if (Input.GetButton(KeyAction))
            {
                TempIntervalCount++;
                if (LastClickAction == KeyAction)
                {
                    if (TempIntervalCount > FirstClickInterval * (ActionRepeatCount > 0 ? 1 : 0) + ActionRepeatCount * ClickInterval)
                    {
                        ActionRepeatCount++;
                        InputClickedAction[KeyAction]();
                    }
                }
                else
                {
                    ActionRepeatCount++;
                    InputClickedAction[KeyAction]();
                }
                LastClickAction = KeyAction;
            }
            if (Input.GetButtonUp(KeyAction))
            {
                ActionRepeatCount = 0;
                LastClickAction = "";
                TempIntervalCount = 0;
            }
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

    UnityAction GetActionBinding(EInputActionType ActionType, string ActionName)
    {
        switch (ActionType)
        {
            case EInputActionType.IE_Clicked:
                if (InputClickedAction.ContainsKey(ActionName))
                    return InputClickedAction[ActionName];
                break;
            case EInputActionType.IE_Pressed:
                if (InputPressedAction.ContainsKey(ActionName))
                    return InputPressedAction[ActionName];
                break;
            case EInputActionType.IE_Released:
                if (InputReleasedAction.ContainsKey(ActionName))
                    return InputReleasedAction[ActionName];
                break;
        }
        return null;
    }
}