using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using LOLAWebsite.Models;
using System.Text;

namespace LOLAWebsite.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {   
            return View();
        }

        public ActionResult Donations()
        {
            ViewBag.Message = "Your donation page.";

            return View();
        }

        public ActionResult Photos()
        {
            ViewBag.Message = "Your media page.";

            return View();
        }

        public ActionResult Success()
        {
            ViewBag.Message = "Successful Email";

            return View();
        }

        public ActionResult ArtImpression()
        {
            ViewBag.Message = "Art Impression Page";

            return View();
        }

        public ActionResult Scholarship()
        {
            return View();
        }

        public ActionResult AboutUs()
        {
            return View();
        }

        public ActionResult Links()
        {
            return View();
        }
    }
}