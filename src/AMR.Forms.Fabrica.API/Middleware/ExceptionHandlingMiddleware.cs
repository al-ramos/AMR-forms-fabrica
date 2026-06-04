using Microsoft.AspNetCore.Mvc;

namespace AMR.Forms.Fabrica.API.Middleware;

public class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger,
    IHostEnvironment env)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Requisição inválida");
            await WriteProblemAsync(context, StatusCodes.Status400BadRequest, "Requisição inválida", ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Regra de negócio violada");
            await WriteProblemAsync(context, StatusCodes.Status422UnprocessableEntity, "Regra de negócio violada", ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogWarning(ex, "Não encontrado");
            await WriteProblemAsync(context, StatusCodes.Status404NotFound, "Não encontrado", ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro inesperado");
            var detail = env.IsProduction() ? null : ex.Message;
            await WriteProblemAsync(context, StatusCodes.Status500InternalServerError, "Erro interno do servidor", detail);
        }
    }

    private static async Task WriteProblemAsync(HttpContext context, int statusCode, string title, string? detail)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/problem+json";
        await context.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status   = statusCode,
            Title    = title,
            Detail   = detail,
            Instance = context.Request.Path
        });
    }
}
