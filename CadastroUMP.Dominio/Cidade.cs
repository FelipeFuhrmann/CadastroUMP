using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace CadastroUMP.Dominio
{
    public class Cidade
    {
        public int CidadeId { get; set; }

        [DisplayName("Nome da Cidade")]
        public string NomeCidade { get; set; }

   
        public Estado Estado { get; set; }
    }
}