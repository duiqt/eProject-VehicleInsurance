using System.ComponentModel.DataAnnotations;

namespace VehicleInsuranceClient.Areas.Employee.Models.Dtos
{
    public class LoginAdminDtos
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
