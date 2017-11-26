using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Accommodation_App.Controllers
{
    public class TermsPrivacyController : Controller
    {
        [Route("/TermsPrivacy")]
        [HttpGet]
        public ViewResult Index()
        {
            return View();
        }

        [Route("/TermsPrivacy/Privacy")]
        [HttpGet]
        public ViewResult Privacy()
        {
            return View();
        }
    }
}