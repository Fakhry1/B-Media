namespace BMedia.Application.Common.Models;

/// <summary>Strongly-typed operation result eliminating exception-driven flow control.</summary>
public class Result<T>
{
    public bool IsSuccess { get; }
    public T? Data { get; }
    public string? Error { get; }
    public IEnumerable<string> Errors { get; }
    public int StatusCode { get; }

    private Result(bool isSuccess, T? data, string? error, IEnumerable<string>? errors, int statusCode)
    {
        IsSuccess = isSuccess;
        Data = data;
        Error = error;
        Errors = errors ?? [];
        StatusCode = statusCode;
    }

    public static Result<T> Success(T data, int statusCode = 200) => new(true, data, null, null, statusCode);
    public static Result<T> Created(T data) => new(true, data, null, null, 201);
    public static Result<T> Failure(string error, int statusCode = 400) => new(false, default, error, null, statusCode);
    public static Result<T> Failure(IEnumerable<string> errors, int statusCode = 400) => new(false, default, null, errors, statusCode);
    public static Result<T> NotFound(string error = "Resource not found") => new(false, default, error, null, 404);
    public static Result<T> Forbidden(string error = "Access denied") => new(false, default, error, null, 403);
    public static Result<T> Unauthorized(string error = "Unauthorized") => new(false, default, error, null, 401);
    public static Result<T> Conflict(string error) => new(false, default, error, null, 409);
}

public class Result
{
    public bool IsSuccess { get; }
    public string? Error { get; }
    public IEnumerable<string> Errors { get; }
    public int StatusCode { get; }

    private Result(bool isSuccess, string? error, IEnumerable<string>? errors, int statusCode)
    {
        IsSuccess = isSuccess;
        Error = error;
        Errors = errors ?? [];
        StatusCode = statusCode;
    }

    public static Result Success(int statusCode = 200) => new(true, null, null, statusCode);
    public static Result Failure(string error, int statusCode = 400) => new(false, error, null, statusCode);
    public static Result NotFound(string error = "Resource not found") => new(false, error, null, 404);
    public static Result Forbidden(string error = "Access denied") => new(false, error, null, 403);
}
