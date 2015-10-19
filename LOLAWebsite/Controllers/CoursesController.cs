using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LOLAWebsite.Models;
using Stripe;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace LOLAWebsite.Controllers
{
    public class CoursesController : Controller
    {
        private LOLADBEntities db = new LOLADBEntities();

        // GET: Courses
        public ActionResult Index()
        {
            var courses = db.Courses.Include(c => c.Teacher).OrderBy(d => d.Course_Start_Date);

            var courseTypes = new SelectList(
                db.Courses.Select(r => r.Course_Type).Distinct().ToList());

            ViewBag.CourseTypes = courseTypes;


            return View(courses.ToList());
        }
    

        // GET: Courses/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Courses courses = db.Courses.Find(id);
            if (courses == null)
            {
                return HttpNotFound();
            }
            TempData["course"] = courses;
            return View(courses);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Details(Courses model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var chargeId = await ProcessPayment(model);

            var courseReg = new Course_Registration()
            {
                Transaction_ID = 123,
                Course_ID = model.Course_ID,
                Id = User.Identity.GetUserId()
            };

            db.Course_Registration.Add(courseReg);
            db.SaveChanges();

            return View("PaymentSuccessful");
        }
        private async Task<string> ProcessPayment(Courses model)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());
            string userEmail = currentUser.Email;
            return await Task.Run(() =>
            {
                var myCharge = new StripeChargeCreateOptions
                {
                    Amount = (int)(model.Course_Cost * 100),
                    Currency = "usd",
                    Description = "Course Registration",
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
        

        public PartialViewResult CoursesByTypePartial(string id)
        {
            return PartialView(
                db.Courses.Where(r => r.Course_Type == id).OrderBy(r => r.Course_Start_Date).ToList());
        }
    }
}
