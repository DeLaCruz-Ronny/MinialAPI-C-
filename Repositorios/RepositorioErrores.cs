using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using minimalAPIPeliculas.Entidades;

namespace minimalAPIPeliculas.Repositorios
{
    public class RepositorioErrores : IRepositorioErrores
    {
        private readonly ApplicationDbContext context;
        public RepositorioErrores(ApplicationDbContext context)
        {
            this.context = context;   
        }

        public async Task Crear(Error error)
        {
            context.Add(error);
            await context.SaveChangesAsync();
        }
    }
}