using System.Web.Mvc;
using System.Web.Security;
using CadastroUMP.Aplicacao;
using CadastroUMP.UI.Web.Helpers;

namespace CadastroUMP.UI.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.erro = "";
            return View();
        }

        [HttpPost]
        public ActionResult Index(string usuario, string senha)
        {
            var app = new PresidenteAplicacao();
            var presidente = app.Logar(usuario, senha);
            if (presidente != null)
            {
                FormsAuthentication.SetAuthCookie(presidente.NomePresidente, false);
                Session["permissao"] = presidente.Cargo.TipoCargo;
                Session["relacionadoId"] = presidente.RelacionadoId;
                Session["presidenteId"] = presidente.PresidenteId;

                //guarda o filtro que é o tipo ou nivel do presidente
                return RedirectToAction("Painel");

            }
            ViewBag.erro = "Usuario e senha invalidos";
            return View();
        }

        [Seguranca]
        public ActionResult Sair()
        {
            FormsAuthentication.SignOut();
            Session["permissao"] = "";
            return RedirectToAction("Index");
        }

        [Seguranca(Action = "Index", Controller = "Home")]
        public ActionResult Painel()
        {
            return View();
        }
    }
}
