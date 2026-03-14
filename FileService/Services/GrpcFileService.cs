using Grpc.Core;
using TeamChat.Contracts.Grpc;
using Google.Protobuf.WellKnownTypes;

namespace FileService.Services;

public class GrpcFileService : TeamChat.Contracts.Grpc.FileService.FileServiceBase
{
    private readonly string _basePath = Path.Combine(AppContext.BaseDirectory, "uploads");

    public override async Task<UploadFileResponse> UploadFile(IAsyncStreamReader<UploadChunk> requestStream, ServerCallContext context)
    {
        UploadChunk? firstChunk = null;
        string? folderPath = null;
        string? newFileName = null;
        string? mimeType = null;
        long totalSize = 0;

        var tempFilePath = Path.GetTempFileName();

        await using (var fs = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write))
        {
            await foreach (var chunk in requestStream.ReadAllAsync())
            {
                if (firstChunk == null)
                {
                    firstChunk = chunk;
                    folderPath = Path.Combine(_basePath, chunk.Folder);
                    Directory.CreateDirectory(folderPath);

                    newFileName = $"{Guid.NewGuid()}{Path.GetExtension(chunk.FileName)}";
                    mimeType = chunk.MimeType;
                }

                var content = chunk.Content.ToByteArray();
                await fs.WriteAsync(content, 0, content.Length);
                totalSize += content.Length;
            }
        }

        if (firstChunk == null || folderPath == null || newFileName == null)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "No data received"));

        var finalPath = Path.Combine(folderPath, newFileName);
        File.Move(tempFilePath, finalPath, overwrite: true);

        return new UploadFileResponse
        {
            Url = $"/uploads/{firstChunk.Folder}/{newFileName}",
            FileName = firstChunk.FileName,
            FileSize = totalSize,
            MimeType = mimeType ?? "application/octet-stream",
            UploadedAt = Timestamp.FromDateTime(DateTime.UtcNow)
        };
    }

    public override async Task GetFile(GetFileRequest request, IServerStreamWriter<FileChunk> responseStream, ServerCallContext context)
    {
        var folderPath = Path.Combine(_basePath, request.Folder);
        var filePath = Path.Combine(folderPath, request.FileName);

        if (!File.Exists(filePath))
            throw new RpcException(new Status(StatusCode.NotFound, "File not found"));

        const int chunkSize = 1024 * 64;
        var buffer = new byte[chunkSize];
        var mimeType = "application/octet-stream";

        using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        int bytesRead;
        long chunkIndex = 0;
        long totalChunks = (fs.Length + chunkSize - 1) / chunkSize;

        while ((bytesRead = await fs.ReadAsync(buffer, 0, chunkSize)) > 0)
        {
            var chunk = new FileChunk
            {
                Content = Google.Protobuf.ByteString.CopyFrom(buffer, 0, bytesRead),
                ChunkIndex = chunkIndex++,
                TotalChunks = totalChunks,
                MimeType = mimeType
            };

            await responseStream.WriteAsync(chunk);
        }
    }

    public override Task<DeleteFileResponse> DeleteFile(DeleteFileRequest request, ServerCallContext context)
    {
        var filePath = Path.Combine(_basePath, request.Url.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

        if (!File.Exists(filePath))
            return Task.FromResult(new DeleteFileResponse { Success = false });

        File.Delete(filePath);
        return Task.FromResult(new DeleteFileResponse { Success = true });
    }
}