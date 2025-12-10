using Microsoft.AspNetCore.Http;

namespace TeamChat.Application.Abstraction.Infrastructure.File;

public interface IFileService
{
    Task<string> UploadFileAsync(IFormFile file, string folder);
}