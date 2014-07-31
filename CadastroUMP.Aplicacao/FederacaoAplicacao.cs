using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using CadastroUMP.Dominio;
using CadastroUMP.Repositorio;

namespace CadastroUMP.Aplicacao
{
    public class FederacaoAplicacao
    {
        private Contexto contexto;

        private void InserirFederacao(Federacao federacao)
        {
            var strQuery = "";
            strQuery += "INSERT INTO FEDERACAO (NOME_FEDERACAO, SINODALID)";
            strQuery += string.Format("VALUES ('{0}','{1}')", federacao.NomeFederacao, federacao.Sinodal.SinodalId);

            //destroi o obejeto contexto depois de executar o comando.
            using (contexto = new Contexto())
            {
                contexto.ExecutaComando(strQuery);
            }

        }

        private void AlterarFederacao(Federacao federacao)
        {
            //var strQuery = string.Format(" UPDATE FEDERACAO SET NOME_FEDERACAO = '{0}', REGIONALID = '{1}', SINODALID = '{2}', ESTADOID = '{3}' WHERE FEDERACAOID = '{4}'", federacao.NomeFederacao, federacao.Regional.RegionalId, federacao.Sinodal.SinodalId, federacao.Estado.EstadoId, federacao.FederacaoId);
            var strQuery = "";
            strQuery += "UPDATE FEDERACAO SET";
            strQuery += string.Format(" NOME_FEDERACAO = '{0}', ", federacao.NomeFederacao);
            strQuery += string.Format(" SINODALID =  '{0}' ", federacao.Sinodal.SinodalId);
            strQuery += string.Format(" WHERE FEDERACAOID = {0} ", federacao.FederacaoId);
            using (contexto = new Contexto())
            {
                contexto.ExecutaComando(strQuery);
            }

        }

        public void SalvarFederacao(Federacao federacao)
        {
            if (federacao.FederacaoId > 0)
                AlterarFederacao(federacao);
            else
            {
                InserirFederacao(federacao);
            }
        }

        public void ExcluirFederacao(int id)
        {
            using (contexto = new Contexto())
            {
                var strQuery = string.Format(" DELETE FROM FEDERACAO WHERE FEDERACAOID = '{0}' ", id);
                contexto.ExecutaComando(strQuery);
            }
        }

        public List<Federacao> ListarFederacoes()
        {
            using (contexto = new Contexto())
            {
                var strQuery = "SELECT * FROM FEDERACAO";
                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return TransformaReaderEmListaDeObjeto(retornoDataReader);
            }
        }

        public List<Federacao> ListarPorSinodalId(int idSinodal)
        {
            using (contexto = new Contexto())
            {
                var strQuery = "";
                strQuery +=
                    " SELECT FEDERACAO.FEDERACAOID, FEDERACAO.NOME_FEDERACAO, SINODAL.SINODALID, SINODAL.NOME_SINODAL ";
                strQuery += " FROM FEDERACAO, SINODAL ";
                strQuery += " WHERE FEDERACAO.SINODALID = SINODAL.SINODALID AND SINODAL.SINODALID = " + idSinodal;
                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return TransformaReaderEmListaDeObjeto(retornoDataReader);
            }
        }

        public Federacao ListarPorId(int id)
        {
            using (contexto = new Contexto())
            {
                var strQuery = string.Format("SELECT * FROM FEDERACAO WHERE FEDERACAOID = {0} ", id);
                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return TransformaReaderEmListaDeObjeto(retornoDataReader).FirstOrDefault();
            }
        }

        private List<Federacao> TransformaReaderEmListaDeObjeto(SqlDataReader reader)
        {

            var todasSinodais = new SinodalAplicacao().ListarSinodais();


            var federacoes = new List<Federacao>();
            while (reader.Read())
            {
                int id2 = int.Parse(reader["SINODALID"].ToString());
                var sinodal = todasSinodais.FirstOrDefault(x => x.SinodalId == id2);

                var temObjeto = new Federacao
                                    {
                                        FederacaoId = int.Parse(reader["FEDERACAOID"].ToString()),
                                        NomeFederacao = reader["NOME_FEDERACAO"].ToString(),
                                        Sinodal = sinodal
                                    };
                federacoes.Add(temObjeto);
            }
            reader.Close();
            return federacoes;
        }

        public List<Federacao> ListarFederacoesPorSinodal(int relacionadoId)
        {
            using (contexto = new Contexto())
            {
                var strQuery = "SELECT * FROM FEDERACAO WHERE SINODALID=" + relacionadoId;
                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return TransformaReaderEmListaDeObjeto(retornoDataReader);
            }
        }

        public List<Federacao> ListarFederacoesPorSinodalLivre(int relacionadoId)
        {
            using (contexto = new Contexto())
            {
                var strQuery = "SELECT * FROM FEDERACAO WHERE SINODALID=" + relacionadoId + "AND FEDERACAO.FEDERACAOID NOT IN (SELECT  PRESIDENTE.RELACIONADOID FROM PRESIDENTE WHERE  PRESIDENTE.CARGOID = 10)";
                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return TransformaReaderEmListaDeObjeto(retornoDataReader);
            }
        }


    }
}
