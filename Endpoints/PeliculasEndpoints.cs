using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using minimalAPIPeliculas.DTOs;
using minimalAPIPeliculas.Entidades;
using minimalAPIPeliculas.Filtros;
using minimalAPIPeliculas.Migrations;
using minimalAPIPeliculas.Repositorios;
using minimalAPIPeliculas.Servicios;

namespace minimalAPIPeliculas.Endpoints
{
    public static class PeliculasEndpoints
    {
        private static readonly string contenedor = "peliculas";
        public static RouteGroupBuilder MapPeliculas(this RouteGroupBuilder group)
        {
            group.MapGet("/", Obtener).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("peliculas-get"));
            group.MapGet("/{id:int}", ObtenerPorId);
            group.MapPost("/", Crear).DisableAntiforgery().AddEndpointFilter<FiltroValidaciones<CrearPeliculaDTO>>().RequireAuthorization("esAdmin");
            group.MapPut("/{id:int}",Actualizar).DisableAntiforgery().AddEndpointFilter<FiltroValidaciones<CrearPeliculaDTO>>().RequireAuthorization("esAdmin");
            group.MapDelete("/{id:int}", Borrar).RequireAuthorization("esAdmin");
            group.MapPost("/{id:int}/asignargeneros", AsignarGeneros).RequireAuthorization("esAdmin");
            group.MapPost("/{id:int}/asignaractores", AsignarActores).RequireAuthorization("esAdmin");
            return group;
        }

        static async Task<Created<PeliculaDTO>> Crear([FromForm] CrearPeliculaDTO crearPeliculaDTO, IRepositorioPeliculas repositorioPeliculas, IAlmacenadorArchivos almacenadorArchivos, IOutputCacheStore outputCacheStore, IMapper mapper)
        {
            var pelicula = mapper.Map<Pelicula>(crearPeliculaDTO);

            if (crearPeliculaDTO.Poster is not null)
            {
                var url = await almacenadorArchivos.Almacenar(contenedor, crearPeliculaDTO.Poster);
                pelicula.Poster = url;
            }

            var id = await repositorioPeliculas.Crear(pelicula);
            await outputCacheStore.EvictByTagAsync("peliculas-get", default);
            var peliculaDTO = mapper.Map<PeliculaDTO>(pelicula);
            return TypedResults.Created($"/peliculas/{id}", peliculaDTO);
        }

        static async Task<Ok<List<PeliculaDTO>>> Obtener(IRepositorioPeliculas repositorioPeliculas, IMapper mapper, int pagina = 1, int recordsPorPagina = 10)
        {
            var paginacion = new PaginacionDTO {Pagina = pagina, RecordsPorPagina = recordsPorPagina};
            var peliculas = await repositorioPeliculas.ObtenerTodos(paginacion);
            var peliculaDTO = mapper.Map<List<PeliculaDTO>>(peliculas);
            return TypedResults.Ok(peliculaDTO);
        }

        static async Task<Results<Ok<PeliculaDTO>, NotFound>> ObtenerPorId(int id, IRepositorioPeliculas repositorioPeliculas, IMapper mapper)
        {
            var pelicula = await repositorioPeliculas.ObtenerPorId(id);
            if (pelicula is null)
            {
                return TypedResults.NotFound();
            }

            var pelidulaDTO = mapper.Map<PeliculaDTO>(pelicula);
            return TypedResults.Ok(pelidulaDTO);
        }

        static async Task<Results<NoContent, NotFound>> Actualizar(int id, [FromForm] CrearPeliculaDTO crearPeliculaDTO,
        IRepositorioPeliculas repositorioPeliculas, IAlmacenadorArchivos almacenadorArchivos, IOutputCacheStore outputCacheStore, IMapper mapper)
        {
            var peliculaDB = await repositorioPeliculas.ObtenerPorId(id);

            if (peliculaDB is null)
            {
                return TypedResults.NotFound();
            }

            var peliculaActualizar = mapper.Map<Pelicula>(crearPeliculaDTO);
            peliculaActualizar.Id = id;
            peliculaActualizar.Poster = peliculaActualizar.Poster;

            if (crearPeliculaDTO.Poster is not null)
            {
                var url = await almacenadorArchivos.Editar(peliculaActualizar.Poster,contenedor,crearPeliculaDTO.Poster);
                peliculaActualizar.Poster = url;
            }

            await repositorioPeliculas.ActualizarPelicula(peliculaActualizar);
            await outputCacheStore.EvictByTagAsync("peliculas-get", default);
            return TypedResults.NoContent();
        }

        static async Task<Results<NoContent, NotFound>> Borrar(int id,IRepositorioPeliculas repositorioPeliculas, IAlmacenadorArchivos almacenadorArchivos, IOutputCacheStore outputCacheStore)
        {
            var peliculaDB = await repositorioPeliculas.ObtenerPorId(id);

            if (peliculaDB is null)
            {
                return TypedResults.NotFound();
            }

            await repositorioPeliculas.Borrar(id);
            await almacenadorArchivos.Borrar(peliculaDB.Poster, contenedor);
            await outputCacheStore.EvictByTagAsync("peliculas-get", default);
            return TypedResults.NoContent();
        }

        static async Task<Results<NoContent, NotFound, BadRequest<string>>> AsignarGeneros(int id,List<int> generosIds,IRepositorioPeliculas repositorioPeliculas, IRepositorioGeneros repositorioGeneros)
        {
            if (!await repositorioPeliculas.Existe(id))
            {
                return TypedResults.NotFound();
            }

            var generosExistentes = new List<int>();

            if (generosIds.Count != 0)
            {
                generosExistentes = await repositorioGeneros.Existen(generosIds);
            }

            if (generosExistentes.Count != generosIds.Count)
            {
                var generosNoExistentes = generosIds.Except(generosExistentes);
                return TypedResults.BadRequest($"Los generos de id {string.Join(",",generosNoExistentes)} no existen.");
            }

            await repositorioPeliculas.AsignarGenerosa(id, generosIds);
            return TypedResults.NoContent();
        }

        static async Task<Results<NoContent, NotFound, BadRequest<string>>> AsignarActores(int id,List<AsignarActorPeliculaDTO> actoresDTO,IRepositorioPeliculas repositorioPeliculas, IRepositorioActores repositorioActores, IMapper mapper)
        {
            if (! await repositorioPeliculas.Existe(id))
            {
                return TypedResults.NotFound();
            }

            var actoresExistentes = new List<int>();
            var actoresIds = actoresDTO.Select(a => a.ActorId).ToList();

            if (actoresDTO.Count != 0)
            {
                actoresExistentes = await repositorioActores.Existen(actoresIds);
            }

            if (actoresExistentes.Count != actoresDTO.Count)
            {
                var actoresNoExistentes = actoresIds.Except(actoresExistentes);
                return TypedResults.BadRequest($"Los actores de id {string.Join(",",actoresNoExistentes)} no existen.");
            }

            var actores = mapper.Map<List<ActorPelicula>>(actoresDTO);

            await repositorioPeliculas.AsignarActores(id,actores);
            return TypedResults.NoContent();
        }
    }
}