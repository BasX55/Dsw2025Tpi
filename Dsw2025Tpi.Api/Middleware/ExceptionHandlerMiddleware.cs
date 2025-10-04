using Dsw2025Tpi.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        var statusCode = exception switch
        {
            ArgumentNullException => HttpStatusCode.BadRequest,
            ArgumentException => HttpStatusCode.BadRequest,
            EntityNotFoundException => HttpStatusCode.NotFound,
            InsufficientStockException => HttpStatusCode.UnprocessableEntity,
            InactiveProductException => HttpStatusCode.UnprocessableEntity,
            InvalidProductPriceException => HttpStatusCode.UnprocessableEntity,
            DuplicatedEntityException => HttpStatusCode.Conflict,
            _ => HttpStatusCode.InternalServerError
        };

        var result = JsonSerializer.Serialize(new
        {
            error = exception.Message,
            type = exception.GetType().Name
        });

        response.StatusCode = (int)statusCode;
        return response.WriteAsync(result);
    }
}