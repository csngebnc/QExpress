using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QExpress.Models.DTOs
{
    public class CegDTO
    {
        public CegDTO(Ceg c)
        {
            Id = c.Id;
            Nev = c.nev;
            Cegadmin = c.Cegadmin;
        }

        public int Id { get; set; }
        public String Nev { get; set; }
        public Felhasznalo Cegadmin { get; set; }
    }
}
