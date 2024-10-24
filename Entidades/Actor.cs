using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace minimalAPIPeliculas.Entidades
{
    public class Actor
    {
        public int Id { get; set; }
        [StringLength(150)]
        public string Nombre { get; set; } = null!;
        public DateTime FechaNacimiento { get; set; }
        [Unicode(true)]
        public string? Foto { get; set; }
    }
}