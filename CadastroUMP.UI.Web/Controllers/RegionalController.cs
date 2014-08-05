using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CadastroUMP.Aplicacao;
using CadastroUMP.Dominio;
using CadastroUMP.UI.Web.Helpers;

namespace CadastroUMP.UI.Web.Controllers
{
    [Seguranca]
    public class RegionalController : Controller
    {

        public ActionResult ListarRegionais()
        {
            var aplicacao = new RegionalAplicacao();
            var regionais = aplicacao.ListarRegionais();

            return Json(regionais, JsonRequestBehavior.AllowGet);
        }


    }
}
