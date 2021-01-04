using System.Web.Mvc;

/// <summary>
/// This controller exists to provide the error page
/// </summary>
public class ErrorController : Controller
{
    public ActionResult Index(string message)
    {
        // We choose to use the ViewBag to communicate the error message to the view
        ViewBag.Message = message;
        return View();
    }
}