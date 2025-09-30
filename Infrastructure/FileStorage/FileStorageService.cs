using Microsoft.AspNetCore.Http;

namespace Infrastructure.FileStorage;

public class FileStorageService(string rootPath) : IFileStorageService
{
    public async Task<string> SaveFileAsync(IFormFile file, string relativeFolder)
    {
        try
        {
            var path = Path.Combine(rootPath,"wwwroot",relativeFolder);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var fullPath = Path.Combine(path, fileName);
            await using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);
            return Path.Combine(relativeFolder, fileName).Replace("\\", "/");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw new Exception("Error saving file");
        }
    }

    public Task DeleteFileAsync(string relativePath)
    {
        try
        {
            var path = Path.Combine(rootPath,"wwwroot",relativePath.Replace("/",Path.DirectorySeparatorChar.ToString()));
            if (File.Exists(path)) File.Delete(path);
            return Task.CompletedTask;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw new  Exception("Error deleting file");
        }
    }
}