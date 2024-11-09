using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using minimalAPIPeliculas.DTOs;
using minimalAPIPeliculas.Entidades;
using minimalAPIPeliculas.Repositorios;

namespace minimalAPIPeliculas.Endpoints
{
    public static class ComentariosEndpoints
    {
        public static RouteGroupBuilder MapComentarios(this RouteGroupBuilder group)
        {
            group.MapGet("/",ObtenerTodos).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("comentarios-get").SetVaryByRouteValue(new string[] {"peliculaId"}));
            group.MapGet("/{id:int}",ObtenerPorId);
            group.MapPost("/", Crear);
            return group;
        }

        static async Task<Results<Ok<List<ComentarioDTO>>, NotFound>> ObtenerTodos(int peliculaId,IRepositorioComentarios repositorioComentarios,IRepositorioPeliculas repositorioPeliculas,IMapper mapper, IOutputCacheStore outputCacheStore)
        {
            if (! await repositorioPeliculas.Existe(peliculaId))
            {
                return TypedResults.NotFound();
            }

            var comentarios = await repositorioComentarios.ObtenerTodos(peliculaId);
            var comentarioDTO = mapper.Map<List<ComentarioDTO>>(comentarios);
            return TypedResults.Ok(comentarioDTO);
        }

        static async Task<Results<Ok<ComentarioDTO>, NotFound>> ObtenerPorId(int peliculaId, int id,IRepositorioComentarios repositorioComentarios,IMapper mapper)
        {
            var comentario = await repositorioComentarios.ObtenerPorId(id);
            if (comentario is null)
            {
                return TypedResults.NotFound();
            }

            var comentarioDTO = mapper.Map<ComentarioDTO>(comentario);
            return TypedResults.Ok(comentarioDTO);
        }

        static async Task<Results<Created<ComentarioDTO>, NotFound>> Crear(int peliculaId,CrearComentarioDTO crearComentarioDTO, IRepositorioComentarios repositorioComentarios,IRepositorioPeliculas repositorioPeliculas,IMapper mapper, IOutputCacheStore outputCacheStore)
        {
            if (! await repositorioPeliculas.Existe(peliculaId))
            {
                return TypedResults.NotFound();
            }

            var comentario = mapper.Map<Comentario>(crearComentarioDTO);
            comentario.PeliculaId = peliculaId;
            var id = await repositorioComentarios.Crear(comentario);
            await outputCacheStore.EvictByTagAsync("comentarios-get",default);
            var comentarioDTO = mapper.Map<ComentarioDTO>(comentario);
            return TypedResults.Created($"/comentario/{id}", comentarioDTO);

        }
    }
}