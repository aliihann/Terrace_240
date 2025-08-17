namespace Terrace_240.Services
{
    public interface IFileService
    {
        Task<string?> SavePosterAsync(IFormFile file);
        void DeleteIfExists(string relativePath);
    }
}
