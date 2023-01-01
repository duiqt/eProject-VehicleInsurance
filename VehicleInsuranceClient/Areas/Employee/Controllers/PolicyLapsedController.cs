using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Policy;
using VehicleInsuranceClient.Areas.Employee.Models;
using VehicleInsuranceClient.Areas.Employee.Models.Dtos;
using VehicleInsuranceClient.Models;

namespace VehicleInsuranceClient.Areas.Employee.Controllers
{
    [Area("Employee")]
    public class PolicyLapsedController : Controller
    {
        private string urlDashboard = "https://localhost:7008/api/PolicyLapsed/";
        private HttpClient client = new HttpClient();
        public IActionResult Index()
        {
            return View();
        }
        public List<PolicyLapsedDto> GetDashboard()
        {
            var res = JsonConvert.DeserializeObject<List<PolicyLapsedDto>>(client.GetStringAsync(urlDashboard + "GetPolicyLapsed").Result);
            ViewBag.ChartData = res;
            return res;
        }
    }
}
