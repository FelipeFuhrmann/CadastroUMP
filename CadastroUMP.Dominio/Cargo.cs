using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace CadastroUMP.Dominio
{
    public class Cargo
    {
        public int CargoId { get; set; }

        [DisplayName("Nome do Cargo")]
        public string NomeCargo { get; set; }

        [DisplayName("Tipo do Cargo")]
        public string TipoCargo { get; set; }


    }
}