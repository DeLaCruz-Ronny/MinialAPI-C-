using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using minimalAPIPeliculas.Entidades;

namespace minimalAPIPeliculas.Repositorios
{
    public interface IRepositorioActores
    {
        Task Actualizar(Actor actor);
        Task Borrar(int id);
        Task<int> Crear(Actor actor);
        Task<bool> Existe(int id);
        Task<Actor?> ObtenerPorId(int id);
        Task<List<Actor>> ObtenerPorNombre(string nombre);
        Task<List<Actor>> ObtenerTodos();
    }
}