using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using minimalAPIPeliculas.DTOs;

namespace minimalAPIPeliculas.Validaciones
{
    public class CrearPeliculaDTOValidador : AbstractValidator<CrearPeliculaDTO>
    {
        public CrearPeliculaDTOValidador()
        {
            RuleFor(x => x.Titulo).NotEmpty().WithMessage(Utilidades.CampoRequeridoMensaje)
            .MaximumLength(150).WithMessage(Utilidades.MaximumLengthMensaje);
        }
    }
}