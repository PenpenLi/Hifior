public interface ISaveable
{
    string GetFullRecordPathName();
    T LoadFromDisk<T>();
    void SaveToDisk();
}