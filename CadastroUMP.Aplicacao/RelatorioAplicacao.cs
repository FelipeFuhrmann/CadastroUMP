using System.Collections.Generic;
using System.Data.SqlClient;
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
                     Setor = reader["NOME_REGIONAL"].ToString(),
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

        private List<Relatorio> TransformaReaderEmListaDeObjeto2(SqlDataReader reader)
        {

            var relatorio = new List<Relatorio>();
            while (reader.Read())
            {
                var temObjeto = new Relatorio()
                {
                    QtdMembro = int.Parse(reader["QTD_MEMBROS"].ToString()),
                    Setor = reader["NOME_SINODAL"].ToString(),
                };
                relatorio.Add(temObjeto);
            }
            reader.Close();
            return relatorio;
        }

        
        public List<Relatorio> ListarMembrosPorSinodais(string relacionadoId = "" )
        {
            using (contexto = new Contexto())
            {
                var strQuery = " SELECT DISTINCT SI.NOME_SINODAL, COUNT (ME.MEMBROID) AS QTD_MEMBROS ";
                strQuery += " FROM MEMBRO ME, IGREJA IG, FEDERACAO FE , SINODAL SI, REGIONAL RE WHERE  RE.REGIONALID= '" + relacionadoId + "' AND RE.REGIONALID = SI.REGIONALID AND IG.FEDERACAOID = FE.FEDERACAOID AND SI.SINODALID = FE.SINODALID AND IG.IGREJAID = ME.IGREJAID ";
                strQuery += " GROUP BY SI.NOME_SINODAL ";
                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return TransformaReaderEmListaDeObjeto2(retornoDataReader);
            }
        }

        private List<Relatorio> TransformaReaderEmListaDeObjeto3(SqlDataReader reader)
        {

            var relatorio = new List<Relatorio>();
            while (reader.Read())
            {
                var temObjeto = new Relatorio()
                {
                    QtdMembro = int.Parse(reader["QTD_MEMBROS"].ToString()),
                    Setor = reader["NOME_FEDERACAO"].ToString(),
                };
                relatorio.Add(temObjeto);
            }
            reader.Close();
            return relatorio;
        }

        public List<Relatorio> ListarMembrosPorFederacao(string relacionadoId = "")
        {
            using (contexto = new Contexto())
            {
                var strQuery = " SELECT DISTINCT FE.NOME_FEDERACAO, COUNT (ME.MEMBROID) AS QTD_MEMBROS ";
                strQuery += " FROM MEMBRO ME, IGREJA IG, FEDERACAO FE , SINODAL SI, REGIONAL RE WHERE  SI.SINODALID= '" + relacionadoId + "' AND RE.REGIONALID = SI.REGIONALID AND IG.FEDERACAOID = FE.FEDERACAOID AND SI.SINODALID = FE.SINODALID AND IG.IGREJAID = ME.IGREJAID ";
                strQuery += " GROUP BY FE.NOME_FEDERACAO ";
                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return TransformaReaderEmListaDeObjeto3(retornoDataReader);
            }
        }



    }
}
