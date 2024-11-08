using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace minimalAPIPeliculas.Entidades
{
    public class Pelicula
    {
        public int Id { get; set; }
        [StringLength(150)]
        public string Titulo { get; set; } = null!;
        public bool EnCines { get; set; }
        public DateTime FechaLanzamiento { get; set; }
        [Unicode(true)]
        public string? Poster { get; set; }
        public List<Comentario> Comentario { get; set; } = new List<Comentario>();
    }
}