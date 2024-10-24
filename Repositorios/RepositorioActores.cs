using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using minimalAPIPeliculas.Entidades;

namespace minimalAPIPeliculas.Repositorios
{
    public class RepositorioActores : IRepositorioActores
    {
        private readonly ApplicationDbContext context;

        public RepositorioActores(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<List<Actor>> ObtenerTodos()
        {
            return await context.Actores.OrderBy(a => a.Nombre).ToListAsync();
        }

        public async Task<Actor?> ObtenerPorId(int id)
        {
            return await context.Actores.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Actor>> ObtenerPorNombre(string nombre)
        {
            return await context.Actores.Where(a => a.Nombre.Contains(nombre)).OrderBy(w => w.Nombre).ToListAsync();
        }

        public async Task<int> Crear(Actor actor)
        {
            context.Add(actor);
            await context.SaveChangesAsync();
            return actor.Id;
        }

        public async Task<bool> Existe(int id)
        {
            return await context.Actores.AnyAsync(x => x.Id == id);
        }

        public async Task Actualizar(Actor actor)
        {
            context.Update(actor);
            await context.SaveChangesAsync();
        }

        public async Task Borrar(int id)
        {
            await context.Actores.Where(a => a.Id == id).ExecuteDeleteAsync();
        }

    }
}