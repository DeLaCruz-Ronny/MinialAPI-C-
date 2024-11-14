using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace minimalAPIPeliculas.DTOs
{
    public class ActorPeliculaDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Personaje { get; set; } = null!;
    }
}