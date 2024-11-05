using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using minimalAPIPeliculas.DTOs;
using minimalAPIPeliculas.Entidades;
using minimalAPIPeliculas.Utilidades;

namespace minimalAPIPeliculas.Repositorios
{
    public class RepositorioPeliculas :IRepositorioPeliculas
    {
        private readonly ApplicationDbContext context;
        private readonly HttpContext httpContext;

        public RepositorioPeliculas(ApplicationDbContext context, IHttpContextAccessor contextAccessor)
        {
            this.context = context;
            httpContext = contextAccessor.HttpContext!;
        }

        public async Task<List<Pelicula>> ObtenerTodos(PaginacionDTO paginacionDTO)
        {
            var queryable = context.Peliculas.AsQueryable();
            await httpContext.InsertarParametrosPaginacionCabecera(queryable);
            return await queryable.OrderBy(p => p.Titulo).Paginar(paginacionDTO).ToListAsync();
        }

        public async Task<Pelicula?> ObtenerPorId(int id)
        {
            return await context.Peliculas.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<int> Crear(Pelicula pelicula)
        {
            context.Add(pelicula);
            await context.SaveChangesAsync();
            return pelicula.Id;
        }

        public async Task ActualizarPelicula(Pelicula pelicula)
        {
            context.Update(pelicula);
            await context.SaveChangesAsync();
        }

        public async Task Borrar(int id)
        {
            await context.Peliculas.Where(p => p.Id == id).ExecuteDeleteAsync();
        }

        public async Task<bool> Existe(int id)
        {
            return await context.Peliculas.AnyAsync(p => p.Id == id);
        }
    }
}