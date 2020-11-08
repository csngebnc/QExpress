using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QExpress.Models.DTOs
{
    public class KategoriaDTO
    {
        public KategoriaDTO() { }
        public KategoriaDTO(Kategoria k)
        {
            Id = k.Id;
            Megnevezes = k.Megnevezes;
            CegId = k.CegId;
        }

        public int Id { get; set; }
        public String Megnevezes { get; set; }
        public int CegId { get; set; }
    }
}
