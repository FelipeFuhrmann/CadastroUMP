using System.Web.Mvc;
using CadastroUMP.Aplicacao;
using CadastroUMP.Dominio;

namespace CadastroUMP.UI.Web.Controllers
{
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

