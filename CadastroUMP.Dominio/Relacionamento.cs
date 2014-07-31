using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastroUMP.Dominio
{
    class Relacionamento
    {
        public int RelacionadoId { get; set; }
        public Regional Regional { get; set; }

        public Sinodal Sinodal { get; set; }

        public Federacao Federacao { get; set; }

        public Igreja Igreja { get; set; }
    }
}
