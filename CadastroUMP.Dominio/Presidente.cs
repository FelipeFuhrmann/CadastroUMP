using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CadastroUMP.Dominio
{
    public class Presidente
    {
        public int PresidenteId { get; set; }
       
        [DisplayName("Nome do Presidente")]
        public string NomePresidente { get; set; }
        
        [DisplayName("Sexo")]
        public char Sexo { get; set; }

        [DisplayName("E-mail")]
        public string Email { get; set; }
        
        [DisplayName("Vigência do Cargo Incial")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime VigenciaInicio { get; set; }

        [DisplayName("Vigência do Cargo Final")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime VigenciaFinal { get; set; }
        public string Usuario { get; set; }
        public string Senha { get; set; }

        public Cargo Cargo { get; set; }

        [DisplayName("Setor de Atuação")]
        public int RelacionadoId { get; set; }
        
        public Relacionado Relacionado { get; set; }

    }

    public class Relacionado
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
    }
}