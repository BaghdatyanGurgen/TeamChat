using Microsoft.AspNetCore.Http;

namespace TeamChat.Application.Abstraction.Infrastructure.File;

public interface IFileService
{
    Task<(string Url, string OriginalFileName)> UploadFileAsync(IFormFile file, string folder);
    Task<(byte[] Content, string MimeType)> GetFileAsync(string folder, string fileName);
}