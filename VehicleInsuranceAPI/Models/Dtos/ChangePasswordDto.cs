using System.ComponentModel.DataAnnotations;

namespace VehicleInsuranceAPI.Models.Dtos
{
    public class ChangePasswordDto
    {
        //[Required]
        public int Id { get; set; }

        //[RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[#$^+=!*()@%&]).{8,}$")]
        //[Required(ErrorMessage = "Must be eight characters including one uppercase letter, one lowercase letter, and one number or special character")]
        public string Password { get; set; }

        //[RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[#$^+=!*()@%&]).{8,}$")]
        //[Required(ErrorMessage = "Must be eight characters including one uppercase letter, one lowercase letter, and one number or special character")]
        public string ChangePassword { get; set; }

        //[RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[#$^+=!*()@%&]).{8,}$")]
        //[Required(ErrorMessage = "Must be eight characters including one uppercase letter, one lowercase letter, and one number or special character")]
        public string ConfirmPassword { get; set; }
    }
}
