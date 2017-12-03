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

        public ActionResult Landing()
        {
            return View();
        }

        public ActionResult LogOff()
        {
            Session.Clear();

            return View("Landing");
        }
    }
}