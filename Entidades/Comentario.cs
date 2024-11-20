using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace minimalAPIPeliculas.Entidades
{
    public class Comentario
    {
        public int Id { get; set; }
        public string Cuerpo{get; set;} = null!;
        public int PeliculaId { get; set; }
        public string UsuarioId { get; set; } = null!;
        public IdentityUser Usuario { get; set; } = null!;

    }
}