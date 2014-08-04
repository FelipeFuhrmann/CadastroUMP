using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CadastroUMP.Aplicacao;
using Rotativa;
using Rotativa.Options;

namespace CadastroUMP.UI.Web.Controllers
{
    public class RelatorioController : Controller
    {


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


        public ActionResult MembrosNacional()
        {

            return View();
        }

        public ActionResult MembrosPorRegiao()
        {
            var appRelatorio = new RelatorioAplicacao();
            var listaDeMmembros = appRelatorio.ListarMembrosPorRegionais();

            var pdf = new ViewAsPdf
                       {
                           ViewName = "MembrosNacional",
                           Model = listaDeMmembros
                       };
            return pdf;

        }



    }

}







