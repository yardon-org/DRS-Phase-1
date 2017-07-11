using System.Web.Mvc;

namespace drs_backend_phase1.Controllers
{
    /// <summary>
    /// Home Controller
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
    }
}
