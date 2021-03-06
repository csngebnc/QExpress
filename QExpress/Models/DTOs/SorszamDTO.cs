﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QExpress.Models.DTOs
{
    public class SorszamDTO
    {
        public SorszamDTO() { }
        public SorszamDTO(Sorszam s)
        {
            Id = s.Id;
            SorszamIdTelephelyen = s.SorszamIdTelephelyen;
            Idopont = s.Idopont;
            Allapot = s.Allapot;
            UgyfelId = s.UgyfelId;
            TelephelyId = s.TelephelyId;
            KategoriaId = s.KategoriaId;
        }


        public int Id { get; set; }
        public int SorszamIdTelephelyen { get; set; }
        public String UgyfelId { get; set; }
        public int TelephelyId { get; set; }
        public int KategoriaId { get; set; }
        public DateTime Idopont { get; set; }
        public string Allapot { get; set; }


        public int SorbanAllokSzama { get; set; }
        public String Telephely { get; set; }
        public String Kategoria { get; set; }
        public String Ugyfel { get; set; }
        public String Ceg { get; set; }
        public int CegId { get; set; }
    }
}
