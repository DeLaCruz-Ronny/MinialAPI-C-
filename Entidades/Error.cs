using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace minimalAPIPeliculas.Entidades
{
    public class Error
    {
        public Guid Id { get; set; }
        public string MensajeDeError { get; set; } = null!;
        public string? StackTrace { get; set; }
        public DateTime Fecha {get; set;}
    }
}