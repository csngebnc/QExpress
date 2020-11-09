using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QExpress.Models.DTOs
{
    public class UgyfLevelekDTO
    {
        public UgyfLevelekDTO() { }
        public UgyfLevelekDTO(UgyfLevelek u)
        {
            Id = u.Id;
            Panasz = u.Panasz;
            PanaszoloId = u.PanaszoloId;
            CegId = u.CegId;
        }

        public int Id { get; set; }
        public string Panasz { get; set; }
        public String PanaszoloId { get; set; }
        public int CegId { get; set; }
    }
}
