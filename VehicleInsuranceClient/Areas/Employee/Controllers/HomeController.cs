using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VehicleInsuranceClient.Areas.Employee.Models.Dtos;

namespace VehicleInsuranceClient.Areas.Employee.Controllers
{
    [Area("Employee")]
    public class HomeController : Controller
    {
        private string urlDashboard = "https://localhost:7008/api/Dashboard/";
        private HttpClient client = new HttpClient();

        public IActionResult Index()
        {
            var userString = HttpContext.Session.GetString("admin");
            if (userString == null)
            {
                return RedirectToAction("LoginAdmin", "Account");
            }
            var model = JsonConvert.DeserializeObject<DashboardOverviewDto>(client.GetStringAsync(urlDashboard + "GetReport/").Result);
            return View(model);
        }

        public List<DashboardDto> GetDashboard()
        {
            var res = JsonConvert.DeserializeObject<List<DashboardDto>>(client.GetStringAsync(urlDashboard + "GetClaim/").Result);
            ViewBag.ChartData = res;
            return res;
        }

        public IActionResult GetVehicleReport()
        {
            var model = JsonConvert.DeserializeObject<List<CertificateDto>>(client.GetStringAsync(urlDashboard + "GetVehicleReport/").Result);
            ViewBag.TotalPercenYearA = model.Sum(a => a.VehiclePercenYearA) > 0 ? 100 : 0;
            ViewBag.TotalPercenYearB = model.Sum(a => a.VehiclePercenYearB) > 0 ? 100 : 0;
            ViewBag.TotalPercenYearC = model.Sum(a => a.VehiclePercenYearC) > 0 ? 100 : 0;

            return View(model);
        }
    }
}
