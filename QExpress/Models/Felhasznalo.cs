using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QExpress.Models
{
    public class Felhasznalo : IdentityUser
    {
        public Felhasznalo()
        {
            Ceg = new HashSet<Ceg>();
            UgyfLevelek = new HashSet<UgyfLevelek>();
            Sorszam = new HashSet<Sorszam>();
            FelhasznaloTelephely = new HashSet<FelhasznaloTelephely>();
        }


        [Required]
        public int jogosultsagi_szint { get; set; }

        public ICollection<Ceg> Ceg { get; set; }
        public ICollection<UgyfLevelek> UgyfLevelek { get; set; }
        public ICollection<Sorszam> Sorszam { get; set; }
        public ICollection<FelhasznaloTelephely> FelhasznaloTelephely { get; set; }
    }
}
