namespace Terrace_240.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _env;
        public FileService(IWebHostEnvironment env) { _env = env; }

        public async Task<string?> SavePosterAsync(IFormFile file)
        {
            if (file == null) return null;
            var uploads = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);
            var ext = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}{ext}";
            var full = Path.Combine(uploads, fileName);
            using (var stream = new FileStream(full, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return $"/uploads/{fileName}";
        }

        public void DeleteIfExists(string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath)) return;
            if (relativePath.StartsWith("/")) relativePath = relativePath.Substring(1);
            var full = Path.Combine(_env.WebRootPath, relativePath.Replace("/", Path.DirectorySeparatorChar.ToString()));
            if (File.Exists(full)) File.Delete(full);
        }
    }
}
