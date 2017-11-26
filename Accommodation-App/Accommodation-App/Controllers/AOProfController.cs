using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Accommodation_App.Models;
using System.Data.Entity;

namespace Accommodation_App.Controllers
{
    public class AOProfController : Controller
    {

        private StudAccommodationEntities1 SContextUser;
        private DbSet<User> UserDb;

        private StudAccommodationEntities1Entities SContextProp;
        private DbSet<Property> Properties;

        public AOProfController()
        {
            SContextUser = new StudAccommodationEntities1();
            UserDb = SContextUser.UserDb;

            SContextProp = new StudAccommodationEntities1Entities();
            Properties = SContextProp.Properties;
        }

        private User[] Users = { new Models.User() };


        [Route("/AOProf/Index/{id:int}")]
        // in the AO profile, list of all landlords 
        // GET: AOProf
        public ActionResult Index(int id)
        {
            var controls = from c in SContextUser.User
                           where c.Role == "landlord"
                           select c;

            List<User> users = new List<User>();

            User AO = UserDb.Find(id);
            ViewData["UserName"] = AO.UserName.ToString();

            foreach (var fc in controls)
            {              
                    //Create Object for Generic List
                    User epc = new User();
                    epc.UserId = fc.UserId;
                    epc.FirstName = fc.FirstName;
                    epc.LastName = fc.LastName;

                    //Add Object to FormControls Generic List
                    users.Add(epc);                
            }
            return View(users);
        }


        public ActionResult OpenLandlordProperties(int id)
        {
            return RedirectToAction("LandlordPropertiesPage", "Landlord", new { id = id });
        }


        [Route("/AOProf/PropertyOfLandlord/{id:int}")]
        [HttpGet]
        public ViewResult PropertyOfLandlord(int id)
        {
            var controls = from c in SContextProp.Properties
                           where c.ProId == id
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
                    epc.OwnerId = fc.OwnerId;
                    epc.Photo = fc.Photo;

                    //Add Object to FormControls Generic List
                    properties.Add(epc);
                
            }

            return View(properties);
        }


        [HttpGet]
        public ActionResult Accept(int id)
        {

            var obj = Properties.FirstOrDefault(a => a.ProId == id && a.isApproved != true );
            if (obj != null)
            {
                obj.isApproved = true;
                UpdateModel(obj);
                SContextProp.SaveChanges();
                //return Content("the accpted is confirmed");
                var userId = this.Session["UserId"].ToString();
                int userID = Convert.ToInt32(userId);
                return RedirectToAction("index", new { id = userID });
            }
            else
                return Content("the accepted is not ok ");

        }



        [HttpGet]
        //[AllowAnonymous]
        public ActionResult Reject(int id)
        {
            var obj = Properties.FirstOrDefault(a => a.ProId == id && a.isApproved != false );
            if (obj != null)
            {
                obj.isApproved = false;
                UpdateModel(obj);
                SContextProp.SaveChanges();
                //return Content("the rejected is confirmed");
                return RedirectToAction("AddComment", new { id = obj.ProId });
                //return RedirectToAction("AddComment");
            }
            else
                return Content("Not working well, the obj may be null !!");

        }

        [Route("/AOProf/PropertyOfLandlord/AddComment/{id:int}")]
        [HttpGet]
        public ActionResult AddComment(int id)
        {
            ViewData["id"] = id;
            return View();
        }


        [HttpPost]
        public ActionResult AddComment(Property prop)
        {
            var obj = Properties.FirstOrDefault(a => a.ProId == prop.ProId);
            if (obj != null)
            {
                obj.UAOComment = prop.UAOComment;
                UpdateModel(prop.UAOComment);
                SContextProp.SaveChanges();
                return Content("comments is submitted");
                //return RedirectToAction("AddComment");
            }
            else
                return Content("it's not ok");     
          
        }
    }
}
