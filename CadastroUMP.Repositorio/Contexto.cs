using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastroUMP.Repositorio
{
    public class Contexto : IDisposable
    {
        //Criando conexão com o banco de dados
        private readonly SqlConnection minhaConexao;
        public Contexto()
        {
            minhaConexao = new SqlConnection(ConfigurationManager.ConnectionStrings["CadastroUMPConfig"].ConnectionString);
            minhaConexao.Open();
        }

        //Comando que não retorna dados = Insert, Delete, Update
        public void ExecutaComando(string strQuery)
        {
            var cmdComando = new SqlCommand
            {
                CommandText = strQuery,
                CommandType = CommandType.Text,
                Connection = minhaConexao
            };
            cmdComando.ExecuteNonQuery();
        }


        //Comando que retorna dados = Select
        public SqlDataReader ExecutaComandoComRetorno(string strQuery)
        {
            var cmdComando = new SqlCommand(strQuery, minhaConexao);
            return cmdComando.ExecuteReader();
        }

        // *** Dispose -> fecha a conexao do banco de dados após ser utilizada!!!
        public void Dispose()
        {
            if (minhaConexao.State == ConnectionState.Open)
                minhaConexao.Close();
        }
    }
}
