using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CadastroUMP.Dominio
{
    public class Federacao
    {
        public int FederacaoId { get; set; }
        public string NomeFederacao { get; set; }

        public Sinodal Sinodal { get; set; }


    }
}