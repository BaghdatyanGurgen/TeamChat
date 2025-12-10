namespace TeamChat.Application.DTOs.User;

public record CreateDraftUserRequest(string Email);
public record SetPasswordRequest(Guid UserId, string Password);
public record SetUserProfileRequest(string FirstName, string LastName);
public record VerifyEmailRequest(Guid UserId, string Token);
public record LoginRequest(string Email, string Password);
public record RefreshTokenRequest(string Token, string RefreshToken);
