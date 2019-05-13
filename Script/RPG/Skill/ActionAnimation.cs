using System;
using System.Collections.Generic;
using UnityEngine;
public enum EAnimationType
{
    治疗,
    解毒,
    火球,
}
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class AnimationTypeAttribute : Attribute
{
    public AnimationTypeAttribute(string labelName, EAnimationType type)
    {
        AnimationName = labelName;
        AnimationType = type;
    }
    public EAnimationType AnimationType { get; set; }
    public string AnimationName { get; set; }
}
public abstract class ActionAnimation
{
    public readonly string RESOURCE_ROOT = "animation/action/";
    public EAnimationType GetAnimationType()
    {
        var attr = (AnimationTypeAttribute)Attribute.GetCustomAttribute(GetType(), typeof(AnimationTypeAttribute));
        return attr.AnimationType;
    }
    public string GetAnimationName()
    {
        var attr = (AnimationTypeAttribute)Attribute.GetCustomAttribute(GetType(), typeof(AnimationTypeAttribute));
        return attr.AnimationName;
    }
    protected virtual void Instantiate()
    {
        prefab = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(RESOURCE_ROOT + GetAnimationName()));
    }
    public virtual void InitResource()
    {
        if (hasInit) return;
        Instantiate();
    }
    public void SetTilePos(Vector2Int tilePos)
    {

    }
    public void SetWorldPos(Vector3 worldPos)
    {

    }
    public abstract void Animate();
    protected bool hasInit;
    protected bool isPlaying;
    protected GameObject prefab;
}
