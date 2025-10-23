using TeamChat.Application.DTOs;
using TeamChat.Application.DTOs.User.Request;
using TeamChat.Application.DTOs.User.Responses;

namespace TeamChat.Application.Abstraction
{
    public interface IUserService
    {
        public Task<ResponseModel<RegisterEmailResponse>> CreateDraftUserAsync(CreateDraftUserRequest request);
        public Task<ResponseModel<VerifyEmailResponse>> VerifyEmailAsync(VerivyEmailRequest request);
        public Task<ResponseModel<SetPasswordResponse>> SetPasswordAsync(SetPasswordRequest request);
    }
}
