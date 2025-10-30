namespace TeamChat.Application.DTOs;

public class ResponseModel
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }

    public static ResponseModel Success(string? message = null)
        => new() { IsSuccess = true, Message = message };

    public static ResponseModel Fail(string message)
        => new() { IsSuccess = false, Message = message };
}

public class ResponseModel<T> : ResponseModel
{
    public T? Data { get; set; }

    public static ResponseModel<T> Success(T data, string? message = null)
        => new() { IsSuccess = true, Data = data, Message = message };

    public new static ResponseModel<T> Fail(string message)
        => new() { IsSuccess = false, Message = message };
}