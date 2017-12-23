using Facebook;
using ShookOnline.DAL;
using ShookOnline.Encryption;
using ShookOnline.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
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

            EUser user = new EUser(fb.Get("me?fields=first_name,last_name,id,email"));
            Session["UserName"] = user.UserName;
            Session["Location"] = RegionInfo.CurrentRegion.DisplayName;
            if (ModelState.IsValid)
            {
               EUserDal ud = new EUserDal();
               List<EUser> userToCheck = (from x in ud.Users
                                           where x.Email == user.Email
                                           select x).ToList<EUser>();
               if(userToCheck.Count == 1)
                    return RedirectToAction("Index", "Home");
               //provider key hashing
               PWEncryption enc = new PWEncryption();
               user.ProviderKey = enc.createHash(user.ProviderKey);
               ud.Users.Add(user);
               ud.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Home") ;

        }

        [HttpPost]
        public ActionResult CheckLogin(UserLogin u)
        {
            IUser user = new IUser(u);
            IUserDal ud = new IUserDal();
            PWEncryption enc = new PWEncryption();
            List<IUser> userToCheck = (from x in ud.Users
                                      where x.UserName == user.UserName
                                      select x).ToList<IUser>();


            foreach (IUser iu in userToCheck)
            {
                if (enc.validatePassword(user.Password, iu.Password))
                {
                    Session["UserName"] = user.UserName;
                    Session["Location"] = RegionInfo.CurrentRegion.DisplayName;
                    return RedirectToAction("Index", "Home");
                }
            }
            TempData["LoginMessage"] = "Invalid User Name/Password";
            return RedirectToAction("TryRegister", "Account");
        }


        [HttpPost]
        public ActionResult Register(UserRegister ur)
        {
            IUser user = new IUser(ur);            
            if (ModelState.IsValid)
            {
                IUserDal ud = new IUserDal();
                //check if user allready exist
                List<IUser> userToCheck = (from x in ud.Users
                                           where x.Email == user.Email
                                           select x).ToList<IUser>();
                if (userToCheck.Count == 1)
                {
                    TempData["RegisterMessage"] = "Mail is allready exist";
                    TempData["Register"] = "Register";
                    return View("Register");
                }
                //password hashing
                PWEncryption enc = new PWEncryption();
                user.Password = enc.createHash(user.Password);
                ud.Users.Add(user);
                ud.SaveChangesAsync();
                Session["UserName"] = user.UserName;
                Session["Location"] = RegionInfo.CurrentRegion.DisplayName;
                return RedirectToAction("Index", "Home", user);
            }
            return View("Register");
        }



        public ActionResult TryRegister(UserRegister ur)
        {
            if (ur.UserName != null)
                TempData["Register"] = "Register";
            return View("Register");
        }

        public ActionResult UserProfile()
        {
            return View();
        }

        public ActionResult Landing()
        {
            TempData["landing"] = "landing";
            return View("Register");
        }

    }
}