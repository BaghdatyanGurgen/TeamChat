using Microsoft.AspNetCore.Mvc;
using TeamChat.Application.DTOs;

namespace TeamChat.API.Extensions;

public static class ResponseExtensions
{
    public static IActionResult ToActionResult<T>(this ResponseModel<T> response)
        => response.IsSuccess ? 
        new OkObjectResult(response) : 
        new BadRequestObjectResult(response.Message);
    public static IActionResult ToActionResult(this ResponseModel response)
        => response.IsSuccess ? 
        new OkObjectResult(response) : 
        new BadRequestObjectResult(response.Message);
}