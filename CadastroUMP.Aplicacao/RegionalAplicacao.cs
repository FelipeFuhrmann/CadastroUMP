using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using CadastroUMP.Dominio;
using CadastroUMP.Repositorio;

namespace CadastroUMP.Aplicacao
{
   public class RegionalAplicacao
    {
        private Contexto contexto;

        private void InserirRegional(Regional regional)
        {
            var strQuery = "";
            strQuery += "INSERT INTO REGIONAL (NOME_REGIONAL)";
            strQuery += string.Format("VALUES ('{0}')", regional.NomeRegional);

            //destroi o obejeto contexto depois de executar o comando.
            using (contexto = new Contexto())
            {
                contexto.ExecutaComando(strQuery);
            }

        }

        private void AlterarRegional(Regional regional)
        {
            var strQuery = string.Format(" UPDATE REGIONAL SET NOME_REGIONAL = '{0}' WHERE REGIONALID = '{1}'", regional.NomeRegional, regional.RegionalId);
            //var strQuery = "";
            //strQuery += "UPDATE REGIONAL SET";
            //strQuery += string.Format(" NOME_REGIONAL = '{0}', ", regional.NomeRegional);
            //strQuery += string.Format(" WHERE REGIONALID = {0} " , regional.RegionalId);
            using (contexto = new Contexto())
            {
                contexto.ExecutaComando(strQuery);
            }

        }

        public void SalvarRegional(Regional regional)
        {
            if (regional.RegionalId > 0)
                AlterarRegional(regional);
            else
            {
                InserirRegional(regional);
            }
        }

        public void ExcluirRegional(int id)
        {
            using (contexto = new Contexto())
            {
                var strQuery = string.Format(" DELETE FROM REGIONAL WHERE REGIONALID = '{0}' ", id);
                contexto.ExecutaComando(strQuery);
            }
        }

        

        public List<Regional> ListarRegionais()
        {
            using (contexto = new Contexto())
            {
                var strQuery = "SELECT * FROM REGIONAL";
                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return TransformaReaderEmListaDeObjeto(retornoDataReader);
            }
        }

        public List<Regional> ListarRegionaisLivres()
        {
            using (contexto = new Contexto())
            {
                var strQuery = "SELECT * FROM REGIONAL WHERE REGIONAL.REGIONALID NOT IN (SELECT  PRESIDENTE.RELACIONADOID FROM PRESIDENTE WHERE  PRESIDENTE.CARGOID = 2)";
                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return TransformaReaderEmListaDeObjeto(retornoDataReader);
            }
        }


        public Regional ListarPorId(int id)
        {
            using (contexto = new Contexto())
            {
                var strQuery = string.Format("SELECT * FROM REGIONAL WHERE REGIONALID = {0} ", id);
                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return TransformaReaderEmListaDeObjeto(retornoDataReader).FirstOrDefault();
            }
        }
        private List<Regional> TransformaReaderEmListaDeObjeto(SqlDataReader reader)
        {
            var regionais = new List<Regional>();
            while (reader.Read())
            {
                var temObjeto = new Regional
                {
                    RegionalId = int.Parse(reader["REGIONALID"].ToString()),
                    NomeRegional = reader["NOME_REGIONAL"].ToString()
                };
                regionais.Add(temObjeto);
            }
            reader.Close();
            return regionais;
        }
    }
}
