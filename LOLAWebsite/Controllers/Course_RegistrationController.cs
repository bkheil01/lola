using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LOLAWebsite.Models;
using System.Threading.Tasks;
using Stripe;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace LOLAWebsite.Controllers
{

    public class Course_RegistrationController : Controller
    {
        private LOLADBEntities db = new LOLADBEntities();
        
        [HttpGet]
        public ActionResult Charge(int CourseID)
        {
            TempData["courseid"] = CourseID;
            var course = db.Courses.Find(CourseID);
            return View(new CourseRegistrationModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult>Charge(CourseRegistrationModel model)
        {
            //if(!ModelState.IsValid)
            //{
            //    return View(model);
            //}

            var chargeId = await ProcessPayment(model, (int)TempData["courseid"] );
            Course @course = db.Courses.Find((int)TempData["courseid"]);

            var courseReg = new Course_Registration();

            foreach (var p in model.Participant)
            {
                if (p.Name != null)
                {
                    courseReg = new Course_Registration()
                    {
                        Transaction_ID = chargeId,
                        Course_ID = (int)TempData["courseid"],
                        Id = User.Identity.GetUserId(),
                        P_Name = p.Name,
                        P_Phone = p.PhoneNumber,
                        P_UnderAge = p.UnderAge
                    };
                    db.Course_Registration.Add(courseReg);
                    @course.Participating_Students++;
                    db.SaveChanges();
                }
            }          
            return View("PaymentSuccessful");
        }
        
        private async Task<string> ProcessPayment(CourseRegistrationModel model, int id)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());
            string userEmail = currentUser.Email;
            return await Task.Run(() =>
            {
                Course course = db.Courses.Find(id);
                var myCharge = new StripeChargeCreateOptions
                {
                    Amount = (int)(course.Course_Cost * model.NumberOfParticipants * 100),
                    Currency = "usd",
                    Description = "Description for test charge",
                    ReceiptEmail = userEmail,
                    Source = new StripeSourceOptions
                    {
                        TokenId = model.Token
                    }
                };

                var chargeService = new StripeChargeService("sk_test_yPi2XADkAP3wiS1i6tkjErxZ");
                var stripeCharge = chargeService.Create(myCharge);

                return stripeCharge.Id;
            });
        }

        public ActionResult PaymentSuccessful()
        {
            return View();
        }

        public ActionResult FreeRegistration(int CourseID)
        {
            TempData["courseid"] = CourseID;
            var course = db.Courses.Find(CourseID);
            return View(new CourseRegistrationModel());
        }

        [HttpPost]
        public ActionResult FreeRegistration(CourseRegistrationModel model)
        {
            var courseReg = new Course_Registration();
            Course @course = db.Courses.Find((int)TempData["courseid"]);

            foreach (var p in model.Participant)
            {
                if (p.Name != null)
                {
                    courseReg = new Course_Registration()
                    {
                        Transaction_ID = "Free Course",
                        Course_ID = (int)TempData["courseid"],
                        Id = User.Identity.GetUserId(),
                        P_Name = p.Name,
                        P_Phone = p.PhoneNumber,
                        P_UnderAge = p.UnderAge
                    };
                    db.Course_Registration.Add(courseReg);
                    @course.Participating_Students++;
                    db.SaveChanges();
                }
            }
            return View("PaymentSuccessful");
        }
    }
}
