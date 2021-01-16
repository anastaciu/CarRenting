using System.Web.Mvc;

namespace CarRenting.Controllers
{
    public class CompanyUserAreaController : Controller
    {
       
        [Authorize(Roles = "Utilizador da Empresa")]
        public ActionResult Index()
        {
            return View();
        }
    }
}