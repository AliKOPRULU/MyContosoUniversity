using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyContosoUniversity.Models
{
    public class Department//Bir Bölümde birden fazla ders olabilir olarak alınmış
    {//aslında bir ders birden fazla bölümdede olması lazım ama öyle alınmamış, bir ders sadece tek bölümde okutuluyor görünüyor
        public int DepartmentID { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        [DataType(DataType.Currency), Column(TypeName = "money")]
        public decimal Budget { get; set; }//Bütçe

        [DataType(DataType.Date), Display(Name = "Başlangıç Tarihi")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM--dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        public int? InstructorID { get; set; }//Bölüme ait eğitmen olmayabilir.
        public virtual Instructor Administrator { get; set; }//Bölüm başkanı olarak ele alınmış, hocaları koymamış, yönetici her zaman eğitmendir
        public virtual ICollection<Course> Courses { get; set; }//Bir bölümde birden fazla ders olaibilir.
    }
}