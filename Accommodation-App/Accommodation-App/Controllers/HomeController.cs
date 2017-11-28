using Accommodation_App.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace Accommodation_App.Controllers
{
    public class HomeController : Controller
    {

        private StudAccommodationEntities1 SContextUser;
        private DbSet<User> UserDb;

        private StudAccommodationEntities1Entities SContextProp;
        private DbSet<Property> Properties;

        public HomeController()
        {
            SContextUser = new StudAccommodationEntities1();
            UserDb = SContextUser.UserDb;

            SContextProp = new StudAccommodationEntities1Entities();
            Properties = SContextProp.Properties;
        }

        [Route("Home/Index")]
        // it signifies Form Based authentication that this method can be accessed without authentication
        [AllowAnonymous] 
        public ActionResult Index()
        {
            var mvcName = typeof(Controller).Assembly.GetName();
            var isMono = Type.GetType("Mono.Runtime") != null;

            ViewData["Version"] = mvcName.Version.Major + "." + mvcName.Version.Minor;
            ViewData["Runtime"] = isMono ? "Mono" : ".NET";

               // ViewData["UserName"] = AO.UserName.ToString();

            return View();
        }

        [Route("Home/About")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [Route("Home/Contact")]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}