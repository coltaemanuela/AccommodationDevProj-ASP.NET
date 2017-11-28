using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Accommodation_App.Models;
using System.Web.Security;
using System.Data.Entity;

namespace Accommodation_App.Controllers
{
    public class LoginController : Controller
    {

        private StudAccommodationEntities1 SContext;
        private DbSet<User> UserDb;

        public LoginController()
        {
            SContext = new StudAccommodationEntities1();
            UserDb = SContext.UserDb;
        }

        [Route("Login/Register")]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        //[AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Register(User user)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    UserDb.Add(user);
                    SContext.SaveChanges();
                    ModelState.Clear();
                    ViewBag.SuccessMessage = "Registration is successfully completed";
                    return RedirectToAction("RegisterSuccess");
                }
                catch (Exception x)
                {
                    return Content(x.Message + ":" + x.StackTrace);
                }
            }
            else
            {
            // re-display the registration form again
                return View();
            }
               
        }

        [Route("Login/Login")]
        public ActionResult RegisterSuccess()
        {
            return RedirectToAction("Login", "Login");

        }


        [Route("Login/Login")]
        //[ValidateAntiForgeryToken]
        //It signifies Form Based authentication that this method can be accessed without authentication 
        //[AllowAnonymous] 
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }


        [ValidateAntiForgeryToken]
        //[AllowAnonymous]
        [HttpPost]
        public ActionResult Login(User user)
        {
            try
            { 
            var obj = UserDb.FirstOrDefault(a => a.Email == user.Email && a.Password == user.Password);
            if (obj != null)
            {
                Session["UserId"] = obj.UserId.ToString();
                Session["Email"] = obj.Email.ToString();
                Session["Role"] = obj.Role.ToString();
                Session["TelephoneNum"] = obj.TelephoneNum.ToString();
                Session["FirstName"] = obj.FirstName.ToString();
                //  return RedirectToAction("LoggedIn");
                if (Session["Role"].ToString() == "admin")
                {
                    //return View();
                    FormsAuthentication.SetAuthCookie(user.Email, true);
                    return RedirectToAction("Index", "AOProf", new { id = obj.UserId });
                }
                else
                {
                    return RedirectToAction("Index", "Landlord", new { id= obj.UserId});
                }

               
                //return Content("Login is OK "+"Hello"+ user.Email +" "+ user.Password + "with role:"+ obj.Role );
            }
            else {
                    // re-display the login 
                    ViewBag.SuccessMessage = "Incorrect Email or Password, please try again!";
                    return View();
                }
            }
            catch (Exception x)
            {
                return Content(x.Message + ":" + x.StackTrace);
            }
        }

        [HttpGet]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Login");
        }

    }
}
