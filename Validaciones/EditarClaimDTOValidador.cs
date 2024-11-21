using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using minimalAPIPeliculas.DTOs;

namespace minimalAPIPeliculas.Validaciones
{
    public class EditarClaimDTOValidador : AbstractValidator<EditarClaimDTO>
    {
        public EditarClaimDTOValidador()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage(Utilidades.CampoRequeridoMensaje)
            .MaximumLength(256).WithMessage(Utilidades.MaximumLengthMensaje)
            .EmailAddress().WithMessage(Utilidades.EmailMensaje);
        }
    }
}