namespace Infrastructure.Contracts
{
    public interface IMediaStorageService
    {
        Task<List<string>> GetAllFiles();
        Task<string> UploadFile(Stream mediaStream, string fileName, string contentType);
        Task<string> ReplaceFile(Stream mediaStream, string fileName, string contentType, string FileToReplaceName);
        Task<bool> DeleteFile(string fileName);
    }
}
