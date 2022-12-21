using Microsoft.AspNetCore.Mvc;

namespace VehicleInsuranceClient.Areas.Employee.Controllers
{
    [Area("Employee")]
    public class CertificateController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
