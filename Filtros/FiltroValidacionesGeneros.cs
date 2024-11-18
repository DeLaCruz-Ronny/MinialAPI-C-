using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using minimalAPIPeliculas.DTOs;

namespace minimalAPIPeliculas.Filtros
{
    public class FiltroValidacionesGeneros : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var validador = context.HttpContext.RequestServices.GetService<IValidator<CrearGeneroDTO>>();
            if (validador is null)
            {
                return await next(context);
            }

            var insumoAValidar = context.Arguments.OfType<CrearGeneroDTO>().FirstOrDefault();
            if (insumoAValidar is null)
            {
                return TypedResults.Problem("No pude ser encontrada la entidad a validar");
            }

            var resultadoValidacion = await validador.ValidateAsync(insumoAValidar);
            if (!resultadoValidacion.IsValid)
            {
                return TypedResults.ValidationProblem(resultadoValidacion.ToDictionary());
            }

            return await next(context);
        }
    }
}