using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CadastroUMP.Aplicacao;
using CadastroUMP.Dominio;
using CadastroUMP.UI.Web.Helpers;

namespace CadastroUMP.UI.Web.Controllers
    
{
    [Seguranca(Permissao = "local")]
    public class MembroController : Controller
    {
        private Collection<string> FilhoDoUsuarioLogado()
        {
            if (Session["permissao"] == null)
                return new Collection<string> { "", "" };

            var pai = Session["permissao"].ToString().ToLower();
            var relacionadoId = Session["relacionadoId"].ToString();

            switch (pai)
            {
                case "local":
                    return new Collection<string> { "membro", relacionadoId };
            }

            return new Collection<string> { "" };
        }

        public ActionResult Index()
        {
            var appMembro = new MembroAplicacao();
            string filtro = FilhoDoUsuarioLogado()[1];

            var listaDeMembro = appMembro.ListarMembros(filtro);

            return View(listaDeMembro);
        }

        public ActionResult Cadastrar()
        {
            ViewBag.Estados = new EstadoAplicacao().ListarTodos();

            //var cidades = new CidadeAplicacao().ListarCidades();
            //ViewBag.Cidades = new SelectList(cidades, "CidadeId", "NomeCidade");
            return View();
        }

        //ValidateAntiForgeryToken - gera um codigo no formulario e impede
        //que outras pessoas postam informaçoes indesejadas no formulario!

        [HttpPost]
        public ActionResult Cadastrar(Membro membro)
        {
            if (ModelState.IsValid)
            {
                var appMembro = new MembroAplicacao();
                membro.Igreja = new Igreja()
                                    {
                                        IgrejaId = int.Parse(Session["relacionadoId"].ToString())
                                    };
                appMembro.SalvarMembro(membro);
                return RedirectToAction("Index");
            }
            return View(membro);
        }


        public ActionResult Editar(int id)
        {
            var appMembro = new MembroAplicacao();
            var membro = appMembro.ListarPorId(id);

            if (membro == null)
                return HttpNotFound();

            ViewBag.Estados = new EstadoAplicacao().ListarTodos();

            //var cidades = new CidadeAplicacao().ListarCidades();
            //ViewBag.Cidades = new SelectList(cidades, "CidadeId", "NomeCidade", membro.Cidade.CidadeId);

            return View(membro);
        }

        // o metodo SalvarCargo altera como salva, se vem com id ele altera, se não ele insere.
        [HttpPost]
        public ActionResult Editar(Membro membro)
        {
            if (ModelState.IsValid)
            {
                var appMembro = new MembroAplicacao();
                appMembro.SalvarMembro(membro);
                return RedirectToAction("Index");
            }
            return View(membro);
        }

        public ActionResult Detalhes(int id)
        {
            var appMembro = new MembroAplicacao();
            var membro = appMembro.ListarPorId(id);

            if (membro == null)
                return HttpNotFound();

            return View(membro);
        }

        public ActionResult Buscar(string nome)
        {
            var appMembro = new MembroAplicacao();
            var membro = appMembro.ListarPorNome(nome);

            if (membro == null)
                return HttpNotFound();
            
            return View(membro);
        }

       
        public ActionResult Excluir(int id)
        {
            var appMembro = new MembroAplicacao();
            var membro = appMembro.ListarPorId(id);

            if (membro == null)
                return HttpNotFound();

            return View(membro);

        }

        [HttpPost, ActionName("Excluir")]
        public ActionResult ExcluirConfirmado(int id)
        {
            var appMembro = new MembroAplicacao();
            appMembro.ExcluirMembro(id);
            return RedirectToAction("Index");
        }
    }

}
