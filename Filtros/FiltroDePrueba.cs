using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using minimalAPIPeliculas.Repositorios;

namespace minimalAPIPeliculas.Filtros
{
    public class FiltroDePrueba : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext contexto, EndpointFilterDelegate next)
        {
            //Este codigo se ejecuta antes del endpoint
            var paramRepositorioGeneros = contexto.Arguments.OfType<IRepositorioGeneros>().FirstOrDefault();
            var paramEntero = contexto.Arguments.OfType<int>().FirstOrDefault();
            var paramMapper = contexto.Arguments.OfType<IMapper>().FirstOrDefault();

            var resaultado = await next(contexto);
            //Este se ejecuta despues del endpoint
            return resaultado;
        }
    }
}