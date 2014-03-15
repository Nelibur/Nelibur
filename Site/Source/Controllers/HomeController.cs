using System.Web.Mvc;

namespace Site.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Rendering index page.
        /// </summary>
        /// <returns>Result.</returns>
        public ActionResult Index()
        {
            return View();
        }
    }
}