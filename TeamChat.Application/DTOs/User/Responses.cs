namespace TeamChat.Application.DTOs.User;

public record RegisterEmailResponse(Guid UserId, string Email, string Token);
public record SetPasswordResponse(Guid UserId, string AccessToken, string RefreshToken);
public record UserProfileResponse(Guid Id, string Email, string FirstName, string LastName, string? AvatarUrl, DateTime CreatedAt);
public record AuthResponse(UserProfileResponse Profile, string AccessToken, string RefreshToken);
public record VerifyEmailResponse(Guid UserId);
public record RoleResponse (Guid Id, string Name);
public record UserActivityResponse(Guid Id, string Name);