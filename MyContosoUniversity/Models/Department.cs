using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyContosoUniversity.Models
{
    public class Department
    {//Bölüm
        public int DepartmentID { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        [DataType(DataType.Currency), Column(TypeName = "money")]
        public decimal Budget { get; set; }//Bütçe

        [DataType(DataType.Date), Display(Name = "Başlangıç Tarihi")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM--dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        public int? InstructorID { get; set; }//Bölüme ait eğitmen olmayabilir.
        public virtual Instructor Administrator { get; set; }//Yönetici her zaman eğitmendir
        public virtual ICollection<Course> Courses { get; set; }//Bir bölümde birden fazla ders olaibilir.
    }
}