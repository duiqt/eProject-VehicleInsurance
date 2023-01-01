using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Reflection;
using System.Security.Policy;
using System.Text.Json;
using VehicleInsuranceClient.Areas.Employee.Models;
using VehicleInsuranceClient.Areas.Employee.Models.Dtos;

namespace VehicleInsuranceClient.Areas.Employee.Controllers
{
    [Area("Employee")]
    public class CustomerBillController : Controller
    {
        public CustomerBillViewModel Bill = new CustomerBillViewModel();
        public HttpClient client = new HttpClient();

        [HttpGet]
        public IActionResult CustomerBill()
        {
            var model = JsonConvert.DeserializeObject<IEnumerable<CustomerBillAdminModel>>(client.GetStringAsync(Program.ApiAddress + "/CustomerBill/GetAllCustomerBill").Result);
            return View(model);
        }

        public CustomerBillViewModel GetDetailsBill(int BillId)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = client.GetAsync(Program.ApiAddress + "/CustomerBill/GetDetail/" + BillId).Result;
                    var data = response.Content.ReadAsStringAsync().Result;
                    if (data != null)
                    {
                        CustomerBillViewModel cusbill = System.Text.Json.JsonSerializer.Deserialize<CustomerBillViewModel>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        Bill = cusbill;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpGet]
        public IActionResult CustomerBillUpdate(int BillId)
        {
            if (ModelState.IsValid)
            {
                GetDetailsBill(BillId);

                ViewBag.Status = new List<SelectListItem>
                {
                    new SelectListItem {Text = "Pending", Value = "Pending"},
                    new SelectListItem {Text = "Completed", Value = "Completed"},
                };
            }
            return View(Bill);
        }

        [HttpPost]
        public IActionResult CustomerBillUpdate(CustomerBillViewModel bill)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var data = client.PostAsJsonAsync<CustomerBillViewModel>(Program.ApiAddress + "/CustomerBill/UpdateBill", bill).Result;
                }
                catch (Exception)
                {

                }
            }
            return RedirectToAction("CustomerBill");
        }
    }
}
