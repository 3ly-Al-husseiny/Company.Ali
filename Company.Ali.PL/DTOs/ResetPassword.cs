using System.ComponentModel.DataAnnotations;

namespace Company.Ali.PL.DTOs
{
    public class ResetPassword
    {
        [Required(ErrorMessage = "Email is Required !")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
