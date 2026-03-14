using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Data;
using TeamChat.Application.Abstraction.Infrastructure.Email;
using TeamChat.Application.Abstraction.Infrastructure.File;
using TeamChat.Application.Abstraction.Infrastructure.Repositories;
using TeamChat.Application.Abstraction.Infrastructure.Security;
using TeamChat.Application.Abstraction.Services;
using TeamChat.Application.DTOs;
using TeamChat.Application.DTOs.Chat;
using TeamChat.Application.DTOs.Team;
using TeamChat.Application.DTOs.User;
using TeamChat.Application.Validation;
using TeamChat.Domain.Entities;
using TeamChat.Domain.Models.Exceptions;

namespace TeamChat.Application.Services;

public class UserService(IUserRepository userRepository,
                         IChatMemberRepository chatMemberRepository, 
                         IChatRepository chatRepository,
                         ITeamRepository teamRepository,
                         IEmailSender emailSender,
                         IRefreshTokenService refreshTokenService,
                         IFileService fileService,
                         IJwtTokenService jwtTokenService) : IUserService
{
    private readonly IEmailSender _emailSender = emailSender;
    private readonly IRefreshTokenService _refreshTokenService = refreshTokenService;
    private readonly IJwtTokenService _jwtTokenService = jwtTokenService;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IChatMemberRepository _chatMemberRepository = chatMemberRepository;
    private readonly IChatRepository _chatRepository = chatRepository;
    private readonly ITeamRepository _teamRepository = teamRepository;
    private readonly IFileService _fileService = fileService;

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

        var token = user.EmailConfirmationCode;
        await _userRepository.AddAsync(user);
        var verificationLink = _emailSender.BuildVerificationLink(user.Id, ref token);
        await _emailSender.SendEmailAsync(user.Email, "Confirmation",
            $"Compleat registration: {verificationLink}");

        var response = new RegisterEmailResponse(user.Id, user.Email, token!);

        return ResponseModel<RegisterEmailResponse>.Success(response, "Mail send");
    }

    public async Task<ResponseModel<VerifyEmailResponse>> VerifyEmailAsync(VerifyEmailRequest request)
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

        var response = new VerifyEmailResponse(user.Id);

        return ResponseModel<VerifyEmailResponse>.Success(response, "Mail confirmed.");
    }

    public async Task<ResponseModel<SetPasswordResponse>> SetPasswordAsync(SetPasswordRequest request)
    {
        if (!UserValidation.IsValidPassword(request.Password))
            throw new InvalidPasswordException();

        var user = await _userRepository.SetPassword(request.UserId, request.Password);
        var token = _jwtTokenService.GenerateToken(user);
        var refreshToken = await _refreshTokenService.CreateAsync(request.UserId);

        var response = new SetPasswordResponse(user.Id, token, refreshToken.PlainToken);

        return ResponseModel<SetPasswordResponse>.Success(response);
    }

    public async Task<ResponseModel<UserProfileResponse>> SetUserProfileAsync(Guid userId, SetUserProfileRequest request)
    {
        var user = await _userRepository.GetByIdAsync(userId)
            ?? throw new UserNotFoundException();

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;

        await _userRepository.UpdateAsync(user);

        var response = new UserProfileResponse(user.Id, user.Email, user.FirstName, user.LastName, user.AvatarUrl, user.CreatedAt);

        return ResponseModel<UserProfileResponse>.Success(response);
    }

    public async Task<ResponseModel<AuthResponse>> LoginAsync(LoginRequest request)
    {
        if (!UserValidation.IsValidEmail(request.Email))
            throw new InvalidEmailException();
        if (!UserValidation.IsValidPassword(request.Password))
            throw new InvalidPasswordException();

        var user = await _userRepository.GetByEmailAndPasswordAsync(request.Email, request.Password);

        var jwtToken = _jwtTokenService.GenerateToken(user);

        var refreshToken = await _refreshTokenService.CreateAsync(user.Id);

        var profile = new UserProfileResponse(user.Id, user.Email, user.FirstName, user.LastName, user.AvatarUrl, user.CreatedAt);
        var response = new AuthResponse(profile, jwtToken, refreshToken.PlainToken);

        return ResponseModel<AuthResponse>.Success(response);
    }
    
    public async Task<ResponseModel<AuthResponse>> RefreshTokenAsync(string token, string refreshToken)
    {
        var userId = await _refreshTokenService.ValidateAsync(token, refreshToken);
        if (userId == Guid.Empty)
            return ResponseModel<AuthResponse>.Fail("Invalid token");

        var user = await _userRepository.GetByIdAsync(userId) ?? throw new UserNotFoundException();
        var newJwtToken = _jwtTokenService.GenerateToken(user);
        var newRefreshToken = await _refreshTokenService.CreateAsync(user.Id);

        var profile = new UserProfileResponse(user.Id, user.Email, user.FirstName, user.LastName, user.AvatarUrl, user.CreatedAt);
        var response = new AuthResponse(profile, newJwtToken, newRefreshToken.PlainToken);

        return ResponseModel<AuthResponse>.Success(response);
    }

    public async Task<ResponseModel<string>> LogoutAsync(Guid userGuidId)
    {
        await _refreshTokenService.RevokeAsync(userGuidId);
        return ResponseModel<string>.Success("Logged out successfully");
    }


    public async Task<ResponseModel<IEnumerable<ChatResponse>>> GetUserChatsAsync(Guid userId)
    {
        var chatMembers = await _chatMemberRepository.GetChatsByUserIdAsync(userId);
        var chats = new List<ChatResponse>();

        foreach (var cm in chatMembers)
        {
            var chat = await _chatRepository.GetByIdAsync(cm.ChatId);
            if (chat != null)
                chats.Add(new ChatResponse(chat));
        }

        return ResponseModel<IEnumerable<ChatResponse>>.Success(chats);
    }

    public async Task<ResponseModel<IEnumerable<TeamResponse>>> GetUserProjectsAsync(Guid userId)
    {
        var projects = await _teamRepository.GetByUserIdAsync(userId);
        var responses = projects.Select(p => new TeamResponse(p));

        return ResponseModel<IEnumerable<TeamResponse>>.Success(responses);
    }

    public Task<ResponseModel<UserProfileResponse>> GetUserProfileAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseModel> ChangePasswordAsync(Guid userId, string oldPassword, string newPassword)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseModel> ChangeEmailAsync(Guid userId, string newEmail)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseModel<IEnumerable<RoleResponse>>> GetUserRolesAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseModel<IEnumerable<UserActivityResponse>>> GetUserActivityAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public async Task<ResponseModel> SetUserAvatarAsync(Guid userId, IFormFile avatarFile)
    {
        var user = await _userRepository.GetByIdAsync(userId)
            ?? throw new UserNotFoundException();

        var avatarUrl = await _fileService.UploadFileAsync(avatarFile, $"avatars/{userId}");

        user.AvatarUrl = avatarUrl;
        await _userRepository.UpdateAsync(user);

        return ResponseModel.Success("Avatar updated successfully.");
    }
}