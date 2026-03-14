using Microsoft.AspNetCore.Mvc;
using TeamChat.Application.Abstraction.Infrastructure.File;

namespace TeamChat.API.Controllers;

[ApiController]
[Route("api/files")]
public class FileController(IFileService fileService) : BaseController
{
    private readonly IFileService _fileService = fileService;

    [HttpGet("{**path}")]
    public async Task<IActionResult> GetFile(string path)
    {
        var lastSlash = path.LastIndexOf('/');
        if (lastSlash < 0)
            return NotFound();

        var folder = path[..lastSlash];
        var fileName = path[(lastSlash + 1)..];

        if (string.IsNullOrEmpty(folder) || string.IsNullOrEmpty(fileName))
            return NotFound();

        try
        {
            var (content, mimeType) = await _fileService.GetFileAsync(folder, fileName);
            return File(content, mimeType);
        }
        catch
        {
            return NotFound();
        }
    }
}