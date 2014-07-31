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
    public class SinodalAplicacao
    {
        private Contexto contexto;

        private void InserirSinodal(Sinodal sinodal)
        {
            var strQuery = "";
            strQuery += "INSERT INTO SINODAL (NOME_SINODAL, REGIONALID)";
            strQuery += string.Format("VALUES ('{0}','{1}')", sinodal.NomeSinodal, sinodal.Regional.RegionalId);

            //destroi o obejeto contexto depois de executar o comando.
            using (contexto = new Contexto())
            {
                contexto.ExecutaComando(strQuery);
            }

        }

        private void AlterarSinodal(Sinodal sinodal)
        {
            //var strQuery = string.Format(" UPDATE CARGO SET NOME_CARGO = '{0}', TIPO_CARGO = '{1}' WHERE CARGOID = '{2}'", cargo.NomeCargo, cargo.TipoCargo, cargo.CargoId);
            var strQuery = "";
            strQuery += "UPDATE SINODAL SET";
            strQuery += string.Format(" NOME_SINODAL = '{0}', ", sinodal.NomeSinodal);
            strQuery += string.Format(" REGIONALID =  '{0}' ", sinodal.Regional.RegionalId);
            strQuery += string.Format(" WHERE SINODALID = {0} ", sinodal.SinodalId);
            using (contexto = new Contexto())
            {
                contexto.ExecutaComando(strQuery);
            }

        }

        public void SalvarSinodal(Sinodal sinodal)
        {
            if (sinodal.SinodalId > 0)
                AlterarSinodal(sinodal);
            else
            {
                InserirSinodal(sinodal);
            }
        }

        public void ExcluirSinodal(int id)
        {
            using (contexto = new Contexto())
            {
                var strQuery = string.Format(" DELETE FROM SINODAL WHERE SINODALID = '{0}' ", id);
                contexto.ExecutaComando(strQuery);
            }
        }

        public List<Sinodal> ListarSinodais()
        {
            using (contexto = new Contexto())
            {
                var strQuery = "SELECT * FROM SINODAL";
                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return TransformaReaderEmListaDeObjeto(retornoDataReader);
            }
        }

        public Sinodal ListarPorId(int id)
        {
            using (contexto = new Contexto())
            {
                var strQuery = string.Format("SELECT * FROM SINODAL WHERE SINODALID = {0} ", id);
                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return TransformaReaderEmListaDeObjeto(retornoDataReader).FirstOrDefault();
            }
        }

        public List<Sinodal> ListarPorRegionalId(int idRegional)
        {
            using (contexto = new Contexto())
            {
                var strQuery = "";
                strQuery +=
                    " SELECT SINODAL.SINODALID, SINODAL.NOME_SINODAL, REGIONAL.REGIONALID, REGIONAL.NOME_REGIONAL ";
                strQuery += " FROM SINODAL, REGIONAL ";
                strQuery += " WHERE SINODAL.REGIONALID = REGIONAL.REGIONALID AND REGIONAL.REGIONALID = " + idRegional;
                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return TransformaReaderEmListaDeObjeto(retornoDataReader);
            }
        }

        private List<Sinodal> TransformaReaderEmListaDeObjeto(SqlDataReader reader)
        {
            var todasRegionais = new RegionalAplicacao().ListarRegionais();

            var sinodais = new List<Sinodal>();
            while (reader.Read())
            {
                int id = int.Parse(reader["REGIONALID"].ToString());
                var regional = todasRegionais.FirstOrDefault(x => x.RegionalId == id);

                var temObjeto = new Sinodal
                                    {
                                        SinodalId = int.Parse(reader["SINODALID"].ToString()),
                                        NomeSinodal = reader["NOME_SINODAL"].ToString(),
                                        Regional = regional
                                    };
                sinodais.Add(temObjeto);
            }
            reader.Close();
            return sinodais;
        }

        public List<Sinodal> ListarSinodaisPorRegiao(int relacionadoId)
        {
            using (contexto = new Contexto())
            {
                var strQuery = "SELECT * FROM SINODAL WHERE REGIONALID=" + relacionadoId;
                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return TransformaReaderEmListaDeObjeto(retornoDataReader);
            }

        }

        public List<Sinodal> ListarSinodaisPorRegiaoLivre(int relacionadoId)
        {
            using (contexto = new Contexto())
            {
                var strQuery = "SELECT * FROM SINODAL WHERE REGIONALID=" + relacionadoId + "AND SINODAL.SINODALID NOT IN (SELECT  PRESIDENTE.RELACIONADOID FROM PRESIDENTE WHERE  PRESIDENTE.CARGOID = 7)";
                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return TransformaReaderEmListaDeObjeto(retornoDataReader);
            }

        }
    }
}
