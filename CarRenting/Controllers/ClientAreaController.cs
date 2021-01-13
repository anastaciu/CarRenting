using System.Web.Mvc;

namespace CarRenting.Controllers
{
    [Authorize(Roles = "Utilizador Registado")]
    public class ClientAreaController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

    }
}