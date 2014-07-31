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
    [Seguranca(Permissao = "nacional")]
    public class CargoController : Controller
    {
        
        public ActionResult Index()
        {
            var appCargo = new CargoAplicacao();
            var listaDeCargos = appCargo.ListarCargos();
            return View(listaDeCargos);
        }

        public ActionResult Cadastrar()
        {
            return View();
        }

        //ValidateAntiForgeryToken - gera um codigo no formulario e impede
        //que outras pessoas postam informaçoes indesejadas no formulario!

        [HttpPost]
        public ActionResult Cadastrar(Cargo cargo)
        {
            if (ModelState.IsValid)
            {
                var appCargo = new CargoAplicacao();
                appCargo.SalvarCargo(cargo);
                return RedirectToAction("Index");
            }
            return View(cargo);
        }


        public ActionResult Editar(int id)
        {
            var appCargo = new CargoAplicacao();
            var cargo = appCargo.ListarPorId(id);

            if (cargo == null)
                return HttpNotFound();

            return View(cargo);
        }

        // o metodo SalvarCargo altera como salva, se vem com id ele altera, se não ele insere.
        [HttpPost]
        public ActionResult Editar(Cargo cargo)
        {
            if (ModelState.IsValid)
            {
                var appCargo = new CargoAplicacao();
                appCargo.SalvarCargo(cargo);
                return RedirectToAction("Index");
            }
            return View(cargo);
        }

        public ActionResult Detalhes(int id)
        {
            var appCargo = new CargoAplicacao();
            var cargo = appCargo.ListarPorId(id);

            if (cargo == null)
                return HttpNotFound();

            return View(cargo);
        }

        public ActionResult Excluir (int id)
        {
            var appCargo = new CargoAplicacao();
            var cargo = appCargo.ListarPorId(id);

            if (cargo == null)
                return HttpNotFound();

            return View(cargo);
            
        }

        [HttpPost, ActionName("Excluir")]
        public ActionResult ExcluirConfirmado(int id)
        {
            var appCargo = new CargoAplicacao();
            appCargo.ExcluirCargo(id);
            return RedirectToAction("Index");
        }
    }

}
