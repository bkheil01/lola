using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LOLAWebsite.Models;

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
            return View(courses);
        }

        // GET: Courses/Create
        public ActionResult Create()
        {
            ViewBag.Teacher_ID = new SelectList(db.Teachers, "Teacher_ID", "Id");
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Course_ID,Course_Type,Course_Desc,Teacher_ID,Course_Max_Size,Course_Start_Date,Course_End_Date,Course_Time,Course_Cost,Course_Location,Course_Notes")] Courses courses)
        {
            if (ModelState.IsValid)
            {
                db.Courses.Add(courses);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Teacher_ID = new SelectList(db.Teachers, "Teacher_ID", "Id", courses.Teacher_ID);
            return View(courses);
        }

        // GET: Courses/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.Teacher_ID = new SelectList(db.Teachers, "Teacher_ID", "Id", courses.Teacher_ID);
            return View(courses);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Course_ID,Course_Type,Course_Desc,Teacher_ID,Course_Max_Size,Course_Start_Date,Course_End_Date,Course_Time,Course_Cost,Course_Location,Course_Notes")] Courses courses)
        {
            if (ModelState.IsValid)
            {
                db.Entry(courses).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Teacher_ID = new SelectList(db.Teachers, "Teacher_ID", "Id", courses.Teacher_ID);
            return View(courses);
        }

        // GET: Courses/Delete/5
        public ActionResult Delete(int? id)
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
            return View(courses);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Courses courses = db.Courses.Find(id);
            db.Courses.Remove(courses);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public PartialViewResult CoursesByTypePartial(string id)
        {
            return PartialView(
                db.Courses.Where(r => r.Course_Type == id).OrderBy(r => r.Course_Start_Date).ToList());
        }
    }
}
