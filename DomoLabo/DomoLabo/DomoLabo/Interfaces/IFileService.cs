namespace DomoLabo.Interfaces
{
    public interface IFileService
    {
        void CreateFile(string content);
        string ReadFile();
    }
}