using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using minimalAPIPeliculas.Entidades;

namespace minimalAPIPeliculas.Repositorios
{
    public class RepositorioGeneros : IRepositorioGeneros
    {
        private readonly ApplicationDbContext context;

        public RepositorioGeneros(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<int> Crear(Genero genero)
        {
            context.Add(genero);
            await context.SaveChangesAsync();
            return genero.Id;
        }

        public async Task<Genero?> ObtenerPorId(int id)
        {
            return await context.Generos.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Genero>> ObtenerTodos()
        {
            return await context.Generos.OrderBy(x => x.Nombre).ToListAsync();
        }
    }
}