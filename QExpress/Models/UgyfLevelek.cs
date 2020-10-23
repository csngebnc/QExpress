using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QExpress.Models
{
    public class UgyfLevelek
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public String Panasz { get; set; }

        [ForeignKey("Panaszolo")]
        [Required]
        public String PanaszoloId { get; set; }
        public Felhasznalo Panaszolo { get; set; }

        [ForeignKey("Ceg")]
        [Required]
        public int CegId { get; set; }
        public Ceg Ceg { get; set; }
    }
}
