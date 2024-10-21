using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using minimalAPIPeliculas.Entidades;

namespace minimalAPIPeliculas.Repositorios
{
    public interface IRepositorioGeneros
    {
        Task<List<Genero>> ObtenerTodos();
        Task<Genero?> ObtenerPorId(int id);
        Task<int> Crear(Genero genero);
    }
}