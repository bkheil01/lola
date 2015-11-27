using LOLAWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Owin;

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
        [ValidateAntiForgeryToken]
        public ActionResult PersonalInfo(LOLAWebsite.Models.ApplicationUser model)
        {
            ViewBag.Message = "Changes Made.";
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

        // GET : /UserAccounts/EventFeedback
        public ActionResult EventFeedback(int? eventId)
        {
            TempData["eventid"] = eventId;
            return View(new Event_Feedback());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EventFeedback(LOLAWebsite.Models.Event_Feedback model)
        {
            Event @event = db.Events.Find((int)TempData["eventid"]);
            if (model.Id != null)
            {
                var thisFeedback = new Event_Feedback()
                {
                    Event_ID = @event.Event_ID,
                    Event_Marketing = model.Event_Marketing,
                    Event_Rating = model.Event_Rating,
                    Event_Registration_Type = model.Event_Registration_Type,
                    Student_Comment = model.Student_Comment,
                    Id = User.Identity.GetUserId(),
                };
                db.Event_Feedback.Add(thisFeedback);
                db.SaveChanges();
            }
            return View("Changes Saved");
        }

        // GET : /UserAccounts/ClassFeedback
        public ActionResult CourseFeedback(int? courseId)
        {
            TempData["courseid"] = courseId;
            return View(new Course_Feedback());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CourseFeedback(LOLAWebsite.Models.Course_Feedback model)
        {
            Course @course = db.Courses.Find((int)TempData["courseid"]);
            if (model.Id != null)
            {
                var thisFeedback = new Course_Feedback()
                {
                    Course_ID = @course.Course_ID,
                    Course_Marketing = model.Course_Marketing,
                    Course_Rating = model.Course_Rating,
                    Course_Registration_Type = model.Course_Registration_Type,
                    Student_Comment = model.Student_Comment,
                    Id = User.Identity.GetUserId(),
                };
                db.Course_Feedback.Add(thisFeedback);
                db.SaveChanges();
            }
            return View("Changes Saved");
        }
        
    }
}