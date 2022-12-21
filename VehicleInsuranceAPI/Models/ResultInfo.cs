namespace VehicleInsuranceAPI.Models
{
    public class ResultInfo
    {
        public string Token { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}
