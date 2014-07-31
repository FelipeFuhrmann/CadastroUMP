using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CadastroUMP.Dominio;
using CadastroUMP.Repositorio;

namespace CadastroUMP.Aplicacao
{
    public class CidadeAplicacao
    {
        private Contexto contexto;


        public List<Cidade> ListarCidades()
        {
            using (contexto = new Contexto())
            {
                var strQuery = "SELECT * FROM CIDADE";
                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return TransformaReaderEmListaDeObjeto(retornoDataReader);
            }
        }

        public Cidade ListarPorId(int id)
        {
            using (contexto = new Contexto())
            {
                var strQuery = string.Format("SELECT * FROM CIDADE WHERE CIDADEID = {0} ", id);
                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return TransformaReaderEmListaDeObjeto(retornoDataReader).FirstOrDefault();
            }
        }

       
        public List<Cidade> ListarPorEstadoId(int idEstado)
        {
            using (contexto = new Contexto())
            {
                var strQuery = "";
                strQuery += " SELECT CIDADE.CIDADEID, CIDADE.NOME_CIDADE, ESTADO.ESTADOID, ESTADO.NOME_ESTADO ";
                strQuery += " FROM CIDADE, ESTADO ";
                strQuery += " WHERE CIDADE.ESTADOID = ESTADO.ESTADOID AND ESTADO.ESTADOID = " + idEstado;
                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return TransformaReaderEmListaDeObjeto(retornoDataReader);
            }
        }


        public List<Cidade> TransformaReaderEmListaDeObjeto(SqlDataReader reader)
        {
            var todosEstados = new EstadoAplicacao().ListarTodos();

            var cidades = new List<Cidade>();
            while (reader.Read())
            {
                int id = int.Parse(reader["ESTADOID"].ToString());
                var estado = todosEstados.FirstOrDefault(x => x.EstadoId == id);
                var temObjeto = new Cidade()
                                    {
                                        CidadeId = int.Parse(reader["CIDADEID"].ToString()),
                                        NomeCidade = reader["NOME_CIDADE"].ToString(),
                                        Estado = estado
                                    };
                cidades.Add(temObjeto);
            }
            reader.Close();
            return cidades;

        }

    }
}