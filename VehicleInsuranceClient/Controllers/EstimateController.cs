using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;
using System.Text;
using System.Text.Json;
using VehicleInsuranceClient.Models;

namespace VehicleInsuranceClient.Controllers
{
    public class EstimateController : Controller
    {
        public static List<VehicleViewModel>? Vehicles;
        public static List<PoliciesViewModel>? Policies;
        public static List<SelectListItem> vehicleNamesItems;
        public static List<SelectListItem> vehicleModelsItems;
        public static List<SelectListItem> vehicleVersionsItems;
        public IActionResult Index()
        {
            if (Vehicles == null || Policies == null)
            {
                InitializeEstimateView();
            }
            //ViewBag.Vehicles = Vehicles;
            //ViewBag.Policies = Policies;

            vehicleNamesItems = Vehicles.Select(v => new SelectListItem
            {
                Value = v.VehicleName,
                Text = v.VehicleName
            }).ToList();

            vehicleModelsItems = Vehicles.Select(v => new SelectListItem
            {
                Value = v.VehicleModel,
                Text = v.VehicleName + "_" + v.VehicleModel
            }).ToList();
            vehicleVersionsItems = Vehicles.Select(v => new SelectListItem
            {
                Value = v.VehicleVersion,
                Text = v.VehicleModel + "_" + v.VehicleVersion
            }).ToList();

            ViewBag.VehicleNames = vehicleNamesItems;
            ViewBag.VehicleModels = vehicleModelsItems;
            ViewBag.VehicleVersions = vehicleVersionsItems;
            ViewBag.Policies = Policies;
            return View(new EstimateClientViewModel());
        }

        [HttpPost]
        public IActionResult Estimate([FromForm] EstimateClientViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Calling API to estimate the premium, then save this Estimate model as cookie
                using (var client = new HttpClient())
                {
                    try
                    {
                        StringContent stringContent = new StringContent(JsonSerializer.
                                    Serialize(new
                                    {
                                        PolicyType = model.PolicyType,
                                        VehicleName = model.VehicleName,
                                        VehicleModel = model.VehicleModel,
                                        VehicleVersion = model.VehicleVersion,
                                        VehicleRate = model.VehicleRate
                                    }), Encoding.UTF8, "application/json");
                        var response = client.PostAsync(Program.ApiAddress + "/Estimate", stringContent).Result;
                        decimal Premium = JsonSerializer.Deserialize<decimal>(response.Content.ReadAsStringAsync().Result);
                        model.Premium = Math.Round(Premium, 2);

                        var estimateCookie = Request.Cookies[model.EstimateNo.ToString()!];
                        if (estimateCookie == null)
                        {
                            StringBuilder builder = new StringBuilder();
                            // Generate Estimate unique number 9 digits
                            byte digits = 9;
                            foreach (char c in Guid.NewGuid().ToString())
                            {
                                builder.Append((short)c);
                                if (builder.Length >= digits)
                                {
                                    break;
                                }
                            }
                            model.EstimateNo = int.Parse(builder.ToString(0, digits));
                            CreateCookie(model.EstimateNo.ToString(), JsonSerializer.Serialize(model));
                        }
                        else
                        {
                            CreateCookie(model.EstimateNo.ToString(), JsonSerializer.Serialize(model));
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
                ViewBag.VehicleNames = vehicleNamesItems;
                ViewBag.VehicleModels = vehicleModelsItems;
                ViewBag.VehicleVersions = vehicleVersionsItems;
                ViewBag.Policies = Policies;
                return View("Index", model);
            }
            else
            {
                return View("Index", model);
            }
        }

        /// <summary>
        /// Retrieve all policy types from Db by calling API and display in Estimate View
        /// </summary>
        /// <returns>Policy types</returns>
        public IActionResult GetEstimatePolicies()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var response = client.GetAsync(Program.ApiAddress + "/Estimate/GetPolicies").Result;
                    var data = response.Content.ReadAsStringAsync().Result;
                    if (data != null)
                    {
                        List<PoliciesViewModel> policies = JsonSerializer.Deserialize<List<PoliciesViewModel>>(data)!;
                        return View("Policies", policies);
                    }
                }
                catch (Exception)
                {

                }
            }
            return View("Index", null);
        }
        public static void InitializeEstimateView()
        {
            try
            {
                //Using HttpClient
                using (var client = new HttpClient())
                {
                    var responseVehicles = client.GetAsync(Program.ApiAddress + "/Estimate/GetVehicles").Result;
                    var dataVehicles = responseVehicles.Content.ReadAsStringAsync().Result;
                    if (dataVehicles != null)
                    {
                        Vehicles = JsonSerializer.Deserialize<List<VehicleViewModel>>(dataVehicles)!;
                    }

                    var responsePolicies = client.GetAsync(Program.ApiAddress + "/Estimate/GetPolicies").Result;
                    var dataPolicies = responsePolicies.Content.ReadAsStringAsync().Result;
                    if (dataPolicies != null)
                    {
                        Policies = JsonSerializer.Deserialize<List<PoliciesViewModel>>(dataPolicies)!;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// This method is to create Cookie based on key and value 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void CreateCookie(string key, string value)
        {
            CookieOptions options = new CookieOptions()
            {
                //Expires = DateTime.Now.AddMinutes(5)
                Expires = DateTime.Now.AddDays(Program.CookieEstimateDuration)
            };
            Response.Cookies.Append(key, value, options);
        }

        /// <summary>
        /// This method is to remove Cookie based on key
        /// </summary>
        /// <param name="key"></param>
        public void RemoveCookie(string key)
        {
            CookieOptions options = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(-1)
            };
            Response.Cookies.Append(key, String.Empty, options);
        }
    } // End of EstimateController
}
