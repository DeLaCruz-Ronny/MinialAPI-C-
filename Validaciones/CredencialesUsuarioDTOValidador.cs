using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using minimalAPIPeliculas.DTOs;
using FluentValidation; //Tuve que instalar este paquete

namespace minimalAPIPeliculas.Validaciones
{
    public class CredencialesUsuarioDTOValidador: AbstractValidator<CredencialesUsuarioDTO>
    {
        public CredencialesUsuarioDTOValidador()
        {
            
        }
    }
}