using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CadastroUMP.Dominio
{
    public class Igreja
    {
        public int IgrejaId { get; set; }
        public string NomeIgreja { get; set; }

        public Federacao Federacao { get; set; }
        public Estado Estado { get; set; }
        public Cidade Cidade { get; set; }

    }
}