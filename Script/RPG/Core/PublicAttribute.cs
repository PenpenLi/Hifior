using System;
using UnityEngine;
/// <summary>
/// 用于在Hierarchy视图中绘制图标
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class HierarchyIconAttribute : Attribute
{
    public string TextureFileName { get; private set; }
    public int Position { get; private set; }
    public HierarchyIconAttribute(string TexturePath, int Position = 0)
    {
        this.TextureFileName = TexturePath;
        this.Position = Position;
    }
}
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public class GUIContentAttribute : Attribute
{
    public GUIContentAttribute(string labelName, string tooltip=null)
    {
        this.GUIContent = new GUIContent(labelName, tooltip);
    }
    public GUIContent GUIContent { get; set; }
}
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public class IntSliderAttribute : Attribute
{
    public IntSliderAttribute(int min,int max)
    {
        this.Min = min;
        this.Max = max;
    }
    public int Min  { get; set; }
    public int Max { get; set; }
}