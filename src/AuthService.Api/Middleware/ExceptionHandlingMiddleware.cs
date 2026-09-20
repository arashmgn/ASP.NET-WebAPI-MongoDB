using AuthService.Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Api.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            var (status, title) = exception switch
            {
                NotFoundException => (StatusCodes.Status404NotFound, "Not found"),
                ConflictException => (StatusCodes.Status409Conflict, "Conflict"),
                InvalidCredentialsException => (StatusCodes.Status401Unauthorized, "Unauthorized"),
                ForbiddenException => (StatusCodes.Status403Forbidden, "Forbidden"),
                _ => (StatusCodes.Status500InternalServerError, "Server error")
            };

            if (status == StatusCodes.Status500InternalServerError)
                logger.LogError(exception, "Unhandled exception");

            var problem = new ProblemDetails
            {
                Status = status,
                Title = title,
                Detail = status == StatusCodes.Status500InternalServerError ? "An unexpected error occurred" : exception.Message
            };

            context.Response.StatusCode = status;
            await context.Response.WriteAsJsonAsync(problem);
        }
    }
}
