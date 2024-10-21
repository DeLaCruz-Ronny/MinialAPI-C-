using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace minimalAPIPeliculas.Entidades
{
    public class Genero
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string Nombre { get; set; } = null!;
    }
}