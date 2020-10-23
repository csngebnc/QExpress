using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QExpress.Models.DTOs
{
    public class FelhasznaloDTO
    {
        public FelhasznaloDTO(Felhasznalo f)
        {
            Id = f.Id;
            Email = f.Email;
            UserName = f.UserName;
            jogosultsagi_szint = f.jogosultsagi_szint;
        }

        public String Id { get; set; }
        public String Email { get; set; }
        public String UserName { get; set; }
        public int jogosultsagi_szint { get; set; }
    
    }
}
