using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace minimalAPIPeliculas.DTOs
{
    public class CrearActorDTO
    {
        public string Nombre { get; set; } = null!;
        public DateTime FechaNacimiento { get; set; }
        public IFormFile? Foto { get; set; }
    }
}