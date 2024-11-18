using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using minimalAPIPeliculas.Entidades;

namespace minimalAPIPeliculas.Repositorios
{
    public interface IRepositorioErrores
    {
        Task Crear(Error error);
    }
}