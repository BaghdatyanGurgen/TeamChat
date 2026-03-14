using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using TeamChat.Domain.Models.Exceptions;

namespace TeamChat.API.Controllers;

public abstract class BaseController : ControllerBase
{
    protected Guid CurrentUserId =>
        Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var id)
            ? id
            : throw new UserNotFoundException();
}