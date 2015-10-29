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
            var courses = db.Courses.OrderBy(d => d.Course_Start_Date);

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
            Course courses = db.Courses.Find(id);
            if (courses == null)
            {
                return HttpNotFound();
            }
            return View(courses);
        }

              

        public PartialViewResult CoursesByTypePartial(string id)
        {
            return PartialView(
                db.Courses.Where(r => r.Course_Type == id).OrderBy(r => r.Course_Start_Date).ToList());
        }
    }
}
