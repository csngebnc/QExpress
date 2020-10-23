using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QExpress.Models.DTOs
{
    public class KategoriaDTO
    {
        public KategoriaDTO(Kategoria k)
        {
            Id = k.Id;
            Megnevezes = k.Megnevezes;
            Ceg = k.Ceg;
        }

        public int Id { get; set; }
        public String Megnevezes { get; set; }
        public Ceg Ceg { get; set; }
    }
}
