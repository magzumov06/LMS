using Microsoft.AspNetCore.Http;

namespace Infrastructure.FileStorage;

public interface IFileStorageService
{
    Task<string> SaveFileAsync(IFormFile file, string relativeFolder);
    Task DeleteFileAsync(string relativePath);
}