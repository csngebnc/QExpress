using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QExpress.Models
{
    public class Telephely
    {
        public Telephely()
        {
            Sorszam = new HashSet<Sorszam>();
            FelhasznaloTelephely = new HashSet<FelhasznaloTelephely>();
        }

        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public String Cim { get; set; }

        [ForeignKey("Ceg")]
        [Required]
        public int Ceg_id { get; set; }
        public Ceg Ceg { get; set; }


        public ICollection<Sorszam> Sorszam { get; set; }
        public ICollection<FelhasznaloTelephely> FelhasznaloTelephely { get; set; }
    }
}
