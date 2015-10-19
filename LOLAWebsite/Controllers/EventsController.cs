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
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Stripe;

namespace LOLAWebsite.Controllers
{
    public class EventsController : Controller
    {
        private LOLADBEntities db = new LOLADBEntities();

        // GET: Events
        public ActionResult Index()
        {
            var events = db.Events.OrderBy(e => e.Event_Start_Date);
            return View(events.ToList());
        }

        // GET: Events/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Details(Event model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var chargeId = await ProcessPayment(model);

            var courseReg = new Event_Registration()
            {
                Transaction_ID = 123,
                Event_ID = model.Event_ID,
                Id = User.Identity.GetUserId()
            };

            db.Event_Registration.Add(courseReg);
            db.SaveChanges();

            return View("PaymentSuccessful");
        }
        private async Task<string> ProcessPayment(Event model)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());
            string userEmail = currentUser.Email;
            return await Task.Run(() =>
            {
                var myCharge = new StripeChargeCreateOptions
                {
                    Amount = (int)(model.Event_Cost * 100),
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
    }
}
