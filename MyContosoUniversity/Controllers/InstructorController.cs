using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyContosoUniversity.DAL;
using MyContosoUniversity.Models;
using MyContosoUniversity.ViewModels;
using System.Data.Entity.Infrastructure;

namespace MyContosoUniversity.Controllers
{
    public class InstructorController : Controller
    {
        private SchoolContext db = new SchoolContext();

        public ActionResult Index(int? id, int? courseID)
        {
            var viewModel = new InstructorIndexData();
            viewModel.Instructors = db.Instructors//eğitmen listesini aldık
                .Include(i => i.OfficeAssignment)//eğitmenlere ait ofisleri yükledik
                .Include(i => i.Courses.Select(c => c.Department))//Dersleri yükledik ve herbir ders için bölümü yüklemiş olduk.
                .OrderBy(i => i.LastName);
            if (id != null)
            {//eğer bir eğitmen ID'si gelirse eğitmenin dersleri de gelecek
                ViewBag.InstructorID = id.Value;
                viewModel.Courses = viewModel.Instructors.Where(
                    i => i.ID == id.Value).Single().Courses;
            }
            //if (courseID != null) //---Lazy loading
            //{//eğer ders ID'si gelirse 
            //    ViewBag.CourseID = courseID.Value;
            //    viewModel.Enrollments = viewModel.Courses.Where(
            //        x => x.CourseID == courseID).Single().Enrollments;
            //}
            if (courseID != null)// Explicit loading
            {//eğer ders ID'si gelirse kayıtlı öğrenciler geliyor.
                var selectedCourse = viewModel.Courses.Where(x => x.CourseID == courseID).Single();
                db.Entry(selectedCourse).Collection(x => x.Enrollments).Load();//Ders ID'si yukardan gelen Enrollments lar çekiliyor. 
                foreach (Enrollment enrollment in selectedCourse.Enrollments)
                {//Kayıtlı öğrenciler 
                    db.Entry(enrollment).Reference(x => x.Student).Load();
                }
                viewModel.Enrollments = selectedCourse.Enrollments;
            }
            return View(viewModel);
        }

        //GET: Instructors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructor instructor = db.Instructors.Find(id);
            if (instructor == null)
            {
                return HttpNotFound();
            }
            return View(instructor);
        }

        // GET: Instructors/Create
        public ActionResult Create()
        {
            var instructor = new Instructor();
            instructor.Courses = new List<Course>();
            PopulateAssignedCourseData(instructor);
            return View();
        }

        // POST: Instructors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LastName,FirstMidName,HireDate,OfficeAssignment")] Instructor instructor, string[] selectedCourses)
        {
            if (selectedCourses != null)
            {
                instructor.Courses = new List<Course>();
                foreach (var course in selectedCourses)
                {
                    var courseToAdd = db.Courses.Find(int.Parse(course));
                    instructor.Courses.Add(courseToAdd);
                }
            }
            if (ModelState.IsValid)
            {
                db.Instructors.Add(instructor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            PopulateAssignedCourseData(instructor);
            return View(instructor);
        }

        // GET: Instructors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Instructor instructor = db.Instructors.Find(id);
            Instructor instructor = db.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.Courses)
                .Where(i => i.ID == id)
                .Single();
            PopulateAssignedCourseData(instructor);
            if (instructor == null)
            {
                return HttpNotFound();
            }
            //ViewBag.ID = new SelectList(db.OfficeAssignments, "InstructorID", "Location", instructor.ID);
            return View(instructor);
        }

        private void PopulateAssignedCourseData(Instructor instructor)
        {//check box dizi şeklinde dersleri getireceğiz. Sayfa ilk defa açıldığında var ise Kullanıcının seçtiği dersleri alıyoruz.
            var allCourses = db.Courses;
            var instructorCourses = new HashSet<int>(instructor.Courses.Select(c => c.CourseID));
            var viewModel = new List<AssignedCourseData>();
            foreach (var course in allCourses)
            {
                viewModel.Add(new AssignedCourseData
                {
                    CourseID = course.CourseID,
                    Title = course.Title,
                    Assigned = instructorCourses.Contains(course.CourseID)
                });
            }
            ViewBag.Courses = viewModel;
        }

        // POST: Instructors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id, string[] selectedCourses)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var instructorToUpdate = db.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.Courses)
                .Where(i => i.ID == id)
                .Single();
            if (TryUpdateModel(instructorToUpdate, "", new string[] { "LastName", "FirstMidName", "HireDate", "OfficeAssignment" }))
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(instructorToUpdate.OfficeAssignment.Location))
                    {
                        instructorToUpdate.OfficeAssignment = null;

                    }
                    UpdateInstructorCourses(selectedCourses, instructorToUpdate);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            PopulateAssignedCourseData(instructorToUpdate);
            return View(instructorToUpdate);
        }

        private void UpdateInstructorCourses(string[] selectedCourses, Instructor instructorToUpdate)
        {//Seçili dersler checkbox ile kontrol edilecek            
            if (selectedCourses == null)
            {//Seçili ders varmı yoksa boş ders listesi oluşturuluyor.                    
                instructorToUpdate.Courses = new List<Course>();
                return;
            }

            var selectedCoursesHS = new HashSet<string>(selectedCourses);
            var instructorCourses = new HashSet<int>
                (instructorToUpdate.Courses.Select(c => c.CourseID));
            foreach (var course in db.Courses)
            {//Bütün dersleri getiriyor. 
                if (selectedCoursesHS.Contains(course.CourseID.ToString()))
                {//seçili kurslarla karşılaştırıyor
                    if (!instructorCourses.Contains(course.CourseID))
                    {//eğer seçili ders veritabanında yok seçili olarak görünmüyor ise eklenmek üzere instructorToUpdate'e gönderiliyor.
                        instructorToUpdate.Courses.Add(course);
                    }
                }
                else
                {//eğer ders seçilmemiş ise 
                    if (instructorCourses.Contains(course.CourseID))
                    {//ve veritabanında seçili görünüyor ise silinmek üzere instructorToUpdate' gönderiliyor.
                        instructorToUpdate.Courses.Remove(course);
                    }
                }
            }
        }

        // GET: Instructors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructor instructor = db.Instructors.Find(id);
            if (instructor == null)
            {
                return HttpNotFound();
            }
            return View(instructor);
        }

        // POST: Instructors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Instructor instructor = db.Instructors.Find(id);
            db.Instructors.Remove(instructor);
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
    }
}
