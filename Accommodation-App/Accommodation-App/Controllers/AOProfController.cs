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


        // in the AO profile, list all the registered landlords 
        //[Authorize(Roles = "admin")]
        [Route("/AOProf/Index/{id:int}")]
        [HttpGet]
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

        // List the properties of the selected landlord
        //[Authorize(Roles = "admin")]
        [Route("/AOProf/LandlordPropertiesPage/{id:int}")]
        [HttpGet]
        public ViewResult LandlordPropertiesPage(int id)
        {

            var controls = from c in SContextProp.Properties
                           where c.OwnerId == id
                           select c;

            List<Property> properties = new List<Property>();

            User AO = UserDb.Find(id);
            ViewData["UserName"] = AO.UserName.ToString();

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


        // Details about the specific property
        //[Authorize(Roles = "admin")]
        [Route("/AOProf/PropertyDetails/{id:int}")]
        [HttpGet]
        public ViewResult PropertyDetails(int id)
        {
            Property property = Properties.Find(id);
            return View(property);
        }


        [HttpGet]
        public ActionResult Accept(int id)
        {
            var obj = Properties.FirstOrDefault(a => a.ProId == id);
            if (obj != null)
            {
                obj.isApproved = true;
                obj.UAOComment = "Approved";
                UpdateModel(obj);
                SContextProp.SaveChanges();
                var userId = this.Session["UserId"].ToString();
                int userID = Convert.ToInt32(userId);
                //return Content("the accpted is confirmed");
                return RedirectToAction("index", new { id = userID });
            }
            else
                return Content("It's already accepted ");
        }



        [HttpGet]
        public ActionResult Reject(int id)
        {
            var obj = Properties.FirstOrDefault(a => a.ProId == id && a.isApproved != false);
            if (obj != null)
            {
                obj.isApproved = false;
                UpdateModel(obj);
                SContextProp.SaveChanges();
                return RedirectToAction("AddComment", new { id = obj.ProId });
            }
            else
                return Content("It's already Rejected !!");

        }

        //[Authorize(Roles = "admin")]
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
            if (ModelState.IsValid)
            {
                var obj = Properties.FirstOrDefault(a => a.ProId == prop.ProId);
                if (obj != null)
                {
                    obj.UAOComment = prop.UAOComment;
                    UpdateModel(obj);
                    SContextProp.SaveChanges();
                    //ViewBag.SuccessMessage = "<p>Comments is Submitted Successfully!</p>";
                    //return Content("comments is submitted");
                    return RedirectToAction("AddCommentSuccess");
                }
                else
                    return Content("You cannot add comments !!");
            }
            else
                return View();
        }

        public ActionResult AddCommentSuccess()
        {
            return View();
        }
    }
}
