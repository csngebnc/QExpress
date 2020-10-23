using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QExpress.Models
{
    public class FelhasznaloTelephely
    {
        [ForeignKey("Ugyintezo")]
        public String FelhasznaloId { get; set; }
        public Felhasznalo Ugyintezo { get; set; }


        [ForeignKey("Telephely")]
        public int TelephelyId { get; set; }
        public Telephely Telephely { get; set; }
    }
}
