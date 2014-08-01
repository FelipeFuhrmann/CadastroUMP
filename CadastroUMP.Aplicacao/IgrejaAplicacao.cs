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
    public class IgrejaAplicacao
    {
        private Contexto contexto;

        private void InserirIgreja(Igreja igreja)
        {
            var strQuery = "";
            strQuery += "INSERT INTO IGREJA (NOME_IGREJA, FEDERACAOID, ESTADOID, CIDADEID)";
            strQuery += string.Format("VALUES ('{0}','{1}','{2}','{3}')", igreja.NomeIgreja, igreja.Federacao.FederacaoId, igreja.Estado.EstadoId, igreja.Cidade.CidadeId);

            //destroi o obejeto contexto depois de executar o comando.
            using (contexto = new Contexto())
            {
                contexto.ExecutaComando(strQuery);
            }

        }

        private void AlterarIgreja(Igreja igreja)
        {
            //var strQuery = string.Format(" UPDATE CARGO SET NOME_CARGO = '{0}', TIPO_CARGO = '{1}' WHERE CARGOID = '{2}'", cargo.NomeCargo, cargo.TipoCargo, cargo.CargoId);
            var strQuery = "";
            strQuery += "UPDATE IGREJA SET";
            strQuery += string.Format(" NOME_IGREJA = '{0}', ", igreja.NomeIgreja);
            strQuery += string.Format(" FEDERACAOID =  {0}, ", igreja.Federacao.FederacaoId);
            strQuery += string.Format(" ESTADOID =  {0}, ", igreja.Estado.EstadoId);
            strQuery += string.Format(" CIDADEID =  {0} ", igreja.Cidade.CidadeId);
            strQuery += string.Format(" WHERE IGREJAID = {0} ", igreja.IgrejaId);
            using (contexto = new Contexto())
            {
                contexto.ExecutaComando(strQuery);
            }

        }

        public void SalvarIgreja(Igreja igreja)
        {
            if (igreja.IgrejaId > 0)
                AlterarIgreja(igreja);
            else
            {
                InserirIgreja(igreja);
            }
        }

        public void ExcluirIgreja(int id)
        {
            using (contexto = new Contexto())
            {
                var strQuery = string.Format(" DELETE FROM IGREJA WHERE IGREJAID = '{0}' ", id);
                contexto.ExecutaComando(strQuery);
            }
        }

        public List<Igreja> ListarIgrejas()
        {
            using (contexto = new Contexto())
            {
                var strQuery = "SELECT * FROM IGREJA";
                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return TransformaReaderEmListaDeObjeto(retornoDataReader);
            }
        }

        public Igreja ListarPorId(int id)
        {
            using (contexto = new Contexto())
            {
                var strQuery = string.Format("SELECT * FROM IGREJA WHERE IGREJAID = {0} ", id);
                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return TransformaReaderEmListaDeObjeto(retornoDataReader).FirstOrDefault();
            }
        }

        private List<Igreja> TransformaReaderEmListaDeObjeto(SqlDataReader reader)
        {
            var todasFederacoes = new FederacaoAplicacao().ListarFederacoes();
            var todosEstados = new EstadoAplicacao().ListarTodos();
            var todasCidades = new CidadeAplicacao().ListarCidades();

            var igrejas = new List<Igreja>();
            while (reader.Read())
            {
                int id1 = int.Parse(reader["FEDERACAOID"].ToString());
                var federacao = todasFederacoes.FirstOrDefault(x => x.FederacaoId == id1);

                int id4 = int.Parse(reader["ESTADOID"].ToString());
                var estado = todosEstados.FirstOrDefault(x => x.EstadoId == id4);

                int id5 = int.Parse(reader["CIDADEID"].ToString());
                var cidade = todasCidades.FirstOrDefault(x => x.CidadeId == id5);

                var temObjeto = new Igreja
                {
                    IgrejaId = int.Parse(reader["IGREJAID"].ToString()),
                    NomeIgreja = reader["NOME_IGREJA"].ToString(),
                    Federacao = federacao,
                    Estado = estado,
                    Cidade = cidade
                };
                igrejas.Add(temObjeto);
            }
            reader.Close();
            return igrejas;
        }


        public List<Igreja> ListarIgrejasPorFederacao(int relacionadoId)
        {
            using (contexto = new Contexto())
            {
                var strQuery = "SELECT * FROM IGREJA WHERE FEDERACAOID=" + relacionadoId;
                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return TransformaReaderEmListaDeObjeto(retornoDataReader);
            }
        }

        public List<Igreja> ListarIgrejasPorFederacaoLivre(int relacionadoId)
        {
            using (contexto = new Contexto())
            {
                var strQuery = "SELECT * FROM IGREJA WHERE FEDERACAOID=" + relacionadoId + "AND IGREJA.IGREJAID NOT IN (SELECT  PRESIDENTE.RELACIONADOID FROM PRESIDENTE WHERE  PRESIDENTE.CARGOID = 5)";
                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return TransformaReaderEmListaDeObjeto(retornoDataReader);
            }
        }
    }
}
