using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace minimalAPIPeliculas.DTOs
{
    public class ComentarioDTO
    {
        public int Id { get; set; }
        public string Cuerpo{get; set;} = null!;
        public int PeliculaId { get; set; }
    }
}