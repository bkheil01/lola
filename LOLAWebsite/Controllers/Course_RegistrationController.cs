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
    [Authorize]
    public class Course_RegistrationController : Controller
    {
        private LOLADBEntities db = new LOLADBEntities();


        // GET: Course_Registration
        public ActionResult Charge(int CourseID)
        {
            TempData["course"] = CourseID;
            return View(new StripeChargeModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult>Charge(StripeChargeModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            var chargeId = await ProcessPayment(model, (int)TempData["courseid"] );

            var courseReg = new Course_Registration()
            {
                Transaction_ID = 123,
              Course_ID = (int)TempData["courseid"],
                Id = User.Identity.GetUserId()
            };

            db.Course_Registration.Add(courseReg);
            db.SaveChanges();
            
            return View("PaymentSuccessful");
        }
        
        private async Task<string> ProcessPayment(StripeChargeModel model, int id)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());
            string userEmail = currentUser.Email;
            return await Task.Run(() =>
            {
                Courses course = db.Courses.Find(id);
                var myCharge = new StripeChargeCreateOptions
                {
                    Amount = (int)(course.Course_Cost * 100),
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
    }
}
