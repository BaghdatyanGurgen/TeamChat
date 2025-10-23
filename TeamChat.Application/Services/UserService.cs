using TeamChat.Domain.Entities;
using TeamChat.Application.DTOs;
using TeamChat.Domain.Interfaces;
using TeamChat.Infrastructure.Email;
using TeamChat.Application.Abstraction;
using TeamChat.Application.DTOs.User.Request;
using TeamChat.Domain.Models.Exceptions.User;
using TeamChat.Application.DTOs.User.Responses;
using TeamChat.Application.Validation;

namespace TeamChat.Application.Services
{
    public class UserService(IUserRepository userRepository, IEmailSender emailSender) : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IEmailSender _emailSender = emailSender;

        public async Task<ResponseModel<RegisterEmailResponse>> CreateDraftUserAsync(CreateDraftUserRequest request)
        {
            if (!await _userRepository.IsEmailAvailableAsync(request.Email))
                throw new InvalidEmailException();

            if (!UserValidation.IsValidEmail(request.Email))
                throw new InvalidEmailException();

            var user = new User
            {
                Email = request.Email,
                EmailConfirmed = false,
                CreatedAt = DateTime.UtcNow,
                EmailConfirmationCode = Guid.NewGuid().ToString()
            };

            await _userRepository.AddAsync(user);
            var verificationLink = _emailSender.BuildVerificationLink(user.Id, user.EmailConfirmationCode);
            await _emailSender.SendEmailAsync(user.Email, "Подтверждение регистрации",
                $"Для завершения регистрации перейдите по ссылке: {verificationLink}");

            var response = new RegisterEmailResponse
            {
                UserId = user.Id,
                Email = user.Email
            };

            return ResponseModel<RegisterEmailResponse>.Success(response, "Письмо с подтверждением отправлено на ваш email");
        }

        public async Task<ResponseModel<VerifyEmailResponse>> VerifyEmailAsync(VerivyEmailRequest request)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId)
                ?? throw new UserNotFoundException();

            if (user.EmailConfirmed)
                throw new InvalidEmailException();

            if (user.EmailConfirmationCode != request.Token)
                throw new InvalidTokenException();

            user.EmailConfirmed = true;
            user.EmailConfirmationCode = string.Empty;
            await _userRepository.UpdateAsync(user);

            var response = new VerifyEmailResponse
            {
                UserId = user.Id
            };

            return ResponseModel<VerifyEmailResponse>.Success(response, "Email успешно подтверждён.");
        }

        public async Task<ResponseModel<SetPasswordResponse>> SetPasswordAsync(SetPasswordRequest request)
        {
            if (!UserValidation.IsValidPassword(request.Password))
                throw new InvalidPasswordException();

            var setPasswordResult = await _userRepository.SetPassword(request.UserId, request.Password);

            var response = new SetPasswordResponse
            {
                UserId = setPasswordResult
            };
            return ResponseModel<SetPasswordResponse>.Success(response, "Пароль успешно установлен.");
        }
    }
}
