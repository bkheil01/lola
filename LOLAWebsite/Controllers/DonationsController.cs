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
using System.Text;

namespace LOLAWebsite.Controllers
{

    public class DonationsController : Controller
    {
        private LOLADBEntities db = new LOLADBEntities();

        public ActionResult Donations()
        {
            return View();
        }


        [Authorize]
        public ActionResult Donate()
        {
            List<string> categories = new List<string>();
            categories.Add("General Fund");
            categories.Add("Sandy Hardy Brown - Classroom Fund");
            categories.Add("Scholarship Fund");
            categories.Add("Other (example - In Memory or In Honor of");
            ViewBag.Category_List = new SelectList(categories);
            
            return View(new NewDonationModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Donate(NewDonationModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.Type == true)
            {
                var plan = CreatePlan(model);
 
                var customer = GetCustomer(model, plan);

                var chargeId = await SubscriptionDonation(model, customer);

                var donate = new Donation()
                {
                    Donation_Type = "Subscription - Monthly",
                    Donation_Category = model.Category,
                    Id = User.Identity.GetUserId(),
                    Donation_Amount = model.Amount
                };

                db.Donations.Add(donate);
                db.SaveChanges();

            }
            else
            {
                var chargeId = await OneTimeProcessPayment(model);
                var donate = new Donation()
                {
                    Donation_Type = "One Time",
                    Donation_Category = model.Category,
                    Id = User.Identity.GetUserId(),
                    Donation_Amount = model.Amount
                };

                db.Donations.Add(donate);
                db.SaveChanges();

            }


            return View("PaymentSuccessful");
        }


        private StripePlan CreatePlan(NewDonationModel model)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());


            var myPlan = new StripePlanCreateOptions();
            myPlan.Amount = (int)(model.Amount*100);
            myPlan.Currency = "usd";
            myPlan.Interval = "month";
            myPlan.Name = currentUser.FirstName + "-" + currentUser.LastName + "-" + model.Amount + "-" + model.Category;
            myPlan.Id = currentUser.FirstName + "-" + currentUser.LastName + "-" + model.Amount + "-" + model.Category;

            var planService = new StripePlanService("sk_test_yPi2XADkAP3wiS1i6tkjErxZ");
            return planService.Create(myPlan);
        }

        private StripeCustomer GetCustomer(NewDonationModel model, StripePlan plan)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());
            string userEmail = currentUser.Email;
            var myCustomer = new StripeCustomerCreateOptions
            {
                Email = userEmail,
                Description = currentUser.FirstName + currentUser.LastName,
                Source = new StripeSourceOptions()
                {
                    TokenId = model.Token
                }
            };

            myCustomer.PlanId = plan.Id;

            var customerService = new StripeCustomerService("sk_test_yPi2XADkAP3wiS1i6tkjErxZ");
            return customerService.Create(myCustomer);
        }


        private async Task<string> OneTimeProcessPayment(NewDonationModel model)
        {
            return await Task.Run(() =>
                {
                    var myCharge = new StripeChargeCreateOptions {
                        Amount = (int)(model.Amount * 100),
                        Currency = "usd",
                        ReceiptEmail = "test@test.com",
                        Source = new StripeSourceOptions {
                            TokenId = model.Token
                        }
                    };

                    var chargeService = new StripeChargeService("sk_test_yPi2XADkAP3wiS1i6tkjErxZ");
                    var stripeCharge = chargeService.Create(myCharge);

                    return stripeCharge.Id;
                });
        }


        private async Task<string> SubscriptionDonation(NewDonationModel model, StripeCustomer customer)
        {
            return await Task.Run(() =>
                {
                    var myCharge = new StripeChargeCreateOptions
                    {
                        Amount = (int)(model.Amount * 100),
                        Currency = "usd",
                        ReceiptEmail = customer.Email,
                        CustomerId = customer.Id
                    };

                    var chargeService = new StripeChargeService("sk_test_yPi2XADkAP3wiS1i6tkjErxZ");
                    var stripeCharge = chargeService.Create(myCharge);

                    return stripeCharge.Id;
                });
        }


    }
}
