using Grpc.Core;
using TeamChat.Contracts.Grpc;

namespace FileService.Services;

public class GrpcFileService : TeamChat.Contracts.Grpc.FileService.FileServiceBase
{
    public override async Task<UploadFileResponse> UploadFile(UploadFileRequest request, ServerCallContext context)
    {
        var folderPath = Path.Combine("uploads", request.Folder);
        Directory.CreateDirectory(folderPath);

        var newName = $"{Guid.NewGuid()}{Path.GetExtension(request.FileName)}";
        var filePath = Path.Combine(folderPath, newName);

        await File.WriteAllBytesAsync(filePath, request.Content.ToByteArray());

        return new UploadFileResponse
        {
            Url = $"/uploads/{request.Folder}/{newName}"
        };
    }
}