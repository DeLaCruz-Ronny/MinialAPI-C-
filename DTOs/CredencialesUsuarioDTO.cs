using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace minimalAPIPeliculas.DTOs
{
    public class CredencialesUsuarioDTO
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}