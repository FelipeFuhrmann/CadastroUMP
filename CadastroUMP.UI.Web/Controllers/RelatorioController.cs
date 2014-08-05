using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CadastroUMP.Aplicacao;
using CadastroUMP.UI.Web.Helpers;
using Rotativa;
using Rotativa.Options;

namespace CadastroUMP.UI.Web.Controllers
{
    public class RelatorioController : Controller
    {

        [Seguranca (Permissao = "nacional")]
        public ActionResult PDFPadrao()
        {
            var tipoCargo = Session["permissao"].ToString().ToLower();
            var relacionadoId = Session["relacionadoId"].ToString();

            var appPresidente = new PresidenteAplicacao();

            var listaDePresidente = appPresidente.ListarTodos(tipoCargo, relacionadoId);

            var pdf = new ViewAsPdf
                          {
                              ViewName = "Modelo",
                              Model = listaDePresidente
                          };
            return pdf;

        }


        /* Retorna a view simples em HTML, usada como modelo para gerar o PDF */

        public ActionResult Modelo()
        {
            return View();
        }


        /*
        * Retorna um PDF diretamente no browser com as configurações padrões
        * ViewName é setado somente para utilizar o proprio Modelo anterior
        * Caso não queira setar o ViewName, você deve gerar a view com o mesmo nome da action
        */


        /*
        * Configura algumas propriedades do PDF, inclusive o nome do arquivo gerado,
        * Porem agora ele baixa o pdf ao inves de mostrar no browser
        */
        [Seguranca(Permissao = "nacional")]
        public ActionResult PDFConfigurado()
        {
            var tipoCargo = Session["permissao"].ToString().ToLower();
            var relacionadoId = Session["relacionadoId"].ToString();
            var appPresidente = new PresidenteAplicacao();

            var listaDePresidente = appPresidente.ListarTodos(tipoCargo, relacionadoId);
            var pdf = new ViewAsPdf
            {
                ViewName = "Modelo",
                Model = listaDePresidente,
                FileName = "NomeDoArquivoPDF.pdf",
                PageSize = Size.A4,
                IsGrayScale = true,
                PageMargins = new Margins { Bottom = 5, Left = 5, Right = 5, Top = 5 },
            }
            ;
            return pdf;
        }

        [Seguranca(Permissao = "nacional")]
        public ActionResult MembrosRegiao()
        {


            return View();
        }
        
        [Seguranca(Permissao = "nacional")]
        public ActionResult MembrosPorRegiao()
        {
            var appRelatorio = new RelatorioAplicacao();
            var listaDeMmembros = appRelatorio.ListarMembrosPorRegionais();

            var pdf = new ViewAsPdf
                           {
                               ViewName = "MembrosRegiao",
                               Model = listaDeMmembros,
                           };
            return pdf;

        }

        [Seguranca(Permissao = "regional")]
        public ActionResult MembrosSinodal()
        {


            return View();
        }

        [Seguranca(Permissao = "regional")]
        public ActionResult MembrosPorSinodal()
        {
            var relacionadoId = Session["relacionadoId"].ToString();

            var appRelatorio = new RelatorioAplicacao();

            var listaDeMembros = appRelatorio.ListarMembrosPorSinodais(relacionadoId);

            var pdf = new ViewAsPdf
            {
                ViewName = "MembrosSinodal",
                Model = listaDeMembros,
            };
            return pdf;

        }


        [Seguranca(Permissao = "sinodal")]
        public ActionResult MembrosFederacao()
        {


            return View();
        }

        [Seguranca(Permissao = "sinodal")]
        public ActionResult MembrosPorFederacao()
        {
            var relacionadoId = Session["relacionadoId"].ToString();

            var appRelatorio = new RelatorioAplicacao();

            var listaDeMembros = appRelatorio.ListarMembrosPorFederacao(relacionadoId);

            var pdf = new ViewAsPdf
            {
                ViewName = "MembrosFederacao",
                Model = listaDeMembros,
            };
            return pdf;

        }








        [Seguranca(Permissao = "local")]
        public ActionResult MembrosIgreja()
        {


            return View();
        }

        [Seguranca(Permissao = "local")]
        public ActionResult MembrosPorIgreja()
        {
            var relacionadoId = Session["relacionadoId"].ToString();

            var appRelatorio = new MembroAplicacao();
            var listaDeMembros = appRelatorio.ListarMembros(relacionadoId);
           
            var pdf = new ViewAsPdf
            {
                ViewName = "MembrosIgreja",
                Model = listaDeMembros,
            };
            return pdf;

        }

    }

}







