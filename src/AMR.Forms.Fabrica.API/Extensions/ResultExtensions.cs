using AMR.Forms.Fabrica.Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace AMR.Forms.Fabrica.API.Extensions;

public static class ResultExtensions
{
    public static IActionResult ToActionResult<T>(this Result<T> result, ControllerBase controller)
    {
        if (result.Sucesso)
            return controller.Ok(result.Valor);

        if (result.Erro?.Contains("não encontrado", StringComparison.OrdinalIgnoreCase) == true)
            return controller.Problem(result.Erro, statusCode: StatusCodes.Status404NotFound);

        return controller.Problem(result.Erro, statusCode: StatusCodes.Status400BadRequest);
    }
}
