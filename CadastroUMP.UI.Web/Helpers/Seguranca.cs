using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CadastroUMP.UI.Web.Helpers
{
    /// <summary>
    /// Verifica se o usuario esta logado e/ou tem permissão para acessar determinado controller ou action. 
    /// Usando para isso uma variavel de sessão, que deve ser criada na hora do login Session["permissao"]
    /// </summary>
    public class Seguranca : ActionFilterAttribute
    {
        /// <summary>
        ///    Seta a permissao que o usuario logado precisa ter para acessar (string)
        /// </summary>
        public string Permissao { get; set; }
        /// <summary>
        ///    Seta o controller para onde o usuario será redirecionado caso não tenha a permissão
        /// </summary>
        public string Controller { get; set; }
        /// <summary>
        /// Seta a action para onde o usuario será redirecionado caso não tenha a permissão
        /// </summary>
        public string Action { get; set; }


        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            string ctl = "Home", act = "Painel";

            if (!string.IsNullOrEmpty(Controller)) ctl = Controller;
            if (!string.IsNullOrEmpty(Action)) act = Action;

            var rota = new RouteValueDictionary { { "controller", ctl }, { "action", act } };

            if (!Autenticado(Permissao))
                filterContext.Result = new RedirectToRouteResult(rota);
           
        }

        /// <summary>
        /// Verifica se o usuario esta logado e/ou tem permissão passada como parametro
        /// </summary>
        /// <param name="permissao">Permissão de acesso</param>
        /// <returns>True se tiver a logado e/ou tiver a permissão, retorna False caso contrário</returns>
        public static bool Autenticado(string permissao="")
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
                return false;

            if (!string.IsNullOrEmpty(permissao))
            {
                if (HttpContext.Current.Session["permissao"] == null)
                    return false;
                if (HttpContext.Current.Session["permissao"].ToString().ToLower() != permissao.ToLower())
                    return false;
            }

            return true;
        }
    }
}