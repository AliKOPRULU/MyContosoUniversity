using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MyContosoUniversity.Models
{
    public class Student//Öğrenci ile Ders arasına çoka çok ilişki vardır
    {
        public int ID { get; set; }
        [Required, StringLength(50, MinimumLength = 2), Display(Name = "Soyadı")]
        public string LastName { get; set; }
        [Required, StringLength(50, ErrorMessage = "İsim 50 kaakterden fazla olamaz."), Column("FirstName"), Display(Name = "Adı")]
        public string FirstMidName { get; set; }
        [DataType(DataType.Date), Display(Name = "Kayıt Tarihi")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EnrollmentDate { get; set; }

        [Display(Name = "Tam Adı")]
        public string FullName
        {
            get { return LastName + ", " + FirstMidName; }

        }

        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}