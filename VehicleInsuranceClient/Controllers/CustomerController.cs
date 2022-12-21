using Microsoft.AspNetCore.Mvc;

namespace VehicleInsuranceClient.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
