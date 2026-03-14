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

        const int chunkSize = 1024 * 64; // 64 KB
        long totalChunks = (file.Length + chunkSize - 1) / chunkSize;
        long chunkIndex = 0;

        using var call = _client.UploadFile();

        var stream = call.RequestStream;

        using var fs = file.OpenReadStream();
        var buffer = new byte[chunkSize];
        int bytesRead;

        while ((bytesRead = await fs.ReadAsync(buffer, 0, chunkSize)) > 0)
        {
            var chunk = new UploadChunk
            {
                FileName = file.FileName,
                Folder = folder,
                Content = ByteString.CopyFrom(buffer, 0, bytesRead),
                ChunkIndex = chunkIndex++,
                TotalChunks = totalChunks,
                FileSize = file.Length,
                MimeType = file.ContentType
            };

            await stream.WriteAsync(chunk);
        }

        await stream.CompleteAsync();

        var response = await call.ResponseAsync;

        return response.Url;
    }

}