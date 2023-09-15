namespace Services.StorageSystem.Interface
{
    public interface IStorageService
    {
        void ClearData();
        void SetData(string key, string value);
        string GetData(string key, string defaultValue = "-1");
    }
}