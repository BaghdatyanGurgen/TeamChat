using Google.Protobuf;
using Grpc.Core;
using Microsoft.AspNetCore.Http;
using TeamChat.Application.Abstraction.Infrastructure.File;
using TeamChat.Contracts.Grpc;

namespace TeamChat.Infrastructure.File;

public class GrpcFileServiceAdapter(FileService.FileServiceClient client) : IFileService
{
    private readonly FileService.FileServiceClient _client = client;

    public async Task<string> UploadFileAsync(IFormFile file, string folder)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("File is empty");

        const int chunkSize = 1024 * 64;
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

    public async Task<(byte[] Content, string MimeType)> GetFileAsync(string folder, string fileName)
    {
        var request = new GetFileRequest { Folder = folder, FileName = fileName };
        var call = _client.GetFile(request);

        var chunks = new List<byte[]>();
        var mimeType = "application/octet-stream";

        await foreach (var chunk in call.ResponseStream.ReadAllAsync())
        {
            chunks.Add(chunk.Content.ToByteArray());
            if (!string.IsNullOrEmpty(chunk.MimeType))
                mimeType = chunk.MimeType;
        }

        var totalSize = chunks.Sum(c => c.Length);
        var result = new byte[totalSize];
        var offset = 0;
        foreach (var chunk in chunks)
        {
            Buffer.BlockCopy(chunk, 0, result, offset, chunk.Length);
            offset += chunk.Length;
        }

        return (result, mimeType);
    }
}