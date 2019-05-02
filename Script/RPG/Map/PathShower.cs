using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathShower : MonoBehaviour
{
    public enum EPathShowerType
    {
        Damage,
        Move,
        Heal,
        Both,
        HighLight
    }
    [Tooltip("将所有的SpriteRenderer的SortOrder设置为255")]
    public Transform[] ShowerTransform;
    Transform GetTransformRoot(EPathShowerType t) { return ShowerTransform[(int)t]; }

    public void Clear()
    {
        SetRootVisible(true);
        HideAll();
    }

    GameObject GetFirstPathShower(EPathShowerType t) { return GetTransformRoot(t).GetChild(0).gameObject; }
    public void SetVisible(EPathShowerType t, bool show)
    {
        GetTransformRoot(t).gameObject.SetActive(show);
        bVisible = true;
    }
    public void SetRootVisible(bool show)
    {
        gameObject.SetActive(show);
    }
    bool bVisible = false;
    public bool IsRangeVisible()
    {
        return bVisible;
    }
    public void HideAll()
    {
        foreach (Transform v in ShowerTransform)
        {
            v.gameObject.SetActive(false);
        }
        bVisible = false;
    }
    public void HidePath(EPathShowerType t)
    {
        ShowerTransform[(int)t].gameObject.SetActive(false);
    }
    public void ShowHighLightTiles(List<Vector2Int> pos)
    {
        gameObject.SetActive(true);
        HidePath(EPathShowerType.HighLight);
        ShowTiles(EPathShowerType.HighLight, pos, true, false);
    }
    public void ShowTalkCharacterTiles(List<Vector2Int> pos)
    {
        gameObject.SetActive(true);
        HidePath(EPathShowerType.Heal);
        ShowTiles(EPathShowerType.Heal, pos, true, false);
    }
    public void ShowTiles(EPathShowerType t, List<Vector2Int> pos, bool showNow = true, bool hideOther = true)
    {
        gameObject.SetActive(true);
        var transRoot = GetTransformRoot(t);
        int tileCount = transRoot.childCount;
        int posCount = pos.Count;
        int needCount = posCount - tileCount;
        if (needCount > 0)
        {
            for (int i = 0; i < needCount; i++)
            {
                GameObject newObj = GameObject.Instantiate(GetFirstPathShower(t));
                newObj.transform.SetParent(transRoot, false);
            }
        }
        int iter = 0;
        foreach (Transform v in transRoot)
        {
            if (iter >= posCount)
            {
                v.gameObject.SetActive(false);
            }
            else
            {
                v.localPosition = PositionMath.TilePositionToTileLocalPosition(pos[iter]);
                v.gameObject.SetActive(true);
            }
            iter++;
        }
        if (hideOther)
        {
            HideAll();
        }
        if (showNow)
        {
            SetVisible(t, true);
        }
    }
    /*void Start()
    {
        ShowTiles(EPathShowerType.Damage, new List<Vector2Int> { new Vector2Int(0, 1), new Vector2Int(6, 5), new Vector2Int(1, 2), new Vector2Int(2, 2) });
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            ShowTiles(EPathShowerType.Damage, new List<Vector2Int> {
                new Vector2Int(0, 1), new Vector2Int(6, 5), new Vector2Int(1, 2), new Vector2Int(2, 2),
                new Vector2Int(1, 1), new Vector2Int(2, 5), new Vector2Int(4, 2), new Vector2Int(3, 2),
            });
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            SetVisible(EPathShowerType.Damage, false);
            ShowTiles(EPathShowerType.Heal, new List<Vector2Int> {
                new Vector2Int(0, 1)
            });
        }
    }*/
}
