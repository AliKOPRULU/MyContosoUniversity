using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyContosoUniversity.Models
{
    public class OfficeAssignment
    {
        [Key]//Ofis Atama ile eğitmen arasında one-to-zero-or-one ilişki kuran varlık
        [ForeignKey("Instructor")]//Burada ana tablonun yabancı anahtarı olduğunu belirtiyoruz. Sonra Fluent api ile göstereceğiz 
        public int InstructorID { get; set; }

        [StringLength(50),Display(Name ="Ofis Konumu")]
        public string Location { get; set; }

        //Eğitmen olmadan ofise atama olamayacağı için birebir ilişki oluşturduk
        public virtual Instructor Instructor { get; set; }
    }
}