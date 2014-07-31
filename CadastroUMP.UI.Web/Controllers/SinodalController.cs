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
    [Seguranca(Permissao = "regional")]
    public class SinodalController : Controller
    {
        public ActionResult Index()
        {
            var regionalPai = int.Parse(Session["relacionadoId"].ToString());
            return View(new SinodalAplicacao().ListarSinodaisPorRegiao(regionalPai));
        }

        public ActionResult Cadastrar()
        {
            
            return View();
        }

        //ValidateAntiForgeryToken - gera um codigo no formulario e impede
        //que outras pessoas postam informaçoes indesejadas no formulario!

        // sinodal.Regional metodo que pegar o ReferencialId do usuário pai e passa para o filho. 
        [HttpPost]
        public ActionResult Cadastrar(Sinodal sinodal)
        {
            if (ModelState.IsValid)
            {
                var appSinodal = new SinodalAplicacao();
                sinodal.Regional = new Regional()
                                       {
                                           RegionalId = int.Parse(Session["relacionadoId"].ToString())
                                       };
                appSinodal.SalvarSinodal(sinodal);
                return RedirectToAction("Index");
            }
            return View(sinodal);
        }


        public ActionResult Editar(int id)
        {
            var appSinodal = new SinodalAplicacao();
            var sinodal = appSinodal.ListarPorId(id);

            if (sinodal == null)
                return HttpNotFound();

            var regionais = new RegionalAplicacao().ListarRegionais();
            ViewBag.Regionais = new SelectList(regionais, "RegionalId", "NomeRegional", sinodal.Regional.RegionalId);

            return View(sinodal);
        }

        // o metodo SalvarCargo altera como salva, se vem com id ele altera, se não ele insere.
        [HttpPost]
        public ActionResult Editar(Sinodal sinodal)
        {
            if (ModelState.IsValid)
            {
                var appSinodal = new SinodalAplicacao();
                appSinodal.SalvarSinodal(sinodal);
                return RedirectToAction("Index");
            }
            return View(sinodal);
        }

        public ActionResult Detalhes(int id)
        {
            var appSinodal = new SinodalAplicacao();
            var sinodal = appSinodal.ListarPorId(id);

            if (sinodal == null)
                return HttpNotFound();

            return View(sinodal);
        }


        public ActionResult ListarPorID (int id)

        {
            var aplicacao = new SinodalAplicacao();
            var sinodais = aplicacao.ListarPorRegionalId(id);

            return Json(sinodais, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Excluir(int id)
        {
            var appSinodal = new SinodalAplicacao();
            var sinodal = appSinodal.ListarPorId(id);

            if (sinodal == null)
                return HttpNotFound();

            return View(sinodal);

        }

        [HttpPost, ActionName("Excluir")]
        public ActionResult ExcluirConfirmado(int id)
        {
            var appSinodal = new SinodalAplicacao();
            appSinodal.ExcluirSinodal(id);
            return RedirectToAction("Index");
        }
    }
}
