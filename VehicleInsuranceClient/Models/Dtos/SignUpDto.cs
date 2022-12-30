using System.ComponentModel.DataAnnotations;

namespace VehicleInsuranceClient.Models.Dtos
{
    public class SignUpDto
    {
        [Required(ErrorMessage = "Required")]
        [EmailAddress]
        //[RegularExpression(@"\b[A-Z0-9._%-]+@[A-Z0-9.-]+\.[A-Z]{2,4}\b", ErrorMessage = "Ivalid")]
        public string CustomerEmail { get; set; } = null!;

        [Required(ErrorMessage = "Must be eight characters including one uppercase letter, one lowercase letter, and one number or special character")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[#$^+=!*()@%&]).{8,}$", ErrorMessage = "Ivalid")]
        public string Password { get; set; } = null!;



        //[RegularExpression(@"^[a-zA-Z''-'\s]{2,30}$", ErrorMessage = "Ivalid")]
        [Required(ErrorMessage = "Must be between 2 to 30 character")]
        public string CustomerName { get; set; } = null!;

        //[RegularExpression("/(84|0[3|5|7|8|9])+([0-9]{8})/g;", ErrorMessage = "Ivalid")] // cai nay no bi khung ha ta
        //[Required(ErrorMessage = "Must start with '84', then have 0 and any 9 digits.")]
        [Required(ErrorMessage = "Required")]
        [Phone]
        public long CustomerPhone { get; set; }

        public string CustomerAddress { get; set; } = null!;
    }
}
