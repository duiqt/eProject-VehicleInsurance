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
        static List<VehicleViewModel>? Vehicles;
        static List<PoliciesViewModel>? Policies;
        public List<PoliciesViewModel> GetPolicies()
        {
            if (Policies == null)
            {
                Policies = InitializePolicies();
            }
            return Policies;
        }
        public static List<SelectListItem>? vehicleNamesItems;
        public static List<SelectListItem>? vehicleModelsItems;
        public static List<SelectListItem>? vehicleVersionsItems;
        private static readonly EstimateController? _instance;
        public static EstimateController Instance { get { return _instance ?? new EstimateController(); } }
        public EstimateController()
        {
            Vehicles = InitializeVehicles();
            Policies = InitializePolicies();
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (Vehicles == null || Policies == null)
            {
                Vehicles = InitializeVehicles();
                Policies = InitializePolicies();
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
            return View(new EstimateViewModel());
        }

        [HttpPost]
        public IActionResult Index([FromForm] EstimateViewModel model)
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
                                        PolicyId = model.PolicyId,
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
                        }
                        ContractModel contract = new ContractModel { Estimation = model, Contract = new Contract() };
                        contract.Contract.CustomerName = String.Empty;

                        Helper.CookieHelper.CreateCookie(HttpContext, model.EstimateNo.ToString(), contract);
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

        public static List<VehicleViewModel> InitializeVehicles()
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
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Vehicles ?? new List<VehicleViewModel>();
        }
        public static List<PoliciesViewModel> InitializePolicies()
        {
            try
            {
                using (var client = new HttpClient())
                {
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
            return Policies ?? new List<PoliciesViewModel>();
        }

    } // End of EstimateController
}
