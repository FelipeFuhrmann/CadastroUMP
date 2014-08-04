using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CadastroUMP.Dominio
{
    public class Membro 
    {
        public int MembroId { get; set; }
        
        [DisplayName("Nome do Membro")]
        public string NomeMembro { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime IdadeMembro { get; set; }

        public string TelefoneMembro { get; set; }
        public string Sexo { get; set; }
        public string Email { get; set; }
        public Cidade Cidade { get; set; }
        public Igreja Igreja { get; set; }

       

        //public IEnumerator GetEnumerator()
        //{
        //    throw new NotImplementedException();
        //}
    }
}