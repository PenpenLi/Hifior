using UnityEngine;

[System.Serializable]
public class SelectEffectRangeType
{
    public EnumSelectEffectRangeType SelectType;
    public EnumSelectEffectRangeType EffectType;
    public Vector2Int SelectRange;
    public Vector2Int EffectRange;
    public SelectEffectRangeType()
    {
        SelectRange = Vector2Int.one;
        EffectRange = Vector2Int.zero;
    }
}
