public interface ISaveLoad
{
    string GetFullRecordPathName();
    T LoadFromDisk<T>();
    void SaveToDisk();
}