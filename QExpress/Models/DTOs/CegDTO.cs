using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QExpress.Models.DTOs
{
    public class CegDTO
    {
        public CegDTO() { }
        public CegDTO(Ceg c)
        {
            Id = c.Id;
            Nev = c.nev;
            CegadminId = c.CegadminId;
        }

        public int Id { get; set; }
        public String Nev { get; set; }
        public String CegadminId { get; set; }
    }
}
