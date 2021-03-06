﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QExpress.Models.DTOs
{
    public class FelhasznaloTelephelyDTO 
    {
        public FelhasznaloTelephelyDTO() { }

        public FelhasznaloTelephelyDTO(FelhasznaloTelephely ft)
        {
            FelhasznaloId = ft.FelhasznaloId;
            TelephelyId = ft.TelephelyId;
        }

        public String FelhasznaloId { get; set; }
        public int TelephelyId { get; set; }
    }
}
