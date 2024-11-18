using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using minimalAPIPeliculas.DTOs;

namespace minimalAPIPeliculas.Validaciones
{
    public class CrearComentarioDTOValidacion : AbstractValidator<CrearComentarioDTO>
    {
        public CrearComentarioDTOValidacion()
        {
            RuleFor(x => x.Cuerpo).NotEmpty().WithMessage(Utilidades.CampoRequeridoMensaje);
        }
    }
}