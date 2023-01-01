using System.ComponentModel.DataAnnotations;

namespace VehicleInsuranceAPI.Models
{
    public class CustomerBillModel
    {
        public int Id { get; set; }
        public int BillNo { get; set; }

        public int PolicyNo { get; set; }

        public string? Status { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]

        public DateTime? Date { get; set; }

        public decimal? Amount { get; set; }
    }

    public class CustomerBillUpdateModel
    {
        public int Id { get; set; }
        public int BillNo { get; set; }
        public int PolicyNo { get; set; }
        public string? Status { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Date { get; set; }
        public decimal? Amount { get; set; }
        public decimal Premium { get; set; }
        public int? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public long CustomerPhone { get; set; }
    }

    public class CustomerBillViewModel
    {
        public int Id { get; set; }
        public int BillNo { get; set; }
        public int PolicyNo { get; set; }
        public string? Status { get; set; }
        public DateTime? Date { get; set; }
        public decimal? Amount { get; set; }
        public string SelectedStatus { get; set; }
    }
}
