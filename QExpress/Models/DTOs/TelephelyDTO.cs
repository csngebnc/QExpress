using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QExpress.Models.DTOs
{
    public class TelephelyDTO
    {
        public TelephelyDTO() { }
        public TelephelyDTO(Telephely t)
        {
            Id = t.Id;
            Cim = t.Cim;
            Ceg_id = t.Ceg_id;
        }


        public int Id { get; set; }
        public string Cim { get; set; }
        public int Ceg_id { get; set; }
    }
}
