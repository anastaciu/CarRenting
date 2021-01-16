using System.Web.Mvc;

namespace CarRenting.Controllers
{
    public class CompanyAdminAreaController : Controller
    {

        [Authorize(Roles = "Administrador da Empresa")]
        public ActionResult Index()
        {
            return View();
        }

    }

}