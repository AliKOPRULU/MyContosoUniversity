using MyContosoUniversity.Migrations;
using MyContosoUniversity.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace MyContosoUniversity.DAL
{
    public class SchoolContext : DbContext
    {//DataContext
        public SchoolContext() : base("SchoolContext")
        {//connection string bu olacak 
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<SchoolContext, Configuration>("SchoolContext"));
        }

        //entity (varlıklarını) burada tanımladık
        public DbSet<Course> Courses { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<OfficeAssignment> OfficeAssignments { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {//Tablo isimlerinin s takısı almasını engelledik
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //modelBuilder.Entity().HasRequired(d => d.Administrator).WithMany().WillCascadeOnDelete(false);//Modify the Enrollment Entity başlığının üstündeki yazıyı oku

            modelBuilder.Entity<Course>()//Burada CourseInstructor adında ara bir tablo oluşturduk, Sol anahtar CourseID ve sağ anahtar InstructorID ders-eğitmen ilişkisi
                .HasMany(c => c.Instructors).WithMany(i => i.Courses)
                .Map(t => t.MapLeftKey("CourseID")
                .MapRightKey("InstructorID")
                .ToTable("CourseInstructor"));

            modelBuilder.Entity<Instructor>()//Ofise atama için eğitmen gereklidir dedik.
                .HasOptional(p => p.OfficeAssignment).WithRequired(p => p.Instructor);
        }
    }
}