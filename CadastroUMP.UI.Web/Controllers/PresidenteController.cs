using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.Mvc;
using CadastroUMP.Aplicacao;
using CadastroUMP.Dominio;
using CadastroUMP.UI.Web.Helpers;

namespace CadastroUMP.UI.Web.Controllers
{
    // [Seguranca]
    public class PresidenteController : Controller
    {

        private Collection<string> FilhoDoUsuarioLogado()
        {
            if (Session["permissao"] == null)
                return new Collection<string> { "", "" };


            var pai = Session["permissao"].ToString().ToLower();
            var relacionadoId = Session["relacionadoId"].ToString();

            switch (pai)
            {
                case "nacional":
                    return new Collection<string> { "regional", relacionadoId };
                case "regional":
                    return new Collection<string> { "sinodal", relacionadoId };
                case "sinodal":
                    return new Collection<string> { "federação", relacionadoId };
                case "federação":
                    return new Collection<string> { "local", relacionadoId };
                case "local":
                    return new Collection<string> { "membro", relacionadoId };
            }

            return new Collection<string> { "", "" };
        }

        public ActionResult Nacional()
        {
            var appPresidente = new PresidenteAplicacao();
            string tipoCargo = FilhoDoUsuarioLogado()[0];
            string filtro = FilhoDoUsuarioLogado()[1];

            var listaDePresidente = appPresidente.ListarPresidentes(tipoCargo, filtro);
        
            return View(listaDePresidente);

           }

        public ActionResult Cadastrar()
        {
            string nome;
            var retorno = CarregaDropDowns(out nome);

            ViewBag.NomeLista = nome;
            ViewBag.Lista = retorno;

            return View();
        }

        private Dictionary<string, string> CarregaDropDowns(out string nome)
        {
            var retorno = new Dictionary<string, string>();
            nome = "";

            var relacionadoId = int.Parse(Session["relacionadoId"].ToString());

            var pai = Session["permissao"].ToString().ToLower();
            switch (pai)
            {
                case "nacional":
                    retorno = new RegionalAplicacao().ListarRegionaisLivres().ToDictionary(x => x.RegionalId.ToString(), x => x.NomeRegional);
                    nome = "Regionais";
                    break;
                case "regional":
                    retorno = new SinodalAplicacao().ListarSinodaisPorRegiaoLivre(relacionadoId).ToDictionary(x => x.SinodalId.ToString(), x => x.NomeSinodal);
                    nome = "Sinodais";
                    break;
                case "sinodal":
                    retorno = new FederacaoAplicacao().ListarFederacoesPorSinodalLivre(relacionadoId).ToDictionary(x => x.FederacaoId.ToString(), x => x.NomeFederacao);
                    nome = "Federações";
                    break;
                case "federação":
                    retorno = new IgrejaAplicacao().ListarIgrejasPorFederacaoLivre(relacionadoId).ToDictionary(x => x.IgrejaId.ToString(), x => x.NomeIgreja);
                    nome = "Igrejas";
                    break;

            }
            return retorno;
        }


        [HttpPost]
        public ActionResult Cadastrar(Presidente presidente)
        {
            if (ModelState.IsValid)
            {
                var cargoAplicacao = new CargoAplicacao();
                var cargo = new Cargo();
                var pai = Session["permissao"].ToString().ToLower();
                switch (pai)
                {
                    case "nacional":
                        cargo.CargoId = cargoAplicacao.ListarCargos("Regional");
                        break;
                    case "regional":
                        cargo.CargoId = cargoAplicacao.ListarCargos("Sinodal");
                        break;
                    case "sinodal":
                        cargo.CargoId = cargoAplicacao.ListarCargos("Federação");
                        break;
                    case "federação":
                        cargo.CargoId = cargoAplicacao.ListarCargos("Local");
                        break;
                }

                presidente.Cargo = cargo;

                var appPresidente = new PresidenteAplicacao();
                appPresidente.SalvarPresidente(presidente);
                return RedirectToAction("Nacional");
            }

            string nome;
            var retorno = CarregaDropDowns(out nome);

            ViewBag.NomeLista = nome;
            ViewBag.Lista = retorno;

            return View(presidente);
        }

        //criar uma ation pra responder pelo ajax
        // vai ter um parametro string que é o tipo de cargo, ai tu faz um swith e instancia a aplicacao correta, e busca na tabela os cargos


        public ActionResult Editar(int id)
        {
            var appPresidente = new PresidenteAplicacao();
            var presidente = appPresidente.ListarPorId(id);

            if (presidente == null)
                return HttpNotFound();

            string nome;
            var retorno = CarregaDropDowns(out nome);

            ViewBag.NomeLista = nome;
            ViewBag.Lista = retorno;


            //  var cargos = new CargoAplicacao().ListarCargos();
            // ViewBag.Cargos = new SelectList(cargos, "CargoId", "TipoCargo", presidente.Cargo.CargoId);

            return View(presidente);
        }

        // o metodo SalvarCargo altera como salva, se vem com id ele altera, se não ele insere.
        [HttpPost]
        public ActionResult Editar(Presidente presidente)
        {
            if (ModelState.IsValid)
            {
                var appPresidente = new PresidenteAplicacao();
                appPresidente.SalvarPresidente(presidente);
                return RedirectToAction("Nacional");
            }
            string nome;
            var retorno = CarregaDropDowns(out nome);

            ViewBag.NomeLista = nome;
            ViewBag.Lista = retorno;

            return View(presidente);
        }


        public ActionResult EditarPerfil()
        {
           
            var presidenteId = int.Parse(Session["presidenteId"].ToString());
            var appPresidente = new PresidenteAplicacao();
            var presidente = appPresidente.ListarPorId(presidenteId);

            if (presidente == null)
                return HttpNotFound();           
           

            return View(presidente);
        }

        // o metodo SalvarCargo altera como salva, se vem com id ele altera, se não ele insere.
        [HttpPost]
        public ActionResult EditarPerfil(Presidente presidente)
        {
            if (ModelState.IsValid)
            {
                var appPresidente = new PresidenteAplicacao();
                appPresidente.SalvarPresidente(presidente);
                return RedirectToAction("Nacional");
            }
           

            return View(presidente);
        }


        // comando pra retornar as regionais quando eu selecionar o tipo de cargo REGIONAL.
        public ActionResult ListarRegionalPorCargo(int id)
        {
            var aplicacao = new PresidenteAplicacao();
            var regionais = aplicacao.ListarRegionalPorCargoID(id);

            return Json(regionais, JsonRequestBehavior.AllowGet);
        }



        public ActionResult Detalhes(int id)
        {
            var appPresidente = new PresidenteAplicacao();
            var presidente = appPresidente.ListarPorId(id);

            if (presidente == null)
                return HttpNotFound();

            return View(presidente);
        }

        public ActionResult Excluir(int id)
        {
            var appPresidente = new PresidenteAplicacao();
            var presidente = appPresidente.ListarPorId(id);

            if (presidente == null)
                return HttpNotFound();

            return View(presidente);

        }

        [HttpPost, ActionName("Excluir")]
        public ActionResult ExcluirConfirmado(int id)
        {
            var appPresidente = new PresidenteAplicacao();
            appPresidente.ExcluirPresidente(id);
            return RedirectToAction("Nacional");
        }

    }
}
