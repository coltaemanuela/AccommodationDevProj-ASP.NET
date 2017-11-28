using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Accommodation_App.Models;
using System.Web.Security;
using System.Data.Entity;
using System.IO;

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


        //[Authorize(Roles = "landlord")]
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

        
        //[Authorize(Roles = "landlord")]
        [Route("/Landlord/ViewProperty/{pid:int}")]
        [HttpGet]
        public ViewResult ViewProperty(int id, int pid)
        {
            Property property = Properties.Find(pid);
             return View(property);
        }



        //[Authorize(Roles = "landlord")]
        [Route("/Landlord/AddProperty")]
        [HttpGet]
        public ActionResult AddProperty()
        {
            var uid = this.Session["userId"];

            var list = new SelectList(new[]
                {
                    new { ID = "Studio", Name = "Studio" },
                    new { ID = "Flat", Name = "Flat" },
                    new { ID = "House", Name = "House" },
                },
                "ID", "Name", 1);

            ViewData["list"] = list;
            return View();
        }

        //[Authorize(Roles = "landlord")]
        [HttpPost]
        public ActionResult AddProperty(Property prop, HttpPostedFileBase file)
        {
            //if(ModelState.IsValid)
            //{
            try
            {
                if (file != null && file.ContentLength > 0)
                {
                    //get the name of the uploaded file to store it in the database
                    prop.Photo = file.FileName;
                    string _FileName = Path.GetFileName(file.FileName);
                    string _path = Path.Combine(Server.MapPath("~/UploadFiles"), _FileName);
                    file.SaveAs(_path);
                }
                else { prop.Photo = "house.jpg"; }
                var uid = this.Session["userId"];
                prop.OwnerId = Convert.ToInt32(uid);
                prop.isApproved = true;
                prop.UAOComment = "";

                Properties.Add(prop);
                SContext.SaveChanges();
            }
            catch (Exception e)
            {
                return Content(e.Message + ":" + e.StackTrace);
            }
            ModelState.Clear();
            return RedirectToAction("AddSuccess", prop);
            //}
            //else
            //{
            // re-display the form 
            //  return View();
            //}
        }

        //[Route("Landlord")]
        public ActionResult AddSuccess(Property prop)
        {
            return View(prop);
        }
    }

}