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

    public class RelatorioAplicacao
    {
        private Contexto contexto;

        private List<Relatorio> TransformaReaderEmListaDeObjeto(SqlDataReader reader)
        {

            var relatorio = new List<Relatorio>();
            while (reader.Read())
            {
                var temObjeto = new Relatorio()
                 {
                     QtdMembro = int.Parse(reader["QTD_MEMBROS"].ToString()),
                     Regional = reader["NOME_REGIONAL"].ToString(),
                 };
                relatorio.Add(temObjeto);
            }
            reader.Close();
            return relatorio;
        }

        public List<Relatorio> ListarMembrosPorRegionais()
        {
            using (contexto = new Contexto())
            {
                var strQuery = " SELECT DISTINCT RE.NOME_REGIONAL, COUNT (ME.MEMBROID) AS QTD_MEMBROS ";
                strQuery += "FROM MEMBRO ME, IGREJA IG, FEDERACAO FE , SINODAL SI, REGIONAL RE WHERE IG.FEDERACAOID = FE.FEDERACAOID AND SI.SINODALID = FE.SINODALID AND RE.REGIONALID = SI.REGIONALID AND IG.IGREJAID = ME.IGREJAID ";
                strQuery += " GROUP BY RE.NOME_REGIONAL ";
                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return TransformaReaderEmListaDeObjeto(retornoDataReader);
            }
        }



    }
}
