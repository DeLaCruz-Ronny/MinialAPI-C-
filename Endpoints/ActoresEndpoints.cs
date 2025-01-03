using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using minimalAPIPeliculas.DTOs;
using minimalAPIPeliculas.Entidades;
using minimalAPIPeliculas.Filtros;
using minimalAPIPeliculas.Repositorios;
using minimalAPIPeliculas.Servicios;

namespace minimalAPIPeliculas.Endpoints
{
    public static class ActoresEndpoints
    {
        private static readonly string contenedor = "actores";
        public static RouteGroupBuilder MapActores(this RouteGroupBuilder group)
        {
            group.MapGet("/", ObtenerTodos); //.CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("actores-get"));
            group.MapGet("/{id:int}", ObtenerPorId);
            group.MapGet("obtenerPorNombre/{nombre}", ObtenerPorNombre);
            group.MapPost("/", Crear).DisableAntiforgery().AddEndpointFilter<FiltroValidaciones<CrearActorDTO>>().RequireAuthorization("esAdmin");
            group.MapPut("/{id:int}", Actualizar).DisableAntiforgery().AddEndpointFilter<FiltroValidaciones<CrearActorDTO>>().RequireAuthorization("esAdmin");
            group.MapDelete("/{id:int}", Borrar).RequireAuthorization("esAdmin");
            return group;
        }
        static async Task<Ok<List<ActorDTO>>> ObtenerTodos(IRepositorioActores repositorio, IMapper mapper, int pagina = 1, int recordsPorPagina = 10)
        {
            var paginacion = new PaginacionDTO{ Pagina = pagina, RecordsPorPagina = recordsPorPagina };
            var actores = await repositorio.ObtenerTodos(paginacion);
            var actoresdto = mapper.Map<List<ActorDTO>>(actores);
            return TypedResults.Ok(actoresdto);
        }

        static async Task<Results<Ok<ActorDTO>, NotFound>> ObtenerPorId(int id,IRepositorioActores repositorio, IMapper mapper){
            var actor = await repositorio.ObtenerPorId(id);
            if (actor is null)
            {
                return TypedResults.NotFound();
            }

            var actorDTO = mapper.Map<ActorDTO>(actor);
            return TypedResults.Ok(actorDTO);
        }

        static async Task<Ok<List<ActorDTO>>> ObtenerPorNombre(string nombre,IRepositorioActores repositorio, IMapper mapper){
            var actores = await repositorio.ObtenerPorNombre(nombre);
            var actoresdto = mapper.Map<List<ActorDTO>>(actores);
            return TypedResults.Ok(actoresdto);
        }

        static async Task<Results<Created<ActorDTO>, ValidationProblem>> Crear([FromForm] CrearActorDTO crearActorDTO, IRepositorioActores repositorio, IOutputCacheStore outputCacheStore, IMapper mapper, IAlmacenadorArchivos almacenadorArchivos)
        {
            

            var actor = mapper.Map<Actor>(crearActorDTO);

            if (crearActorDTO.Foto is not null)
            {
                var url = await almacenadorArchivos.Almacenar(contenedor, crearActorDTO.Foto);
                actor.Foto = url;
            }

            var id = await repositorio.Crear(actor);
            await outputCacheStore.EvictByTagAsync("actores-get", default);
            var actorDTO = mapper.Map<ActorDTO>(actor);
            return TypedResults.Created($"/actores/{id}", actorDTO);
        }

        static async Task<Results<NoContent, NotFound>> Actualizar(int id,[FromForm] CrearActorDTO crearActorDTO, IRepositorioActores repositorioActores, IAlmacenadorArchivos almacenadorArchivos, IOutputCacheStore outputCacheStore, IMapper mapper){
            var actorDB = await repositorioActores.ObtenerPorId(id);

            if (actorDB is null)
            {
                return TypedResults.NotFound();
            }

            var actorparaactualizar = mapper.Map<Actor>(crearActorDTO);
            actorparaactualizar.Id = id;
            actorparaactualizar.Foto = actorDB.Foto;

            if (crearActorDTO.Foto is not null)
            {
                var url = await almacenadorArchivos.Editar(actorparaactualizar.Foto, contenedor, crearActorDTO.Foto);
                actorparaactualizar.Foto = url;
            }

            await repositorioActores.Actualizar(actorparaactualizar);
            await outputCacheStore.EvictByTagAsync("actores-get", default);
            return TypedResults.NoContent();
        }

        static async Task<Results<NoContent, NotFound>> Borrar(int id, IRepositorioActores repositorioActores, IOutputCacheStore outputCacheStore, IAlmacenadorArchivos almacenadorArchivos){
            var actorDB = await repositorioActores.ObtenerPorId(id);
            if (actorDB is null)
            {
                return TypedResults.NotFound();
            }

            await repositorioActores.Borrar(id);
            await almacenadorArchivos.Borrar(actorDB.Foto, contenedor);
            await outputCacheStore.EvictByTagAsync("actores-get", default);
            return TypedResults.NoContent();
        }
    }
}