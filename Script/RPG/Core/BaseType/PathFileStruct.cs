using System;
#pragma warning disable 0660 //定义运算符 == 或运算符 !=,但不重写 Object.Equals(object o);

public struct PathFileStruct
{
    public PathFileStruct(string path, string file)
    {
        Path = path;
        File = file;
    }
    public static bool operator ==(PathFileStruct left, PathFileStruct right)
    {
        return string.Equals(left.Path, right.Path, StringComparison.CurrentCultureIgnoreCase) && string.Equals(right.File, right.File, StringComparison.CurrentCultureIgnoreCase);
    }
    public static bool operator !=(PathFileStruct left, PathFileStruct right)
    {
        return !string.Equals(left.Path, right.Path, StringComparison.CurrentCultureIgnoreCase) || !string.Equals(right.File, right.File, StringComparison.CurrentCultureIgnoreCase);
    }
    public static bool Equals(PathFileStruct left, PathFileStruct right)
    {
        return string.Equals(left.Path, right.Path, StringComparison.CurrentCultureIgnoreCase) && string.Equals(right.File, right.File, StringComparison.CurrentCultureIgnoreCase);
    }

    public override int GetHashCode()
    {
        return Path.GetHashCode() + File.GetHashCode();
    }
    public bool IsEmpty() { return string.IsNullOrEmpty(File); }
    public string Path;
    public string File;
}