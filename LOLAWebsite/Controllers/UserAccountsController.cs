using LOLAWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LOLAWebsite.Controllers
{
    [Authorize]
    public class UserAccountsController : Controller
    {
        private LOLADBEntities db = new LOLADBEntities();

        // GET: /UserAccounts/Index
        public ActionResult Index()
        {
            ViewBag.Message = "Account Overview page.";
            return View();
        }

        // GET: /UserAccounts/PersonalInfo
        public ActionResult PersonalInfo()
        {
            ViewBag.Message = "Edit personal information.";
            return View();
        }

        // POST : /UserAccounts/PersonalInfo
        [HttpPost]
        public ActionResult PersonalInfo(string message)
        {
            ViewBag.Message = "Personal information changed.";
            return View();
        }

        // GET : /UserAccounts/ClassHistory
        public ActionResult CourseHistory()
        {
            ViewBag.Message = "You have signed up for these classes.";
            return View();
        }

        // GET : /UserAccounts/EventHistory
        public ActionResult EventHistory()
        {
            ViewBag.Message = "These are your future and past events!";
            return View();
        }

        // GET : /UserAccounts/DonationHistory
        public ActionResult DonationHistory()
        {
            ViewBag.Message = "These are the donations you've made.";
            return View();
        }
        
        /* Future planning for Family administration when it is 
         * worked out in the database.
         * 
        // GET : /UserAccounts/FamilyInfo
        public ActionResult FamilyInfo()
        {
            ViewBag.Message = "Edit your family information.";
            return View();
        }
        
        // POST : /UserAccounts/FamilyInfo
        [HttpPost]
        public ActionResult FamilyInfo()
        {
            ViewBag.Message = "You have edited your family information.";
            return View();
        }
         */
    }
}