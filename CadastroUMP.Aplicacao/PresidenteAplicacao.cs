using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using CadastroUMP.Dominio;
using CadastroUMP.Repositorio;

namespace CadastroUMP.Aplicacao
{
    public class PresidenteAplicacao
    {
        private Contexto contexto;

        private void InserirPresidente(Presidente presidente)
        {
            var strQuery = "";
            strQuery += "INSERT INTO PRESIDENTE (NOME_PRESIDENTE, SEXO, EMAIL, VIGENCIA_INICIO, VIGENCIA_FINAL, USUARIO, SENHA, CARGOID, RELACIONADOID)";
            strQuery += string.Format("VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')", presidente.NomePresidente, presidente.Sexo, presidente.Email, presidente.VigenciaInicio, presidente.VigenciaFinal, presidente.Usuario, presidente.Senha, presidente.Cargo.CargoId, presidente.RelacionadoId);

            //destroi o obejeto contexto depois de executar o comando.
            using (contexto = new Contexto())
            {
                contexto.ExecutaComando(strQuery);
            }

        }

        private void AlterarPresidente(Presidente presidente)
        {
            //var strQuery = string.Format(" UPDATE CARGO SET NOME_CARGO = '{0}', TIPO_CARGO = '{1}' WHERE CARGOID = '{2}'", cargo.NomeCargo, cargo.TipoCargo, cargo.CargoId);
            var strQuery = "";
            strQuery += "UPDATE PRESIDENTE SET";
            strQuery += string.Format(" NOME_PRESIDENTE = '{0}', ", presidente.NomePresidente);
            strQuery += string.Format(" SEXO = '{0}', ", presidente.Sexo);
            strQuery += string.Format(" EMAIL = '{0}', ", presidente.Email);
            strQuery += string.Format(" VIGENCIA_INICIO = '{0}', ", presidente.VigenciaInicio);
            strQuery += string.Format(" VIGENCIA_FINAL = '{0}', ", presidente.VigenciaFinal);
            strQuery += string.Format(" USUARIO = '{0}', ", presidente.Usuario);
            strQuery += string.Format(" SENHA = '{0}', ", presidente.Senha);
            strQuery += string.Format(" CARGOID = '{0}', ", presidente.Cargo.CargoId);
            strQuery += string.Format(" RELACIONADOID =  '{0}' ", presidente.RelacionadoId);
            strQuery += string.Format(" WHERE PRESIDENTEID = {0} ", presidente.PresidenteId);
            using (contexto = new Contexto())
            {
                contexto.ExecutaComando(strQuery);
            }

        }

        public void SalvarPresidente(Presidente presidente)
        {
            if (presidente.PresidenteId > 0)
                AlterarPresidente(presidente);
            else
            {
                InserirPresidente(presidente);
            }
        }

        public void ExcluirPresidente(int id)
        {
            using (contexto = new Contexto())
            {
                var strQuery = string.Format(" DELETE FROM PRESIDENTE WHERE PRESIDENTEID = '{0}' ", id);
                contexto.ExecutaComando(strQuery);
            }
        }

        public List<Presidente> ListarPresidentes(string tipoCargo = "", string filtro = "")
        {
            using (contexto = new Contexto())
            {
                var strQuery = " SELECT DISTINCT PRESIDENTEID, NOME_PRESIDENTE, SEXO, EMAIL, VIGENCIA_INICIO, VIGENCIA_FINAL, PRESIDENTE.CARGOID, PRESIDENTE.RELACIONADOID ";
                strQuery += " FROM PRESIDENTE, CARGO ";


                if (!string.IsNullOrEmpty(filtro))
                {
                    switch (tipoCargo)
                    {
                        case "regional":
                            strQuery += " WHERE TIPO_CARGO = '" + tipoCargo + "' AND PRESIDENTE.CARGOID = CARGO.CARGOID ";
                            break;

                        case "sinodal":
                            strQuery += " , REGIONAL, SINODAL where SINODAL.REGIONALID = REGIONAL.REGIONALID AND PRESIDENTE.RELACIONADOID " +
                                        "IN (Select sinodalid from sinodal  where sinodal.REGIONALID = '" + filtro + "') ";
                            break;

                        case "federação":
                            strQuery += " , SINODAL, FEDERACAO where FEDERACAO.SINODALID = SINODAL.SINODALID AND PRESIDENTE.RELACIONADOID " +
                                        "IN (Select federacaoid from federacao where federacao.sinodalid = '" + filtro + "') ";
                            break;

                        case "local":
                            strQuery += " , FEDERACAO, IGREJA where IGREJA.FEDERACAOID = FEDERACAO.FEDERACAOID AND PRESIDENTE.RELACIONADOID " +
                                        "IN (Select igrejaid from igreja  where igreja.federacaoid = '" + filtro + "') ";
                            break;
                    }

                    if (!string.IsNullOrEmpty(tipoCargo))
                    {
                        strQuery += " AND TIPO_CARGO = '" + tipoCargo + "' AND PRESIDENTE.CARGOID = CARGO.CARGOID ";
                    }

                }


                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return TransformaReaderEmListaDeObjeto(retornoDataReader);
            }
        }


        public Presidente ListarPorId(int id)
        {
            using (contexto = new Contexto())
            {
                var strQuery = string.Format("SELECT * FROM PRESIDENTE WHERE PRESIDENTEID = {0} ", id);
                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return TransformaReaderEmListaDeObjeto(retornoDataReader).FirstOrDefault();
            }
        }

        public List<Presidente> ListarRegionalPorCargoID(int idCargo)
        {
            using (contexto = new Contexto())
            {
                var strQuery = "";
                strQuery += " REGIONAL.REGIONALID, REGIONAL.NOME_REGIONAL, CARGO.CARGOID, CARGO.TIPO_CARGO ";
                strQuery += " FROM REGIONAL, CARGO";
                strQuery += " WHERE CARGO.TIPO_CARGO = 'Regional'";
                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return TransformaReaderEmListaDeObjeto(retornoDataReader);
            }
        }

        public List<Presidente> ListarTodos(string tipoCargo = "", string filtro = "")
        {
            {
                using (contexto = new Contexto())
                {
                    var strQuery = " SELECT PRESIDENTEID, NOME_PRESIDENTE, SEXO, EMAIL, VIGENCIA_INICIO, VIGENCIA_FINAL, PRESIDENTE.CARGOID, PRESIDENTE.RELACIONADOID ";
                    strQuery += " FROM PRESIDENTE, CARGO ";


                    if (!string.IsNullOrEmpty(filtro))
                    {
                        switch (tipoCargo)
                        {
                            case "nacional":
                                strQuery += " WHERE PRESIDENTE.CARGOID = CARGO.CARGOID ORDER BY TIPO_CARGO ";
                                break;

                            case "regional":
                                strQuery += " ";
                                break;

                            case "federação":
                                strQuery += " , ";
                                break;

                            case "local":
                                strQuery += " ,  ";
                                break;
                        }
                        if (!string.IsNullOrEmpty(tipoCargo))
                        {
                            strQuery += " ";
                        }

                    }

                    var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                    return TransformaReaderEmListaDeObjeto(retornoDataReader);
                }
            }

        }



        public Presidente Logar(string usuario, string senha)
        {
            using (contexto = new Contexto())
            {
                var strQuery = string.Format("SELECT * FROM PRESIDENTE WHERE USUARIO = '{0}' AND SENHA = '{1}' ", usuario, senha);
                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return TransformaReaderEmListaDeObjeto(retornoDataReader).FirstOrDefault();
            }
        }

        private List<Presidente> TransformaReaderEmListaDeObjeto(SqlDataReader reader)
        {
            var todosCargos = new CargoAplicacao().ListarCargos();

            var presidentes = new List<Presidente>();
            while (reader.Read())
            {
                int id = int.Parse(reader["CARGOID"].ToString());
                var cargo = todosCargos.FirstOrDefault(x => x.CargoId == id);

                var temObjeto = new Presidente();

                temObjeto.PresidenteId = int.Parse(reader["PRESIDENTEID"].ToString());
                temObjeto.NomePresidente = reader["NOME_PRESIDENTE"].ToString();
                temObjeto.Sexo = char.Parse(reader["SEXO"].ToString());
                temObjeto.Email = reader["EMAIL"].ToString();
                temObjeto.VigenciaInicio = DateTime.Parse(reader["VIGENCIA_INICIO"].ToString());
                temObjeto.VigenciaFinal = DateTime.Parse(reader["VIGENCIA_FINAL"].ToString());
                temObjeto.Cargo = cargo;

                if (cargo != null)

                    switch (cargo.TipoCargo.ToLower())
                    {
                        case "regional":
                            var obj = new RegionalAplicacao().ListarPorId(int.Parse(reader["RELACIONADOID"].ToString()));
                            temObjeto.Relacionado = new Relacionado() { Id = obj.RegionalId, Descricao = obj.NomeRegional };
                            break;

                        case "sinodal":
                            var obj1 = new SinodalAplicacao().ListarPorId(int.Parse(reader["RELACIONADOID"].ToString()));
                            temObjeto.Relacionado = new Relacionado() { Id = obj1.SinodalId, Descricao = obj1.NomeSinodal };
                            break;

                        case "federação":
                            var obj2 = new FederacaoAplicacao().ListarPorId(int.Parse(reader["RELACIONADOID"].ToString()));
                            temObjeto.Relacionado = new Relacionado() { Id = obj2.FederacaoId, Descricao = obj2.NomeFederacao };
                            break;

                        case "local":
                            var obj3 = new IgrejaAplicacao().ListarPorId(int.Parse(reader["RELACIONADOID"].ToString()));
                            temObjeto.Relacionado = new Relacionado() { Id = obj3.IgrejaId, Descricao = obj3.NomeIgreja };
                            break;
                    }

                temObjeto.RelacionadoId = int.Parse(reader["RELACIONADOID"].ToString());
                presidentes.Add(temObjeto);
            }
            reader.Close();
            return presidentes;
        }


    }
}
