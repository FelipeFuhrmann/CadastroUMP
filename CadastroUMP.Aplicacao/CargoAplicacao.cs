using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using CadastroUMP.Dominio;
using CadastroUMP.Repositorio;

namespace CadastroUMP.Aplicacao
{
   public class CargoAplicacao
    {
        private Contexto contexto;

        private void InserirCargo(Cargo cargo)
        {
            var strQuery = "";
            strQuery += "INSERT INTO CARGO (NOME_CARGO, TIPO_CARGO)";
            strQuery += string.Format("VALUES ('{0}','{1}')", cargo.NomeCargo, cargo.TipoCargo);

            //destroi o obejeto contexto depois de executar o comando.
            using (contexto = new Contexto())
            {
                contexto.ExecutaComando(strQuery);
            }

        }

        private void AlterarCargo(Cargo cargo)
        {
            //var strQuery = string.Format(" UPDATE CARGO SET NOME_CARGO = '{0}', TIPO_CARGO = '{1}' WHERE CARGOID = '{2}'", cargo.NomeCargo, cargo.TipoCargo, cargo.CargoId);
            var strQuery = "";
            strQuery += "UPDATE CARGO SET";
            strQuery += string.Format(" NOME_CARGO = '{0}', ", cargo.NomeCargo);
            strQuery += string.Format(" TIPO_CARGO =  '{0}' ", cargo.TipoCargo);
            strQuery += string.Format(" WHERE CARGOID = {0} ", cargo.CargoId);
            using (contexto = new Contexto())
            {
                contexto.ExecutaComando(strQuery);
            }

        }

        public void SalvarCargo(Cargo cargo)
        {
            if (cargo.CargoId > 0)
                AlterarCargo(cargo);
            else
            {
                InserirCargo(cargo);
            }
        }

        public void ExcluirCargo(int id)
        {
            using (contexto = new Contexto())
            {
                var strQuery = string.Format(" DELETE FROM CARGO WHERE CARGOID = '{0}' ", id);
                contexto.ExecutaComando(strQuery);
            }
        }

        public List<Cargo> ListarCargos()
        {
            using (contexto = new Contexto())
            {
                var strQuery = "SELECT * FROM CARGO";
                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return TransformaReaderEmListaDeObjeto(retornoDataReader);
            }
        }

        public Cargo ListarPorId(int id)
        {
            using (contexto = new Contexto())
            {
                var strQuery = string.Format("SELECT * FROM CARGO WHERE CARGOID = {0} ", id);
                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return TransformaReaderEmListaDeObjeto(retornoDataReader).FirstOrDefault();
            }
        }
        private List<Cargo> TransformaReaderEmListaDeObjeto(SqlDataReader reader)
        {
            var cargos = new List<Cargo>();
            while (reader.Read())
            {
                var temObjeto = new Cargo()
                {
                    CargoId = int.Parse(reader["CARGOID"].ToString()),
                    NomeCargo = reader["NOME_CARGO"].ToString(),
                    TipoCargo = reader["TIPO_CARGO"].ToString()
                };
                cargos.Add(temObjeto);
            }
            reader.Close();
            return cargos;
        }

        public int ListarCargos(string p)
        {
           return ListarCargos().FirstOrDefault(x => x.TipoCargo.ToLower() == p.ToLower()).CargoId;
        }
    }
}
