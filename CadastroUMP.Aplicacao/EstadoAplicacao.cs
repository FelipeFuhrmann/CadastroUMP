using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using CadastroUMP.Dominio;
using CadastroUMP.Repositorio;

namespace CadastroUMP.Aplicacao
{
    public class EstadoAplicacao
    {
        private Contexto contexto;

        public List<Estado> ListarTodos()
        {
            using (contexto = new Contexto())
            {
                var strQuery = "SELECT * FROM ESTADO";
                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return TransformaReaderEmListaDeObjeto(retornoDataReader);
            }
        }

        public Estado ListarPorId(int id)
        {
            using (contexto = new Contexto())
            {
                var strQuery = string.Format("SELECT * FROM ESTADO WHERE ESTADOID = {0} ", id);
                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return TransformaReaderEmListaDeObjeto(retornoDataReader).FirstOrDefault();
            }
        }

        private List<Estado> TransformaReaderEmListaDeObjeto(SqlDataReader reader)
        {
            var estados = new List<Estado>();
            while (reader.Read())
            {
                var temObjeto = new Estado
                {
                    EstadoId = int.Parse(reader["ESTADOID"].ToString()),
                    NomeEstado = reader["NOME_ESTADO"].ToString(),
                    UF = reader["UF"].ToString(),
                };
                estados.Add(temObjeto);
            }
            reader.Close();
            return estados;
        }
    }
}
