using ShookOnline.Models;
using System.Web.Mvc;

namespace ShookOnline.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(User u)
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Landing()
        {
            return View();
        }
    }
}