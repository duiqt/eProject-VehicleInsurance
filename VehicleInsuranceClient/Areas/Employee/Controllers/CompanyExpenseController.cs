using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VehicleInsuranceClient.Areas.Employee.Models;

namespace VehicleInsuranceClient.Areas.Employee.Controllers
{
    [Area("Employee")]
    public class CompanyExpenseController : Controller
    {
        public HttpClient client = new HttpClient();
        [HttpGet]
        public IActionResult Index()
        {
            var model = JsonConvert.DeserializeObject<IEnumerable<CompanyExpenseAdminModel>>(client.GetStringAsync(Program.ApiAddress + "/CompanyExpense/GetAllCompanyExpense").Result);
            return View(model);
        }

        [HttpGet]
        public IActionResult CompanyExpenseDetail(int claimNo)
        {
            var model = JsonConvert.DeserializeObject<IEnumerable<ClaimDetailCompanyExpenseAdminModel>>(client.GetStringAsync(Program.ApiAddress + "/CompanyExpense/GetCompanyExpenseDetail/" + claimNo).Result);
            return View(model);
        }
    }
}
