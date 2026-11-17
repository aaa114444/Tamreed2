using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Tamreed2.Application.Common.Exceptions;
using Tamreed2.Domain.Common.Exceptions;

namespace Tamreed2.Api.Middleware;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            if (context.Response.HasStarted)
            {
                throw;
            }

            await HandleExceptionAsync(context, exception);
        }
    }

    private async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception)
    {
        var statusCode = exception switch
        {
            ValidationException => StatusCodes.Status400BadRequest,
            DomainException => StatusCodes.Status400BadRequest,
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            ForbiddenAccessException => StatusCodes.Status403Forbidden,
            KeyNotFoundException => StatusCodes.Status404NotFound,
            InvalidOperationException => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };

        if (statusCode >= StatusCodes.Status500InternalServerError)
        {
            _logger.LogError(
                exception,
                "An unexpected error occurred. TraceId: {TraceId}",
                context.TraceIdentifier);
        }
        else
        {
            _logger.LogWarning(
                "Request failed with status {StatusCode}. Error: {ErrorMessage}. TraceId: {TraceId}",
                statusCode,
                exception.Message,
                context.TraceIdentifier);
        }

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/problem+json";

        if (exception is ValidationException validationException)
        {
            var errors = validationException.Errors
                .GroupBy(error => error.PropertyName)
                .ToDictionary(
                    group => group.Key,
                    group => group
                        .Select(error => error.ErrorMessage)
                        .Distinct()
                        .ToArray());

            var validationProblem = new ValidationProblemDetails(errors)
            {
                Status = statusCode,
                Title = "One or more validation errors occurred.",
                Instance = context.Request.Path
            };

            validationProblem.Extensions["traceId"] =
                context.TraceIdentifier;

            await context.Response.WriteAsJsonAsync(
                validationProblem,
                cancellationToken: context.RequestAborted);

            return;
        }

        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = GetTitle(statusCode),
            Detail = statusCode == StatusCodes.Status500InternalServerError
                ? $"{exception.GetType().Name}: {exception.Message}"
                : exception.Message,
            Instance = context.Request.Path
        };

        problem.Extensions["traceId"] = context.TraceIdentifier;

        await context.Response.WriteAsJsonAsync(
            problem,
            cancellationToken: context.RequestAborted);
    }

    private static string GetTitle(int statusCode)
    {
        return statusCode switch
        {
            StatusCodes.Status400BadRequest => "Bad Request",
            StatusCodes.Status401Unauthorized => "Unauthorized",
            StatusCodes.Status403Forbidden => "Forbidden",
            StatusCodes.Status404NotFound => "Resource Not Found",
            StatusCodes.Status409Conflict => "Conflict",
            _ => "Internal Server Error"
        };
    }
}

