using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CadastroUMP.Dominio
{
    public class Sinodal
    {
        public int SinodalId { get; set; }
        public string NomeSinodal { get; set; }

        public Regional Regional { get; set; }

    }
}