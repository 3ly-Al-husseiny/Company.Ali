// Change Password DTO file
using System.ComponentModel.DataAnnotations;

namespace Company.Ali.PL.DTOs
{
    public class ChangePassword
    {
        [Required(ErrorMessage = "Password is Required !")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm Password is Required !")]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword))]
        public string ConfirmPassword { get; set; }
    }
}
