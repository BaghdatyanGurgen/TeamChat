namespace TeamChat.Application.DTOs.User;

public record RegisterEmailResponse(Guid UserId, string Email, string Token);
public record SetPasswordResponse(Guid UserId, string AccessToken, string RefreshToken);
public record UserProfileResponse(Guid UserId, string Email, string FirstName, string LastName, string? AvatarUrl, DateTime CreatedAt);
public record AuthoResponse(UserProfileResponse Profile, string AccesToken, string RefreshToken);
public record VerifyEmailResponse(Guid UserId);