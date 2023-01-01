using System.ComponentModel.DataAnnotations;

namespace VehicleInsuranceAPI.Models.Dtos
{
    public class Policy
    {
    }
    public class PolicyLapsedDto
    {
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime PolicyDate { get; set; }
        public int PolicyNo { get; set; }
    }
}
