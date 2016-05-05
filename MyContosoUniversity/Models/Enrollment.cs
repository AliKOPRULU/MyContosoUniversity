namespace MyContosoUniversity.Models
{
    public enum Grade
    {//Sınıf
        A, B, C, D, F
    }
    public class Enrollment
    {//Kayıt
        public int EnrollmentID { get; set; }
        public int CourseID { get; set; }
        public int StudentID { get; set; }
        //Sınıf null olabilir, henüz atanmamış //Çekirdek veriler eklenirken de bazılarına sınıf ataması yapılmamış.
        public Grade? Grade { get; set; }

        public virtual Course Course { get; set; }
        public virtual Student Student { get; set; }
    }
}