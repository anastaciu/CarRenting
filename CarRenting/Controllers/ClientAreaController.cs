using System.Web.Mvc;

namespace CarRenting.Controllers
{
    public class ClientAreaController : Controller
    {
        [Authorize(Roles = "Utilizador Registado")]
        public ActionResult Index()
        {
            return View();
        }

    }
}