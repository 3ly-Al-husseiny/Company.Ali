using System.ComponentModel.DataAnnotations;

namespace Company.Ali.PL.DTOs
{
    public class CreateDepartmentDto
    {
        [Required(ErrorMessage = "Code is Rquired!")]
        public string Code { get; set; }
        [Required(ErrorMessage ="Name is Rquired!")]
        public string Name { get; set; }
        
        
        [Required(ErrorMessage ="CreateAt is Required!")]
        public DateTime CreateAt { get; set; }
    }
}
