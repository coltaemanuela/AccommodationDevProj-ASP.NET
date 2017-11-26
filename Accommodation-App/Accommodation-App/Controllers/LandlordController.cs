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
    public class LandlordController : Controller
    {
 
        private StudAccommodationEntities1Entities SContext;
        private DbSet<Property> Properties;
        private StudAccommodationEntities1 SContext1;

        private DbSet<User> UserDb;
       // private User[] Users;

        public LandlordController()
        {
            SContext = new StudAccommodationEntities1Entities();
            Properties = SContext.Properties;

            SContext1 = new StudAccommodationEntities1();
            UserDb = SContext1.UserDb;

        }

        private Property[] Property = { new Models.Property() };


        [Route("/Landlord/Index/{id:int}")]
        [HttpGet]
        public ViewResult Index(int id)
        {
            User landlord = UserDb.Find(id);
            ViewData["LastName"] = landlord.LastName.ToString();
            ViewData["FirstName"] = landlord.FirstName.ToString();
            ViewData["Email"] = landlord.Email.ToString();
            ViewData["Role"] = landlord.Role.ToString();
            ViewData["TelephoneNum"] = landlord.TelephoneNum;

            var controls = from c in SContext.Properties
                           where c.OwnerId==id
                           select c;

            List<Property> properties = new List<Property>();

            foreach (var fc in controls)
            {
                    //Create Object for Generic List
                    Property epc = new Property();
                    epc.ProId = fc.ProId;
                    epc.Name = fc.Name;
                    epc.Type = fc.Type;
                    epc.isApproved = fc.isApproved;
                    epc.AvailableDate = fc.AvailableDate;

                    //Add Object to FormControls Generic List
                    properties.Add(epc);
            }
            return View(properties);
        }


        
        [Route("/Landlord/LandlordPropertiesPage/{id:int}")]
        [HttpGet]
        public ViewResult LandlordPropertiesPage(int id)
        {

            var controls = from c in SContext.Properties
                           where c.OwnerId == id
                           select c;

            List<Property> properties = new List<Property>();

            foreach (var fc in controls)
            {
                    //Create Object for Generic List
                    Property epc = new Property();
                    epc.ProId = fc.ProId;
                    epc.Name = fc.Name;
                    epc.Type = fc.Type;
                    epc.OwnerId = fc.OwnerId;
                    //Add Object to FormControls Generic List
                    properties.Add(epc);
                
            }
            return View(properties);
        }

        [Route("/Property/{id:int}/details/{pid:id}")]
        [HttpGet]
        public ViewResult ViewProperty(int id, int pid)
        {
            Property property = Properties.Find(pid);
             return View(property);
        }

        public ActionResult PropertyDetails(int id)
        {
            return RedirectToAction("PropertyOfLandlord", "AOProf", new { id = id });
            //return RedirectToAction("Index", "Property");
        }

    }
}