using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using minimalAPIPeliculas.DTOs;
using minimalAPIPeliculas.Entidades;

namespace minimalAPIPeliculas.Repositorios
{
    public interface IRepositorioPeliculas
    {
        Task ActualizarPelicula(Pelicula pelicula);
        Task AsignarGenerosa(int id, List<int> generosIds);
        Task AsignarActores(int id, List<ActorPelicula> actores);
        Task Borrar(int id);
        Task<int> Crear(Pelicula pelicula);
        Task<bool> Existe(int id);
        Task<Pelicula?> ObtenerPorId(int id);
        Task<List<Pelicula>> ObtenerTodos(PaginacionDTO paginacionDTO);
    }
}