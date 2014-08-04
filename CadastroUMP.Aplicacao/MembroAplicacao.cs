using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using CadastroUMP.Dominio;
using CadastroUMP.Repositorio;

namespace CadastroUMP.Aplicacao
{
    public class MembroAplicacao
    {
        private Contexto contexto;

        private void InserirMembro(Membro membro)
        {
            var strQuery = "";
            strQuery += "INSERT INTO MEMBRO (NOME_MEMBRO, DATA_NASCIMENTO, TELEFONE_MEMBRO, SEXO, EMAIL, CIDADEID, IGREJAID)";
            strQuery += string.Format("VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}')", membro.NomeMembro, membro.IdadeMembro, membro.TelefoneMembro, membro.Sexo, membro.Email, membro.Cidade.CidadeId, membro.Igreja.IgrejaId);

            //destroi o obejeto contexto depois de executar o comando.
            using (contexto = new Contexto())
            {
                contexto.ExecutaComando(strQuery);
            }

        }

        private void AlterarMembro(Membro membro)
        {
            //var strQuery = string.Format(" UPDATE CARGO SET NOME_CARGO = '{0}', TIPO_CARGO = '{1}' WHERE CARGOID = '{2}'", cargo.NomeCargo, cargo.TipoCargo, cargo.CargoId);
            var strQuery = "";
            strQuery += "UPDATE MEMBRO SET";
            strQuery += string.Format(" NOME_MEMBRO = '{0}', ", membro.NomeMembro);
            strQuery += string.Format(" DATA_NASCIMENTO = '{0}', ", membro.IdadeMembro);
            strQuery += string.Format(" TELEFONE_MEMBRO = '{0}', ", membro.TelefoneMembro);
            strQuery += string.Format(" SEXO = '{0}', ", membro.Sexo);
            strQuery += string.Format(" EMAIL = '{0}', ", membro.Email);
            strQuery += string.Format(" CIDADEID = '{0}', ", membro.Cidade.CidadeId);
            strQuery += string.Format(" IGREJAID = '{0}' ", membro.Igreja.IgrejaId);
            strQuery += string.Format(" WHERE MEMBROID = {0} ", membro.MembroId);
            using (contexto = new Contexto())
            {
                contexto.ExecutaComando(strQuery);
            }

        }

        public void SalvarMembro(Membro membro)
        {
            if (membro.MembroId > 0)
                AlterarMembro(membro);
            else
            {
                InserirMembro(membro);
            }
        }

        public void ExcluirMembro(int id)
        {
            using (contexto = new Contexto())
            {
                var strQuery = string.Format(" DELETE FROM MEMBRO WHERE MEMBROID = '{0}' ", id);
                contexto.ExecutaComando(strQuery);
            }
        }

        public List<Membro> ListarTodosMembros()
        {
            using (contexto = new Contexto())
            {
                var strQuery = "SELECT * FROM MEMBRO";
                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return TransformaReaderEmListaDeObjeto(retornoDataReader);
            }
        }

        public List<Membro> ListarMembros(string filtro = "")
        {
            using (contexto = new Contexto())
            {
                var strQuery = " SELECT MEMBROID, NOME_MEMBRO, DATA_NASCIMENTO, TELEFONE_MEMBRO, SEXO, EMAIL, CIDADEID, IGREJAID ";
                strQuery += " FROM MEMBRO WHERE MEMBRO.IGREJAID = '" + filtro + "'";

                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return TransformaReaderEmListaDeObjeto(retornoDataReader);
            }
        }

        public Membro ListarPorId(int id)
        {
            using (contexto = new Contexto())
            {
                var strQuery = string.Format("SELECT * FROM MEMBRO WHERE MEMBROID = {0} ", id);
                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return TransformaReaderEmListaDeObjeto(retornoDataReader).FirstOrDefault();
            }
        }

        public List<Membro> ListarPorNome(string nome)
        {
            using (contexto = new Contexto())
            {
                var strQuery = string.Format("SELECT * FROM MEMBRO WHERE NOME_MEMBRO LIKE '%{0}%'", nome);
                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return TransformaReaderEmListaDeObjeto(retornoDataReader);
            }
        }

        private List<Membro> TransformaReaderEmListaDeObjeto(SqlDataReader reader)
        {
            var todasCidades = new CidadeAplicacao().ListarCidades();
            var todasIgrejas = new IgrejaAplicacao().ListarIgrejas();


            var membros = new List<Membro>();
            while (reader.Read())
            {
                int id1 = int.Parse(reader["CIDADEID"].ToString());
                var cidade = todasCidades.FirstOrDefault(x => x.CidadeId == id1);

                int id6 = int.Parse(reader["IGREJAID"].ToString());
                var igreja = todasIgrejas.FirstOrDefault(x => x.IgrejaId == id6);

                var temObjeto = new Membro
                {
                    MembroId = int.Parse(reader["MEMBROID"].ToString()),
                    NomeMembro = reader["NOME_MEMBRO"].ToString(),
                    IdadeMembro = DateTime.Parse(reader["DATA_NASCIMENTO"].ToString()),
                    TelefoneMembro = reader["TELEFONE_MEMBRO"].ToString(),
                    Sexo = reader["SEXO"].ToString(),
                    Email = reader["EMAIL"].ToString(),
                    Cidade = cidade,
                    Igreja = igreja

                };
                membros.Add(temObjeto);
            }
            reader.Close();
            return membros;
        }

    }
}
