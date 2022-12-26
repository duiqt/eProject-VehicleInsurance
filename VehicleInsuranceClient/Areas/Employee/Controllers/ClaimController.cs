using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language.Extensions;
using Newtonsoft.Json;
using System.Text.Json;
using VehicleInsuranceClient.Areas.Employee.Models;
using VehicleInsuranceClient.Models;

namespace VehicleInsuranceClient.Areas.Employee.Controllers
{
    [Area("Employee")]
    public class ClaimController : Controller
    {
        public HttpClient client = new HttpClient();

        [HttpGet]
        public IActionResult Index()
        {
            var model = JsonConvert.DeserializeObject<IEnumerable<ClaimModel>>(client.GetStringAsync(Program.ApiAddress + "/Claim/GetClaims").Result);
            return View(model);
        }

        [HttpGet]
        public IActionResult Details(int policyNo)
        {
            var model = JsonConvert.DeserializeObject<IEnumerable<ClaimModel>>(client.GetStringAsync(Program.ApiAddress + "/Claim/GetDetail/" + policyNo).Result);
            return View(model);
        }

    }
}
