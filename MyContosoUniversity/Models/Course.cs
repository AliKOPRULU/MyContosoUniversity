using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyContosoUniversity.Models
{//Müfredat
    public class Course//Öğrenci ile Ders arasına çoka çok ilişki vardır //Eğitmen ile ders arasında çoka çok ilişki vardır
    {//Dersler
        //Yalnız bu DatabaseGenerated  niteliğinin tam anlamadım.
        //Kullanıcı departman için 1.000 seri oluşturabilir veya farklı bir departman için 2.000 seri oluşturabilirmiş bu yüzden veritabanı için bir değer oluşturulmaz.
        [DatabaseGenerated(DatabaseGeneratedOption.None)]//EF veritabanın da ID'yi varsayılan olarak PrimeryKey olarak oluşturuyormuş. Bunu engelliyoruz.
        [Display(Name = "Sınıf No")]
        public int CourseID { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string Title { get; set; }

        [Range(0, 5)]
        public int Credits { get; set; }

        //Buradaki ilşki de  aslında yabancı anahtar ama foreignKey olarak belirtmek gerekmiyor.
        public int DepartmantID { get; set; }//Bir Ders bir bölüme atanıyor.
        public virtual Department Department { get; set; }

        //Bir ders herhangi bir numarayla bir öğrenciye kayıt edilmiş olabilir.
        public virtual ICollection<Enrollment> Enrollments { get; set; }

        //Bir Ders birden fazla eğitmene verilmiş olabilir.
        public virtual ICollection<Instructor> Instructors { get; set; }
    }
}