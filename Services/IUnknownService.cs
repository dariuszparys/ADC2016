namespace DryRun
{
    public interface IUnknownService
    {
        void AddRef();
        string QueryInterface(string key);
        void Release();
    }
}