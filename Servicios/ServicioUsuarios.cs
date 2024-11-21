using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace minimalAPIPeliculas.Servicios
{
    public class ServicioUsuarios : IServicioUsuarios
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<IdentityUser> userManager;

        public ServicioUsuarios(IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.userManager = userManager;
        }

        public async Task<IdentityUser?> ObtenerUsuario()
        {
            var emailClaims = httpContextAccessor.HttpContext!.User.Claims.Where(x => x.Type == "email").FirstOrDefault();
            if (emailClaims is null)
            {
                return null;
            }

            var email = emailClaims.Value;
            return await userManager.FindByEmailAsync(email);
        }
    }
}