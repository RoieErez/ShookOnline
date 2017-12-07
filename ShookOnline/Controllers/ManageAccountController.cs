using ShookOnline.Models;
using System.Web.Mvc;

namespace ShookOnline.Controllers
{
    [SessionTimeout]
    public class ManageAccountController : Controller
    {
       

        public ActionResult Settings()
        {
            return View();
        }
    }
}