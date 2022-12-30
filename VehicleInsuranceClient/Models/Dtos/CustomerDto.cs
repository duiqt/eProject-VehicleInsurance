namespace VehicleInsuranceClient.Models.Dtos
{
    public class CustomerDto
    {
        public int Id { get; set; }

        public string CustomerEmail { get; set; } = null!;

        public string CustomerName { get; set; } = null!;

        public string? CustomerAddress { get; set; }

        public long CustomerPhone { get; set; }

        public string Password { get; set; }
        public string? ChangePassword { get; set; }
    }
}
