using System.Net;
using System.Text.Json;
using TeamChat.Domain.Models.Exceptions;

namespace TeamChat.API.Middlewares;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, message) = exception switch
        {
            UserNotFoundException => (HttpStatusCode.NotFound, "User not found."),
            ChatNotFoundException => (HttpStatusCode.NotFound, "Chat not found."),
            MessageNotFoundException => (HttpStatusCode.NotFound, "Message not found."),
            CompanyNotFoundException => (HttpStatusCode.NotFound, "Company not found."),
            DepartmentNotFoundException => (HttpStatusCode.NotFound, "Department not found."),
            TeamNotFoundException => (HttpStatusCode.NotFound, "Team not found."),
            CompanyUserNotFoundException => (HttpStatusCode.NotFound, "Company member not found."),
            NoAccessException => (HttpStatusCode.Forbidden, "Access denied."),
            InvalidEmailException => (HttpStatusCode.BadRequest, "Invalid email address."),
            InvalidPasswordException => (HttpStatusCode.BadRequest, "Invalid password."),
            InvalidTokenException => (HttpStatusCode.BadRequest, "Invalid or expired token."),
            CannotCreatePositionException => (HttpStatusCode.BadRequest, "Failed to create position."),
            UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "Unauthorized."),
            _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred.")
        };

        var body = JsonSerializer.Serialize(new
        {
            isSuccess = false,
            message,
            statusCode = (int)statusCode
        });

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        return context.Response.WriteAsync(body);
    }
}