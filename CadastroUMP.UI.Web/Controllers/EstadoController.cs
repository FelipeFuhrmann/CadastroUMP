using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CadastroUMP.Aplicacao;
using CadastroUMP.Dominio;

namespace CadastroUMP.UI.Web.Controllers
{
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


