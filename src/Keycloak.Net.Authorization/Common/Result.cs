using System.Net;

namespace Keycloak.Net.Authorization.Common;

public readonly record struct Result(bool IsSuccess, string? Error, HttpStatusCode? StatusCode)
{
    public static Result Success() => new(true, null, null);
    public static Result Success(HttpStatusCode? StatusCode) => new(true, null, StatusCode);
    public static Result Fail(string? error) => new(false, error, null);
    public static Result Fail(string? error, HttpStatusCode? StatusCode) => new(false, error, StatusCode);
}

public record struct Result<TData>(TData? Content, bool IsSuccess, HttpStatusCode? StatusCode, string? Error)
{
    public static Result<TData> Success(TData? content) => new(content, true, null, null);
    public static Result<TData> Success(TData? content, HttpStatusCode? StatusCode) => new(content, true, StatusCode, null);
    public static Result<TData> Fail(HttpStatusCode? StatusCode) => new(default, false, StatusCode, null);
    public static Result<TData> Fail(HttpStatusCode? StatusCode, string? Error) => new(default, false, StatusCode, Error);
    public static Result<TData> Fail(string? Error) => new(default, false, null, Error);
}