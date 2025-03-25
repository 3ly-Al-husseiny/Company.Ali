using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Company.Ali.PL.DTOs
{
    public class SignUpDto
    {
        [Required(ErrorMessage = "UserName is Required !")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "First is Required !")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "LastName is Required !")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email is Required !")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is Required !")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm Password is Required !")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Agree on Our Policy is Required to Continue")]
        public bool IsAgree { get; set; }
    }
}
