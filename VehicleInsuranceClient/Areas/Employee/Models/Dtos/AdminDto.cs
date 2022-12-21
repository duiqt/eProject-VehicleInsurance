namespace VehicleInsuranceClient.Areas.Employee.Models.Dtos
{
    public class AdminDto
    {
        public int AdminId { get; set; }

        public string UserName { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}
