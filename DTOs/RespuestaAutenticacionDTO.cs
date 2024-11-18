using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace minimalAPIPeliculas.DTOs
{
    public class RespuestaAutenticacionDTO
    {
        public string Token { get; set; } = null!;
        public DateTime Expiracion { get; set; }
    }
}