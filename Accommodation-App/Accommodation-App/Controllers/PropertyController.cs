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
    public class PropertyController : Controller
    {
        private StudAccommodationEntities1Entities SContext;
        private DbSet<Property> Properties;
        private Property[] Property = { new Models.Property() };

        public PropertyController()
        {
            SContext = new StudAccommodationEntities1Entities();
            Properties = SContext.Properties;
        }

        // GET:All  Properties - list visible to students.
        //Only the aproved ones will be displayed
        [Route("/Property/Index")]
        [HttpGet]
        public ActionResult Index()
        {
            var controls = from c in SContext.Properties
                           where c.isApproved==true
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


        //One property details visible to students.     
        [Route("/Property/{id:int}")]
        [HttpGet]
        public ViewResult ViewPropertyDetails(int id)
        {
            Property property = Properties.Find(id);
            return View(property);
        }

        [Route("/Property/AddProperty")]
        [HttpGet]
        //[Authorize]
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

        [HttpPost]
        //[Authorize]
        public ActionResult AddProperty(Property tbl, HttpPostedFileBase file)
        {
            using (StudAccommodationEntities1Entities db = new StudAccommodationEntities1Entities())
            {
                try
                {   
                    if (file != null && file.ContentLength > 0)
                    { 
                        //get the name of the uploaded file to store it in the database
                        tbl.Photo = file.FileName;
                        string _FileName = Path.GetFileName(file.FileName);
                        string _path = Path.Combine(Server.MapPath("~/UploadFiles"), _FileName);
                        file.SaveAs(_path);
                    }
                    else { tbl.Photo = "house.jpg"; }
                    var uid = this.Session["userId"];
                    tbl.OwnerId = Convert.ToInt32(uid);
                    tbl.isApproved = true;
                    tbl.UAOComment = "";

                    db.Properties.Add(tbl);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    return Content(e.Message + ":" + e.StackTrace);
                }
                ModelState.Clear();
                ViewBag.SuccessMessage = "Adding accommodation is successfully completed";
                return RedirectToAction("AddSuccess", tbl);
            }
        }
    
        [Route("Landlord")]
        //[Authorize]
        public ActionResult AddSuccesss(Property tbl)
        {
             return View(tbl);
        }
    }

}