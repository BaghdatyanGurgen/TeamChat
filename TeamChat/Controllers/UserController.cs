using Microsoft.AspNetCore.Mvc;
using TeamChat.Application.Abstraction;
using TeamChat.Application.DTOs.User.Request;

namespace TeamChat.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        /// <summary>
        /// Создаёт драфт-пользователя (черновик регистрации)
        /// </summary>
        [HttpPost("create-draft")]
        public async Task<IActionResult> CreateDraftUser([FromBody] CreateDraftUserRequest request)
        {
            var result = await _userService.CreateDraftUserAsync(request);
            return Ok(result);
        }

        /// <summary>
        /// Подтверждение email по токену
        /// </summary>
        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerivyEmailRequest request)
        {
            var result = await _userService.VerifyEmailAsync(request);
            return Ok(result);
        }

        /// <summary>
        /// Завершение регистрации (установка пароля)
        /// </summary>
        [HttpPost("set-password")]
        public async Task<IActionResult> SetPassword([FromQuery] SetPasswordRequest request)
        {
            var result = await _userService.SetPasswordAsync(request);
            return Ok(result);
        }
        /*
        /// <summary>
        /// Получение профиля пользователя
        /// </summary>
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserProfile(Guid userId)
        {
            var result = await _userService.GetUserProfileAsync(userId);
            return Ok(result);
        }*/
    }
}
