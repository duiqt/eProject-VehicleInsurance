using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleInsuranceClient.Models.Dtos
{
    public class ClaimDto
    {
        public int Id { get; set; }

        public int ClaimNo { get; set; }

        public int PolicyNo { get; set; }

        public string PlaceOfAccident { get; set; } = null!;

        public DateTime DateOfAccident { get; set; }

        public string Description { get; set; } = null!;

        public string? Status { get; set; }

        public string? Image { get; set; }

        public decimal InsuredAmount { get; set; }

        public decimal ClaimableAmount { get; set; }

    }
    public class CreateClaimDto
    {

        public int Id { get; set; }
        public int ClaimNo { get; set; }

        public int PolicyNo { get; set; }

        public string PlaceOfAccident { get; set; } = null!;

        public DateTime DateOfAccident { get; set; }

        public string Description { get; set; } = null!;

        public string? Image { get; set; }

        public decimal InsuredAmount { get; set; }

        [NotMapped]
        [FileExtension]
        public IFormFile? ImageUpload { get; set; }

        public int CustomerId { get; set; }
    }
}
