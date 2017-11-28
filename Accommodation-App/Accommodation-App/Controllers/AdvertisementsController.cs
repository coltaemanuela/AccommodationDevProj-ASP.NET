using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Accommodation_App.Models;
using System.Web.Security;
using System.Data.Entity;


namespace Accommodation_App.Controllers
{
    public class AdvertisementsController : Controller
    {
        private StudAccommodationEntities1Entities SContext;
        private DbSet<Property> Properties;
        private Property[] Property = { new Models.Property() };

        public AdvertisementsController()
        {
            SContext = new StudAccommodationEntities1Entities();
            Properties = SContext.Properties;
        }

        // GET:All Properties - list visible to students.
        //Only the approved ones will be displayed
        //[AllowAnonymous]
        [Route("/Advertisements/Index")]
        [HttpGet]
        public ActionResult Index()
        {
            var controls = from c in SContext.Properties
                           where c.isApproved == true
                           select c;

            List<Property> properties = new List<Property>();
            foreach (var fc in controls)
            {
                //Create Object for Generic List
                Property epc = new Property();
                epc.ProId = fc.ProId;
                epc.Name = fc.Name;
                epc.Description = fc.Description;
                epc.RentPrice = fc.RentPrice;
                epc.RoomNumber = fc.RoomNumber;
                epc.Type = fc.Type;
                epc.Address = fc.Address;

                //Add Object to FormControls Generic List
                properties.Add(epc);
            }
            return View(properties);
        }


        //One advertisement details visible to students. 
        //[AllowAnonymous]
        [Route("/Advertisements/{id:int}")]
        [HttpGet]
        public ViewResult ViewAdvertisementDetails(int id)
        {
            Property property = Properties.Find(id);
            return View(property);
        }

    }

}