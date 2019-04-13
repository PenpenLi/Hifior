using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerBase
{
    public GameMode gameMode
    {
        get { return GameMode.Instance; }
    }
    public BattleManager battleManager { get { return gameMode.BattleManager; } }
    public InputManager inputManager { get { return gameMode.InputManager; } }
    public UIManager uiManager { get { return gameMode.UIManager; } }
    public ChapterManager chapterManager { get { return gameMode.ChapterManager; } }
    public virtual void Init() { }
    public virtual void Update() { }
    public virtual void Destroy() { }
}
