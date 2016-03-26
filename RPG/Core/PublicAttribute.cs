using System;
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