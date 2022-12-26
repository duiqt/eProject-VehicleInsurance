using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VehicleInsuranceClient.Areas.Employee.Models;

namespace VehicleInsuranceClient.Areas.Employee.Controllers
{
    [Area("Employee")]
    public class CertificateController : Controller
    {
        public HttpClient client = new HttpClient();
        [HttpGet]
        public IActionResult Index()
        {
            var model = JsonConvert.DeserializeObject<IEnumerable<CertificateAdminModel>>(client.GetStringAsync(Program.ApiAddress + "/Certificate/GetAllCertificates").Result);
            return View(model);
        }

        [HttpGet]
        public IActionResult CertificateDetail(int CertId)
        {
            var model = JsonConvert.DeserializeObject<IEnumerable<CertificateAdminModel>>(client.GetStringAsync(Program.ApiAddress + "/Certificate/GetCertificateDetail/" + CertId).Result);
            return View(model);
        }
    }
}
