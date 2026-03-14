using Microsoft.AspNetCore.Mvc;
using TeamChat.Application.Abstraction.Infrastructure.File;

namespace TeamChat.API.Controllers;

[ApiController]
[Route("api/files")]
public class FileController(IFileService fileService) : BaseController
{
    private readonly IFileService _fileService = fileService;

    [HttpGet("{folder}/{fileName}")]
    public async Task<IActionResult> GetFile(string folder, string fileName)
    {
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