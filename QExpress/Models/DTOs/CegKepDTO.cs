using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace QExpress.Models.DTOs
{
    public class CegKepDTO
    {
        public CegKepDTO() { }
        public CegKepDTO(Ceg c)
        {
            Id = c.Id;
            Nev = c.nev;
            CegadminId = c.CegadminId;
        }

        public int Id { get; set; }
        public String Nev { get; set; }
        public String CegadminId { get; set; }
        public IFormFile image { get; set; }
    }
}
