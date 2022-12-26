using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VehicleInsuranceClient.Models.Dtos;

namespace VehicleInsuranceClient.Controllers
{
    public class CustomerController : Controller
    {
        private string urlCustomer = "https://localhost:7008/api/Customer/";
        private string urlAccount = "https://localhost:7008/api/Account/";

        HttpClient client = new HttpClient();
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var userResult = JsonConvert.DeserializeObject<Customer>(client.GetStringAsync(urlCustomer + id).Result);
            return View(userResult); // return redirect ve "Index", "Certificate"
        }

        [HttpPost]
        public IActionResult Edit(Customer customer)
        {
            var model = client.PutAsJsonAsync(urlCustomer, customer).Result;
            if (model.IsSuccessStatusCode)
            {
                TempData["UpdateOK"] = "Update Account Successfully";
                return RedirectToAction("Edit", "Customer");
            }
            return RedirectToAction("Details", "Customer");
        }
    }
}
