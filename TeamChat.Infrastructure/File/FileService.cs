/*using Microsoft.AspNetCore.Http;
using TeamChat.Application.Abstraction.Infrastructure.File;

namespace TeamChat.Infrastructure.File;

public class FileService : IFileService
{
    private readonly string _basePath;

    public FileService(string basePath)
    {
        _basePath = Path.Combine(basePath, "uploads");
    }

    public async Task<string> UploadFileAsync(IFormFile file, string folder)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("File is empty");

        var folderPath = Path.Combine(_basePath, folder);
        Directory.CreateDirectory(folderPath);

        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var filePath = Path.Combine(folderPath, fileName);

        using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        return $"/uploads/{folder}/{fileName}";
    }
}
*/
using Google.Protobuf;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Http;
using TeamChat.Application.Abstraction.Infrastructure.File;
using TeamChat.Contracts.Grpc;

namespace TeamChat.Infrastructure.File;

public class GrpcFileServiceAdapter : IFileService
{
    private readonly FileService.FileServiceClient _client;

    public GrpcFileServiceAdapter(FileService.FileServiceClient client)
    {
        _client = client;
    }

    public async Task<string> UploadFileAsync(IFormFile file, string folder)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("File is empty");

        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);
        var bytes = ms.ToArray();

        var request = new UploadFileRequest
        {
            FileName = file.FileName,
            Folder = folder,
            Content = ByteString.CopyFrom(bytes)
        };

        var response = await _client.UploadFileAsync(request);

        return response.Url;
    }
}
