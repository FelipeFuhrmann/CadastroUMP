using System.Web.Mvc;
using CadastroUMP.Aplicacao;
using CadastroUMP.Dominio;
using CadastroUMP.UI.Web.Helpers;

namespace CadastroUMP.UI.Web.Controllers
{
    [Seguranca(Permissao = "sinodal")]
    public class FederacaoController : Controller
    {
        public ActionResult Index()
        {
            var sinodalPai = int.Parse(Session["relacionadoId"].ToString());
            return View(new FederacaoAplicacao().ListarFederacoesPorSinodal(sinodalPai));
        }

        public ActionResult Cadastrar()
        {
            return View();
        }

        //ValidateAntiForgeryToken - gera um codigo no formulario e impede
        //que outras pessoas postam informaçoes indesejadas no formulario!

        [HttpPost]
        public ActionResult Cadastrar(Federacao federacao)
        {
            if (ModelState.IsValid)
            {
                var appFederacao = new FederacaoAplicacao();
                federacao.Sinodal = new Sinodal
                                        {
                                            SinodalId = int.Parse(Session["relacionadoId"].ToString())
                                        };
                appFederacao.SalvarFederacao(federacao);
                return RedirectToAction("Index");
            }
            return View(federacao);
        }


        public ActionResult Editar(int id)
        {
            var appFederacao = new FederacaoAplicacao();
            var federacao = appFederacao.ListarPorId(id);

            if (federacao == null)
                return HttpNotFound();

            return View(federacao);
        }

        // o metodo SalvarCargo altera como salva, se vem com id ele altera, se não ele insere.
        [HttpPost]
        public ActionResult Editar(Federacao federacao)
        {
            if (ModelState.IsValid)
            {
                var appFederacao = new FederacaoAplicacao();
                //federacao.Sinodal = new Sinodal()
                //                        {
                //                            SinodalId = int.Parse(Session["relacionadoId"].ToString())
                //                        };
                appFederacao.SalvarFederacao(federacao);
                return RedirectToAction("Index");
            }
            return View(federacao);
        }

        public ActionResult Detalhes(int id)
        {
            var appFederacao = new FederacaoAplicacao();
            var federacao = appFederacao.ListarPorId(id);

            if (federacao == null)
                return HttpNotFound();

            return View(federacao);
        }

        public ActionResult ListarPorID(int id)
        {
            var aplicacao = new FederacaoAplicacao();
            var federacoes = aplicacao.ListarPorSinodalId(id);

            return Json(federacoes, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Excluir(int id)
        {
            var appFederacao = new FederacaoAplicacao();
            var federacao = appFederacao.ListarPorId(id);

            if (federacao == null)
                return HttpNotFound();

            return View(federacao);

        }

        [HttpPost, ActionName("Excluir")]
        public ActionResult ExcluirConfirmado(int id)
        {
            var appFederacao = new FederacaoAplicacao();
            appFederacao.ExcluirFederacao(id);
            return RedirectToAction("Index");
        }
    }

}

