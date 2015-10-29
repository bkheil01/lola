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

    }
}
