using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QExpress.Models
{
    public class Kategoria
    {
        public Kategoria()
        {
            Sorszam = new HashSet<Sorszam>();
        }

        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public String Megnevezes { get; set; }

        [ForeignKey("Ceg")]
        public int CegId { get; set; }
        public Ceg Ceg { get; set; }


        public ICollection<Sorszam> Sorszam { get; set; }
    }
}
