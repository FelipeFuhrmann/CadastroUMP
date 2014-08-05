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
    public class EstadoController : Controller
    {

        public ActionResult Index()
        {
            var aplicacao = new EstadoAplicacao();
            var estados = aplicacao.ListarTodos();

            return Json(estados, JsonRequestBehavior.AllowGet);
        }

    }


}


