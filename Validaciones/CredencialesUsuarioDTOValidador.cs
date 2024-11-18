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
            RuleFor(x => x.Email).NotEmpty().WithMessage(Utilidades.CampoRequeridoMensaje)
            .MaximumLength(256).WithMessage(Utilidades.MaximumLengthMensaje)
            .EmailAddress().WithMessage(Utilidades.EmailMensaje);

            RuleFor(x => x.Password).NotEmpty().WithMessage(Utilidades.CampoRequeridoMensaje);
        }
    }
}