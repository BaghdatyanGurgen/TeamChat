using Google.Protobuf;
using TeamChat.Contracts.Grpc;
using Microsoft.AspNetCore.Http;
using TeamChat.Application.Abstraction.Infrastructure.File;

namespace TeamChat.Infrastructure.File;

public class GrpcFileServiceAdapter(FileService.FileServiceClient client) : IFileService
{
    private readonly FileService.FileServiceClient _client = client;

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