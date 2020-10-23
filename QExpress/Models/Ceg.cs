using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QExpress.Models
{
    public class Ceg
    {
        public Ceg()
        {
            Kategoria = new HashSet<Kategoria>();
            Telephely = new HashSet<Telephely>();
            UgyfLevelek = new HashSet<UgyfLevelek>();
        }

        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public String nev { get; set; }

        [ForeignKey("Cegadmin")]
        public String CegadminId { get; set; }
        public Felhasznalo Cegadmin { get; set; }


        public ICollection<Telephely> Telephely { get; set; }
        public ICollection<Kategoria> Kategoria { get; set; }
        public ICollection<UgyfLevelek> UgyfLevelek { get; set; }
    }
}
