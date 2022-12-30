using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace VehicleInsuranceAPI.Models.Dtos
{
    public class SignUpDto
    {
        [Required(ErrorMessage = "Required")]
        [StringLength(30, ErrorMessage = "Max 30 characters")]
        [RegularExpression(@"\b[A-Z0-9._%-]+@[A-Z0-9.-]+\.[A-Z]{2,4}\b")]
        public string CustomerEmail { get; set; } = null!;

        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[#$^+=!*()@%&]).{8,}$")]
        [Required(ErrorMessage = "Must be eight characters including one uppercase letter, one lowercase letter, and one number or special character")]
        public string Password { get; set; } = null!;


        [RegularExpression(@"^[a-zA-Z''-'\s]{2,30}$")]
        [Required(ErrorMessage = "Must be between 2 to 30 character")]
        public string CustomerName { get; set; } = null!;

        [RegularExpression("/(84|0[3|5|7|8|9])+([0-9]{8})\b/g;")]
        [Required(ErrorMessage = "Must start with '84', then have 0 and any 9 digits.")]
        public long CustomerPhone { get; set; }
        public string CustomerAddress { get; set; }
    }
}
