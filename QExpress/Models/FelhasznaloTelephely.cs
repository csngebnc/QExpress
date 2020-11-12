using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QExpress.Models
{
    public class FelhasznaloTelephely
    {
        [ForeignKey("Ugyintezo")]
        [Required]
        public String FelhasznaloId { get; set; }
        public Felhasznalo Ugyintezo { get; set; }


        [ForeignKey("Telephely")]
        [Required]
        public int TelephelyId { get; set; }
        public Telephely Telephely { get; set; }
    }
}
