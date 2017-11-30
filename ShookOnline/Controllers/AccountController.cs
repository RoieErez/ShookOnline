using Facebook;
using ShookOnline.Models;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MVCApplication1.Controllers
{
    public class AccountController : Controller
    {
        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }

        public ActionResult Facebook()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = System.Configuration.ConfigurationManager.AppSettings["FacebookAppId"],
                client_secret = System.Configuration.ConfigurationManager.AppSettings["FacebookAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope = "email" // Add other permissions as needed
            });

            return Redirect(loginUrl.AbsoluteUri);
        }
        public ActionResult FacebookCallback(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = System.Configuration.ConfigurationManager.AppSettings["FacebookAppId"],
                client_secret = System.Configuration.ConfigurationManager.AppSettings["FacebookAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                code = code
            });
            var accessToken = result.access_token;

            // Store the access token in the session
            Session["AccessToken"] = accessToken;

            // update the facebook client with the access token so
            // we can make requests on behalf of the user
            fb.AccessToken = accessToken;

            // Get the user's information

            User user = new User(fb.Get("me?fields=first_name,last_name,id,email"));

            return user.checkLogin(true) ? RedirectToAction("Index", "Home") : RedirectToAction("TryRegister", "Account");

        }



        [HttpPost]
        public ActionResult CheckLogin(UserLogin u)
        {
            User user = new User(u);

            return  user.checkLogin(false) ? RedirectToAction("Index", "Home") : RedirectToAction("TryRegister", "Account");
        }


        [HttpPost]
        public ActionResult Register(UserRegister ur)
        {
            User user = new User(ur);
            user.userRegister();
            return View("Home",user);
        }


        
        public ActionResult TryRegister()
        {

            return View("Register");
        }

       
    }
}