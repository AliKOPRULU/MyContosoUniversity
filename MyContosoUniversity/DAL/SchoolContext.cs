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
        public DbSet<Student> Students { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Course> Courses { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {//Tablo isimlerinin s takısı almasını engelledik
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}