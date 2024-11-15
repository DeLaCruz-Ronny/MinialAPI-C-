using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace minimalAPIPeliculas.Utilidades
{
    public static class Llaves
    {
        public const string IssuerPropio = "app-dotnet";
        private const string SeccionLlaves = "Authentication:Schemes:Bearer:SigningKeys";
        private const string SeccionLlaves_Emisor = "Issuer";
        private const string SeccionLlaves_Valor = "Value";

        public static IEnumerable<SecurityKey> ObtenerLlave(IConfiguration configuration)
            => ObtenerLlave(configuration, IssuerPropio);

        public static IEnumerable<SecurityKey> ObtenerLlave(IConfiguration configuration, string issuer)
        {
            var signingKey = configuration.GetSection(SeccionLlaves)
                .GetChildren()
                .SingleOrDefault(llave => llave[SeccionLlaves_Emisor] == issuer);

            if (signingKey is not null && signingKey[SeccionLlaves_Valor] is string valorLlave)
            {
                yield return new SymmetricSecurityKey(Convert.FromBase64String(valorLlave));
            }
        }

        public static IEnumerable<SecurityKey> ObtenerTodasLlave(IConfiguration configuration)
        {
            var signingKeys = configuration.GetSection(SeccionLlaves)
                .GetChildren();

            foreach (var signingKey in signingKeys)
            {
                if (signingKey[SeccionLlaves_Valor] is string valorLlave)
                {
                    yield return new SymmetricSecurityKey(Convert.FromBase64String(valorLlave));
                }
            }


        }
    }
}