using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CadastroUMP.Dominio
{
    public class Estado
    {
        public int EstadoId { get; set; }
        public string NomeEstado { get; set; }
        public string UF { get; set; }
    }
}