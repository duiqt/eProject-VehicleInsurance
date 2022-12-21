using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace VehicleInsuranceAPI.Models.Dtos
{
    public class SignUpDto
    {
        [Required(ErrorMessage = "User is required")]
        [StringLength(16, ErrorMessage = "Must be between 3 and 16 characters", MinimumLength = 3)]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        //[StringLength(20, ErrorMessage = "Must be eight characters including one uppercase letter, one lowercase letter, and one number or special character", MinimumLength = 8)]
        //[RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[#$^+=!*()@%&]).{8,}$")]
        public string Password { get; set; } = null!;


        [Required(ErrorMessage = "Customer Name is required")]
        [StringLength(30, ErrorMessage = "Must be 10 number", MinimumLength = 2)]
        public string CustomerName { get; set; } = null!;
        

        [Required(ErrorMessage = "Customer Phone is required")]
        [StringLength(10, ErrorMessage = "Must be 10 digit", MinimumLength = 10)]
        public string CustomerPhone { get; set; }
    }
}
