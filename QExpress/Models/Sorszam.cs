using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QExpress.Models
{
    public class Sorszam
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public int SorszamIdTelephelyen { get; set; }
        [Required]
        public DateTime Idopont { get; set; }
        [Required]
        public String Allapot { get; set; }

        [ForeignKey("Ugyfel")]
        [Required]
        public String UgyfelId { get; set; }
        public Felhasznalo Ugyfel { get; set; }

        [ForeignKey("Telephely")]
        [Required]
        public int TelephelyId { get; set; }
        public Telephely Telephely { get; set; }

        [ForeignKey("Kategoria")]
        [Required]
        public int KategoriaId { get; set; }
        public Kategoria Kategoria { get; set; }
    }
}
