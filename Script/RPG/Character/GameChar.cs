using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameChar : MonoBehaviour//保存运行时的状态
{
    protected readonly Color GAMECHAR_DEFAULT_COLOR = new Color(1.0f, 1.0f, 1.0f);

    public int CharID;//需要新建的人物ID
    public CharacterAttribute attribute;//人物属性
    protected Animator anim;
    protected bool isRunning = false;//是否在移动中

    protected int DamageCount = 0;//收到伤害和造成伤害的次数

    protected VInt2 tileCoords;

    public bool Running
    {
        get { return isRunning; }
        set { isRunning = value; }
    }
    void Update()
    {
        /*if ((int)SLGLevel.battle_State > 2)
        {
            if (HPbar.activeSelf)
                HPbar.SetActive(false);
        }
        else//检测人物是否在移动，不移动不执行
        {
            if (!HPbar.activeSelf)
                HPbar.SetActive(true);
            Vector3 worldPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            Vector2 position = Camera.main.WorldToScreenPoint(worldPosition);
            //得到真实NPC头顶的2D坐标
            HPBarRect.position=new Vector3(position.x, position.y - 8f, 0f);
            HPBarChildRect.sizeDelta = new Vector2(((attribute.CurHP / 1.0f) / attribute.MaxHP) * HPBarRect.sizeDelta.x, HPBarRect.sizeDelta.y);
            HPBarChildRect.position.Set(0.0f, 0.0f, 0.0f);
        }*/
    }
    public bool IsAnimating()
    {
        return GetComponent<Animation>().isPlaying;
    }
    
    public void changeGameCharColor(Color c)
    {
        SkinnedMeshRenderer[] smr = GetComponentsInChildren<SkinnedMeshRenderer>();
        if (smr == null)
            return;
        foreach (SkinnedMeshRenderer sr in smr)
        {
            sr.material.color = c;
        }
    }
}