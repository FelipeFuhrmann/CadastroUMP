using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CadastroUMP.Aplicacao;
using CadastroUMP.Dominio;

namespace CadastroUMP.UI.Web.Controllers
{
    public class RegionalController : Controller
    {
        public ActionResult Index()
        {
            var appRegional = new RegionalAplicacao();
            var listaDeRegionais = appRegional.ListarRegionais();
            return View(listaDeRegionais);
        }

        public ActionResult Cadastrar()
        {
            return View();
        }

        //ValidateAntiForgeryToken - gera um codigo no formulario e impede
        //que outras pessoas postam informaçoes indesejadas no formulario!

        [HttpPost]
        public ActionResult Cadastrar(Regional regional)
        {
            if (ModelState.IsValid)
            {
                var appRegional = new RegionalAplicacao();
                appRegional.SalvarRegional(regional);
                return RedirectToAction("Index");
            }
            return View(regional);
        }


        public ActionResult Editar(int id)
        {
            var appRegional = new RegionalAplicacao();
            var regional = appRegional.ListarPorId(id);

            if (regional == null)
                return HttpNotFound();

            return View(regional);
        }

        // o metodo SalvarCargo altera como salva, se vem com id ele altera, se não ele insere.
        [HttpPost]
        public ActionResult Editar(Regional regional)
        {
            if (ModelState.IsValid)
            {
                var appRegional = new RegionalAplicacao();
                appRegional.SalvarRegional(regional);
                return RedirectToAction("Index");
            }
            return View(regional);
        }

        public ActionResult Detalhes(int id)
        {
            var appRegional = new RegionalAplicacao();
            var regional = appRegional.ListarPorId(id);

            if (regional == null)
                return HttpNotFound();

            return View(regional);
        }



        public ActionResult ListarRegionais()
        {
            var aplicacao = new RegionalAplicacao();
            var regionais = aplicacao.ListarRegionais();

            return Json(regionais, JsonRequestBehavior.AllowGet);
        }


    }
}
