using Microsoft.AspNetCore.Http;
using TeamChat.Application.DTOs;
using TeamChat.Application.DTOs.User;

namespace TeamChat.Application.Abstraction.Services;

public interface IUserService
{
    public Task<ResponseModel<RegisterEmailResponse>> CreateDraftUserAsync(CreateDraftUserRequest request);
    public Task<ResponseModel<VerifyEmailResponse>> VerifyEmailAsync(VerifyEmailRequest request);
    public Task<ResponseModel<SetPasswordResponse>> SetPasswordAsync(SetPasswordRequest request);
    public Task<ResponseModel<UserProfileResponse>> SetUserProfileAsync(Guid userId, SetUserProfileRequest request);
    public Task<ResponseModel<AuthResponse>> LoginAsync(LoginRequest request);
    Task<ResponseModel<AuthResponse>> RefreshTokenAsync(string token, string refreshToken);
    Task<ResponseModel<string>> LogoutAsync(Guid userGuidId);
    Task<ResponseModel<UserProfileResponse>> GetUserProfileAsync(Guid userId);
    Task<ResponseModel> ChangePasswordAsync(Guid userId, string oldPassword, string newPassword); 
    Task<ResponseModel> ChangeEmailAsync(Guid userId, string newEmail);
    Task<ResponseModel<IEnumerable<RoleResponse>>> GetUserRolesAsync(Guid userId);
    Task<ResponseModel<IEnumerable<UserActivityResponse>>> GetUserActivityAsync(Guid userId);
    Task<ResponseModel> SetUserAvatarAsync(Guid userId, IFormFile avatarFile);
}