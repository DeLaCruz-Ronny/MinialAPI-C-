using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using minimalAPIPeliculas.DTOs;
using minimalAPIPeliculas.Repositorios;

namespace minimalAPIPeliculas.Validaciones
{
    public class CrearGeneroDTOValidador : AbstractValidator<CrearGeneroDTO>
    {
        public CrearGeneroDTOValidador(IRepositorioGeneros repositorioGeneros, IHttpContextAccessor httpContextAccessor)
        {
            var valorDeRutaId = httpContextAccessor.HttpContext?.Request.RouteValues["id"];
            var id = 0;

            if (valorDeRutaId is string valorString)
            {
                int.TryParse(valorString, out id);
            }

            RuleFor(x => x.Nombre).NotEmpty().WithMessage("El campo {PropertyName} es requerido!!")
            .MaximumLength(50).WithMessage("El campo {PropertyName} debe tener menos de {MaxLength} caracteres")
            .Must(PrimeraLetraEnMayusculas).WithMessage("El campo {PropertyName} debe comenzar con mayusculas")
            .MustAsync(async (nombre, _) => 
            {
                var existe = await repositorioGeneros.Existe(id: id, nombre);
                return !existe;
            }).WithMessage(g => $"Ya existe un genero con el nombre {g.Nombre}");
        }

        private bool PrimeraLetraEnMayusculas(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
            {
                return true;
            }

            var primeraLetra = valor[0].ToString();
            return primeraLetra == primeraLetra.ToUpper();
        }
    }
}