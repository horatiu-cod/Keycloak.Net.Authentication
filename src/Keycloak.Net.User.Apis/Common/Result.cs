using System.Net;

namespace Keycloak.Net.User.Apis.Common;

public record Result(bool IsSuccess, HttpStatusCode? StatusCode, string? Error)
{
    internal static Result Success() => new(true, null, null);
    internal static Result Success(HttpStatusCode? statusCode) => new(true, statusCode, null);
    internal static Result Fail(string? error) => new(false, null, error);
    internal static Result Fail(HttpStatusCode? httpStatusCode, string? error) => new(false, httpStatusCode, error);
}
public record Result<TData>(TData? Content, bool IsSuccess, HttpStatusCode? StatusCode, string? Error)
{
    internal static Result<TData> Success(TData content) => new(content, true, null, null);
    internal static Result<TData> Success(TData Content, HttpStatusCode? StatusCode) => new(Content, true, StatusCode, null);
    internal static Result<TData?> Fail(string? error) => new(default, false, null, error);
    internal static Result<TData?> Fail(HttpStatusCode? statusCode, string? error) => new(default, false, statusCode, error);
}
