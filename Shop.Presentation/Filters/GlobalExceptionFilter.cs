using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using Shop.Application.DTOs.Response;
using Shop.Domain.Exceptions;

namespace Shop.Presentation.Filters;

public class GlobalExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var traceId = context.HttpContext.TraceIdentifier;
        
        var (statusCode, message, type, details) = MapException(context.Exception);
        
        var errorResponse = new ErrorResponse(message, statusCode, type, details)
        {
            TraceId = traceId
        };

        context.Result = new ObjectResult(errorResponse)
        {
            StatusCode = statusCode
        };
        
        context.ExceptionHandled = true;
    }

    private (int statusCode, string message, string type, object details) MapException(Exception exception)
    {
        return exception switch
        {
            // Excepciones del dominio
            CustomNotFoundException ex => (
                StatusCodes.Status404NotFound,
                ex.Message,
                "NotFound",
                null
            ),

            // Excepciones de validación
            CustomValidationException ex => (
                StatusCodes.Status400BadRequest,
                ex.Message,
                "Validation",
                null
            ),

            // Excepciones de argumentos
            ArgumentNullException ex => (
                StatusCodes.Status400BadRequest,
                $"Required parameter '{ex.ParamName}' was not provided",
                "InvalidArgument",
                new { Parameter = ex.ParamName }
            ),

            ArgumentException ex => (
                StatusCodes.Status400BadRequest,
                ex.Message,
                "InvalidArgument",
                new { Parameter = ex.ParamName }
            ),

            // Excepciones de autorización
            UnauthorizedAccessException => (
                StatusCodes.Status401Unauthorized,
                "Access denied. You don't have permission to access this resource",
                "Unauthorized",
                null
            ),

            // Excepciones de formato
            FormatException ex => (
                StatusCodes.Status400BadRequest,
                "Invalid format provided",
                "InvalidFormat",
                new { Details = ex.Message }
            ),

            // Timeout
            TimeoutException => (
                StatusCodes.Status408RequestTimeout,
                "The request timed out",
                "Timeout",
                null
            ),
            
            SecurityTokenException => (
                StatusCodes.Status400BadRequest,
                "Invalid token",
                "Expired or Invalid Token",
                null
            ),

            // Excepción general - NO exponer detalles internos
            _ => (
                StatusCodes.Status500InternalServerError,
                "An internal server error occurred. Please try again later",
                "InternalError",
                null
            ),
        };
    }
}