using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.UI;

public class UIManager
{
    public UI_BattleTileInfo BattleTileInfo { private set; get; }
    private T FindPanelInChildren<T>(Transform t) where T : IPanel
    {
        T r = null;
        foreach(Transform i in t)
        {
            r = i.GetComponent<T>();
            if (r != null) break;
        }
        if (r == null) Debug.LogError(typeof(T).Name + "is not find under " + t.name);
        return r;
    }
    public void InitBattleUI(Transform panelParent)
    {
        BattleTileInfo = FindPanelInChildren<UI_BattleTileInfo>(panelParent);
    }
    public void InitMainUI(Transform panelParent)
    {
        BattleTileInfo = FindPanelInChildren<UI_BattleTileInfo>(panelParent);
    }
    public void InitPropertyUI(Transform panelParent)
    {
        BattleTileInfo = FindPanelInChildren<UI_BattleTileInfo>(panelParent);
    }
    public void InitSettingUI(Transform panelParent)
    {
        BattleTileInfo = FindPanelInChildren<UI_BattleTileInfo>(panelParent);
    }
}
