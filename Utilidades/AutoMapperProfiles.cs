using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using minimalAPIPeliculas.DTOs;
using minimalAPIPeliculas.Entidades;

namespace minimalAPIPeliculas.Utilidades
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<CrearGeneroDTO, Genero>();
            CreateMap<Genero, GeneroDTO>();

            CreateMap<CrearActorDTO, Actor>().ForMember(x => x.Foto, opciones => opciones.Ignore());
            CreateMap<Actor, ActorDTO>();

            CreateMap<CrearPeliculaDTO, Pelicula>().ForMember(x => x.Poster, opciones => opciones.Ignore());
            CreateMap<Pelicula, PeliculaDTO>();

            CreateMap<CrearComentarioDTO, Comentario>();
            CreateMap<Comentario, ComentarioDTO>();
        }
    }
}