using Microsoft.AspNetCore.Mvc;
using VehicleInsuranceClient.Models;

namespace VehicleInsuranceClient.Controllers
{
    public class BillController : Controller
    {
        public static List<CustomerBill> CusBill = new List<CustomerBill>();

        public IActionResult Index(int policyNo)
        {
            var userString = HttpContext.Session.GetString("user");
            if (userString == null)
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                using (var client = new HttpClient())
                {
                    var response = client.GetAsync(Program.ApiAddress + "/Bill").Result;
                    var data = response.Content.ReadAsStringAsync().Result;
                    if (data != null)
                    {
                        CusBill = System.Text.Json.JsonSerializer.Deserialize<List<CustomerBill>>(data)!;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            if (CusBill != null)
            {
                CustomerBill cusBill = CusBill.Where(c => c.PolicyNo == policyNo).FirstOrDefault();

                return View(cusBill);
            }

            return View();
        }

    }
}
