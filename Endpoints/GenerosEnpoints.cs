using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using minimalAPIPeliculas.DTOs;
using minimalAPIPeliculas.Entidades;
using minimalAPIPeliculas.Filtros;
using minimalAPIPeliculas.Repositorios;

namespace minimalAPIPeliculas.Endpoints
{
    public static class GenerosEnpoints
    {
        public static RouteGroupBuilder MapGeneros(this RouteGroupBuilder group)
        {
            group.MapGet("/", ObtenerGeneros)
                .CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("generos_get")).RequireAuthorization();
            group.MapGet("/{id:int}", ObtenerGeneroPorId);
            group.MapPost("/", CrearGenero).AddEndpointFilter<FiltroValidaciones<CrearGeneroDTO>>().RequireAuthorization("esAdmin");
            group.MapPut("/{id:int}", ActualizarGenero).AddEndpointFilter<FiltroValidaciones<CrearGeneroDTO>>().RequireAuthorization("esAdmin");
            group.MapDelete("/{id:int}", BorrarGenero).RequireAuthorization("esAdmin");
            return group;
        }

        static async Task<Ok<List<GeneroDTO>>> ObtenerGeneros(IRepositorioGeneros repo, IMapper mapper)
        {
            var generos = await repo.ObtenerTodos();
            var generodto = mapper.Map<List<GeneroDTO>>(generos);
            return TypedResults.Ok(generodto);
        }

        static async Task<Results<Ok<GeneroDTO>, NotFound>> ObtenerGeneroPorId(IRepositorioGeneros repo, int id,IMapper mapper)
        {
            var genero = await repo.ObtenerPorId(id);
            if (genero is null)
            {
                return TypedResults.NotFound();
            }
            var generodto = mapper.Map<GeneroDTO>(genero);
            return TypedResults.Ok(generodto);
        }

        static async Task<Results<Created<GeneroDTO>, ValidationProblem>> CrearGenero(CrearGeneroDTO crearGeneroDTO, IRepositorioGeneros repo, IOutputCacheStore output, IMapper mapper)
        {
            
            var genero = mapper.Map<Genero>(crearGeneroDTO);
            var id = await repo.Crear(genero);
            await output.EvictByTagAsync("generos_get", default); //Eliminar cache despues de agregar un registro
            var generodto = mapper.Map<GeneroDTO>(genero);
            return TypedResults.Created($"/generos/{id}", generodto);
        }

        static async Task<Results<NoContent, NotFound, ValidationProblem>> ActualizarGenero(int id, CrearGeneroDTO crearGeneroDTO, IRepositorioGeneros repo, IOutputCacheStore output, IMapper mapper)
        {
            
            var existe = await repo.Existe(id);
            if (!existe)
            {
                return TypedResults.NotFound();
            }
            var genero = mapper.Map<Genero>(crearGeneroDTO);
            genero.Id = id;
            await repo.Actualizar(genero);
            await output.EvictByTagAsync("generos_get", default);
            return TypedResults.NoContent();
        }

        static async Task<Results<NoContent, NotFound>> BorrarGenero(int id, IRepositorioGeneros repo, IOutputCacheStore output)
        {
            var existe = await repo.Existe(id);
            if (!existe)
            {
                return TypedResults.NotFound();
            }
            await repo.Borrar(id);
            await output.EvictByTagAsync("generos_get", default);
            return TypedResults.NoContent();
        }
    }
}