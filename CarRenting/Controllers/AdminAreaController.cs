using System.Web.Mvc;

namespace CarRenting.Controllers
{
    public class AdminAreaController : Controller
    {
      
        [Authorize(Roles = "Administrador do Site")]
        public ActionResult Index()
        {
            return View();
        }
    }
}