using System.Web.Mvc;
using CadastroUMP.Aplicacao;
using CadastroUMP.Dominio;
using CadastroUMP.UI.Web.Helpers;

namespace CadastroUMP.UI.Web.Controllers
{
    [Seguranca]
    public class CidadeController : Controller
    {

        public ActionResult Index(int idEstado)
        {
            var aplicacao = new CidadeAplicacao();
            var cidades = aplicacao.ListarPorEstadoId(idEstado);

            return Json(cidades, JsonRequestBehavior.AllowGet);
        }
    }

}

