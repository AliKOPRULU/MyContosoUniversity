using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MyContosoUniversity.Models
{
    public class Instructor//Eğitmen ile ders arasında çoka çok ilişki vardır
    {//Eğitmen Modeli
        public int ID { get; set; }

        [Required, StringLength(50), Display(Name = "Soyadı")]
        public string LastName { get; set; }

        [Required, Column("FirstName"), StringLength(50), Display(Name = "Adı")]
        public string FirstMidName { get; set; }

        [DataType(DataType.Date), Display(Name = "Kira Tarihi")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime HireDate { get; set; }

        [Display(Name = "Tam Adı")]
        public string FullName
        {
            get { return LastName + ", " + FirstMidName; }
        }

        //Bir eğitmen birden fazla ders verebilir
        public virtual ICollection<Course> Courses { get; set; }

        //Bizim iş rolünde eğitmen OfficeAssignment (atanacak ofis) tanımlama birebir belki null veya atama yapılmayacak
        public virtual OfficeAssignment OfficeAssignment { get; set; }
    }
}