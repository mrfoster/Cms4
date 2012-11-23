using System.Web.Mvc;

namespace Cms4.TestPlugin.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}