using System.Web.Mvc;
using CadastroUMP.Aplicacao;
using CadastroUMP.Dominio;
using CadastroUMP.UI.Web.Helpers;

namespace CadastroUMP.UI.Web.Controllers
{
    [Seguranca(Permissao = "federação")]
    public class IgrejaController : Controller
    {
        public ActionResult Index()
        {
            var federacaoPai = int.Parse(Session["relacionadoId"].ToString());
            return View(new IgrejaAplicacao().ListarIgrejasPorFederacao(federacaoPai));
        }

        public ActionResult Cadastrar()
        {
            ViewBag.Estados = new EstadoAplicacao().ListarTodos();
            return View();
        }

        //ValidateAntiForgeryToken - gera um codigo no formulario e impede
        //que outras pessoas postam informaçoes indesejadas no formulario!

        [HttpPost]
        public ActionResult Cadastrar(Igreja igreja)
        {
            igreja.Federacao = new Federacao();
            if (ModelState.IsValid)
            {
                var appIgreja = new IgrejaAplicacao();
                igreja.Federacao = new Federacao()
                {
                    FederacaoId = int.Parse(Session["relacionadoId"].ToString())
                };
                appIgreja.SalvarIgreja(igreja);
                return RedirectToAction("Index");
            }
            return View(igreja);
        }


        public ActionResult Editar(int id)
        {
            var appIgreja = new IgrejaAplicacao();
            var igreja = appIgreja.ListarPorId(id);

            if (igreja == null)
                return HttpNotFound();

            ViewBag.Estados = new EstadoAplicacao().ListarTodos();

            return View(igreja);
        }

        // o metodo SalvarCargo altera como salva, se vem com id ele altera, se não ele insere.
        [HttpPost]
        public ActionResult Editar(Igreja igreja)
        {
            if (ModelState.IsValid)
            {
                var appIgreja = new IgrejaAplicacao();
                appIgreja.SalvarIgreja(igreja);
                return RedirectToAction("Index");
            }
            return View(igreja);
        }

        public ActionResult Detalhes(int id)
        {
            var appIgreja = new IgrejaAplicacao();
            var igreja = appIgreja.ListarPorId(id);

            if (igreja == null)
                return HttpNotFound();

            return View(igreja);
        }

        public ActionResult Excluir(int id)
        {
            var appIgreja = new IgrejaAplicacao();
            var igreja = appIgreja.ListarPorId(id);

            if (igreja == null)
                return HttpNotFound();

            return View(igreja);

        }

        [HttpPost, ActionName("Excluir")]
        public ActionResult ExcluirConfirmado(int id)
        {
            var appIgreja = new IgrejaAplicacao();
            appIgreja.ExcluirIgreja(id);
            return RedirectToAction("Index");
        }
    }
}
