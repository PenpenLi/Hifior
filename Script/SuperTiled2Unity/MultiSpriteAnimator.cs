using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiSpriteAnimator : MonoBehaviour
{
    public Sprite[] sprites;
    public float switchTime;
    int sizeOfSprites;
    public enum EAnimateType
    {
        Stay,
        Move,
        Count
    }
    const int countOfType = (int)EAnimateType.Count;
    public EAnimateType activeAnimateType;
    float[] ConstSwitchTime;
    List<Sprite[]> staySprites;
    int fromFrame, endFrame;
    public void SetAnimatorContent(EAnimateType t, Sprite[] sp)
    {
        if (staySprites == null || staySprites.Count < countOfType)
            staySprites = new List<Sprite[]>((int)EAnimateType.Count);
        for (int i = 0; i < countOfType; i++)
        {
            staySprites.Add(null);
        }
        staySprites[(int)t] = sp;
    }
    public void SetActiveAnimator(EAnimateType t)
    {
        activeAnimateType = t;
        int index = (int)t;
        switchTime = ConstSwitchTime[index];
        sprites = staySprites[index];
        if (t == EAnimateType.Stay)
        {
            SetActiveRangeAll();
        }
    }
    public void SetActiveRangeAll()
    {
        Sprite[] sp = staySprites[(int)(EAnimateType.Stay)];
        SetActiveRange(0, sp.Length);
    }
    public void SetActiveRange(int from, int end)
    {
        fromFrame = from;
        endFrame = end;
    }
    SpriteRenderer render;
    void Awake()
    {
        render = GetComponent<SpriteRenderer>();
        if (switchTime < 0.001f) switchTime = 0.1f;
        ConstSwitchTime = new float[countOfType] { 10 / Application.targetFrameRate, 4 / Application.targetFrameRate };
    }
    // Update is called once per frame
    void Update()
    {
        sizeOfSprites = fromFrame - endFrame + 1;
        int x = (int)(Time.time / switchTime) % sizeOfSprites;
        render.sprite = sprites[fromFrame + x];
    }
}
