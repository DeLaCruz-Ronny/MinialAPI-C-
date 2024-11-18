using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using minimalAPIPeliculas.DTOs;

namespace minimalAPIPeliculas.Validaciones
{
    public class CrearActorDTOValidador : AbstractValidator<CrearActorDTO>
    {
        public CrearActorDTOValidador()
        {
            RuleFor(x => x.Nombre).NotEmpty().WithMessage(Utilidades.CampoRequeridoMensaje)
            .MaximumLength(150).WithMessage(Utilidades.MaximumLengthMensaje);

            var fechaMinima = new DateTime(1900, 1, 1);
            RuleFor(x => x.FechaNacimiento).GreaterThanOrEqualTo(fechaMinima)
            .WithMessage(Utilidades.GreaterThanOrEqualToMensaje(fechaMinima));
        }
    }
}