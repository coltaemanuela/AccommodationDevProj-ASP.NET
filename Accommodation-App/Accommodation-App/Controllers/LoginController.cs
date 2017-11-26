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
        private User[] Users;

        public LoginController()
        {
            SContext = new StudAccommodationEntities1();
            UserDb = SContext.UserDb;
        }

        [Route("Login/Register")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [Route("Login/Register")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Register(User tbl)
        {

            using (StudAccommodationEntities1 db = new StudAccommodationEntities1())
            {
                try
                {
                    db.UserDb.Add(tbl);
                    db.SaveChanges();
                }
                catch (Exception x)
                {
                    return Content(x.Message + ":" + x.StackTrace);
                }
            }
                ModelState.Clear();
                ViewBag.SuccessMessage = "Registration is successfully completed";
                return RedirectToAction("RegisterSuccess", tbl);

        }

        [Route("Home/Index")]
        //[Authorize]
        public ActionResult RegisterSuccess(User tbl)
        {
            // return View(tbl);
            return RedirectToAction("Index", "Home");

        }


        [Route("Login/Login")]
        //[ValidateAntiForgeryToken]
        //-- It signifies Form Based authentication that this method can be accessed without authentication --
        [AllowAnonymous] 
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }


        //[Route("Login/Login")]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
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
            else
                ViewBag.SuccessMessage = "Incorrect Email or Password, please try again!";
                return View(); 
            }
            catch (Exception x)
            {
                return Content(x.Message + ":" + x.StackTrace);
            }
        }

        [HttpGet]
        //[Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

    }
}
