using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyContosoUniversity.Models
{
    public class Course
    {        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]//veritabanı için bir değer oluşturulmaz
        public int CourseID { get; set; }
        public string Title { get; set; }
        public int Credits { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}