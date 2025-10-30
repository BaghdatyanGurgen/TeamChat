using TeamChat.Application.DTOs;
using TeamChat.Application.DTOs.User;

namespace TeamChat.Application.Abstraction.Services;

public interface IUserService
{
    public Task<ResponseModel<RegisterEmailResponse>> CreateDraftUserAsync(CreateDraftUserRequest request);
    public Task<ResponseModel<VerifyEmailResponse>> VerifyEmailAsync(VerivyEmailRequest request);
    public Task<ResponseModel<SetPasswordResponse>> SetPasswordAsync(SetPasswordRequest request);
    public Task<ResponseModel<UserProfileResponse>> SetUserProfileAsync(Guid userId, SetUserProfileRequest request);
    public Task<ResponseModel<AuthoResponse>> LoginAsync(LoginRequest request); 
}