
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Text;
using System.Text.Json;
using VehicleInsuranceClient.Models;

namespace VehicleInsuranceClient.Controllers
{
    public class CertificateController : Controller
    {
        public static List<CertificateModel> Certificates = new List<CertificateModel>();

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Create view for ListCertificates
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetCertificates([FromBody] List<CertificateModel> model)
        {
            Certificates = model;
            return View("ListCertificates", model);
        }

        public IActionResult Details(int id)
        {
            if (Certificates == null)
            {
                Certificates = GetCertificates();
            }

            CertificateModel model = Certificates.Where(c => c.Id == id).FirstOrDefault();

            if (model == null)
            {
                return RedirectToAction("Index");
            }

            return View(model);
        }

        private List<CertificateModel> GetCertificates()
        {
            HttpClient httpClient = new HttpClient();
            var response = httpClient.GetAsync(Program.ApiAddress + "/Certificate/GetCertificates").Result;
            var data = response.Content.ReadAsStringAsync().Result;
            if (data != null)
            {
                List<CertificateModel> certificate = JsonSerializer.Deserialize<List<CertificateModel>>(data);
                return certificate;
            }

            return null;
        }

        public IActionResult Print(int id)
        {
            CertificateModel model = Certificates.Where(c => c.Id == id).FirstOrDefault();

            return View(model);
        }

        /// <summary>
        /// Create View for customer fill-in the Contract
        /// </summary>
        /// <returns>Contract View</returns>
        [HttpGet]
        [Route("Certificate/Contract/{estimateNo?}")]
        public IActionResult Contract(string estimateNo)
        {
            if (String.IsNullOrEmpty(estimateNo))
            {
                TempData["EstimateNoErrMessage"] = "Please get your estimate!";
                return RedirectToAction("Index", "Estimate");
            }
            var contractCookie = Request.Cookies[estimateNo];
            if (contractCookie == null)
            {
                TempData["EstimateNoErrMessage"] = "Your estimate number is invalid! Please get another!";
                return RedirectToAction("Index", "Estimate");
            }
            ContractModel contract;
            try
            {
                // Check Login


                // Logged in (CustomerEmail)
                //HttpContext.Session.GetString("user");
                int custumerId = 1;
                CustomerContractModel customer = GetCustomer(custumerId);
                if (customer == null)
                {
                    return RedirectToAction("Login", "Customer");
                }
                contract = JsonSerializer.Deserialize<ContractModel>(contractCookie.Normalize())!;

                contract.Contract.CustomerName = customer.CustomerName;
                contract.Contract.CustomerPhone = customer.CustomerPhone;
                contract.Contract.CustomerAddress = customer.CustomerAddress;

                ViewBag.PolicyType = EstimateController.InitializePolicies().Where(p => p.PolicyId == contract.Estimation.PolicyId).Select(m => m.PolicyType).FirstOrDefault();
            }
            catch (Exception)
            {
                TempData["EstimateNoErrMessage"] = "Your estimate number is invalid! Sorry for the inconvenience! Please get another!";
                return RedirectToAction("Index", "Estimate");
            }

            return View(contract);
        }
        public CustomerContractModel? GetCustomer(int id)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var response = client.GetAsync(Program.ApiAddress + "/Customer/GetCustomer/" + id).Result;
                    var data = response.Content.ReadAsStringAsync().Result;
                    if (data != null)
                    {
                        CustomerContractModel model = JsonSerializer.Deserialize<CustomerContractModel>(data)!;
                        return model;
                    }
                }
                catch (Exception)
                {
                }
            }
            return null;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateCertificate(ContractModel model)
        {
            string estimateNo = model.Estimation.EstimateNo.ToString();
            if (String.IsNullOrEmpty(estimateNo))
            {
                TempData["EstimateNoErrMessage"] = "Please get your estimate!";
                return RedirectToAction("Index", "Estimate");
            }
            var contractCookie = Request.Cookies[estimateNo];
            if (contractCookie == null)
            {
                TempData["EstimateNoErrMessage"] = "Your estimate number is invalid! Please get another!";
                return RedirectToAction("Index", "Estimate");
            }

            ContractModel certificate = new ContractModel();
            certificate = JsonSerializer.Deserialize<ContractModel>(contractCookie.Normalize())!;
            certificate.Contract = model.Contract;
            TimeSpan remain = DateTime.Now.Subtract((DateTime)certificate.Estimation.EstimateDate);
            CookieOptions options = new CookieOptions()
            {
                Expires = DateTime.Now.AddDays(Program.CookieEstimateDuration - remain.TotalDays),
                Secure = true,
                SameSite = SameSiteMode.None
            };
            Response.Cookies.Append(model.Estimation.EstimateNo.ToString(), JsonSerializer.Serialize(certificate), options);

            // Store Estimate to database
            int policyNo = 0;
            try
            {
                policyNo = StoreCertificate(model);
                if (policyNo <= 0)
                {
                    TempData["EstimateNoErrMessage"] = "Something wrong with your estimate! Please get another!";
                    return RedirectToAction("Index", "Estimate");
                }
            }
            catch (Exception)
            {
                TempData["EstimateNoErrMessage"] = "Something wrong with your estimate! Please get another!";
                return RedirectToAction("Index", "Estimate");
            }


            return RedirectToAction("Index", "Payment", new { estimateNo = estimateNo });
        }
        public int StoreCertificate(ContractModel model)
        {
            int result;
            int policyNo;
            byte digits = 9;
            // Get from Session
            int customerId = 1;

            StringBuilder builder = new StringBuilder();
            foreach (char c in Guid.NewGuid().ToString())
            {
                builder.Append((short)c);
                if (builder.Length >= digits)
                {
                    break;
                }
            }
            policyNo = int.Parse(builder.ToString(0, digits));
            try
            {
                using (var client = new HttpClient())
                {
                    StringContent stringContent = new StringContent(JsonSerializer.Serialize(new
                    {
                        PolicyNo = policyNo.ToString(),
                        EstimateNo = model.Estimation.EstimateNo.ToString(),
                        CustomerId = customerId,
                        VehicleNumber = model.Contract.VehicleNumber,
                        VehicleBodyNumber = model.Contract.VehicleBodyNumber,
                        VehicleEngineNumber = model.Contract.VehicleEngineNumber,
                        VehicleWarranty = "Not Available",
                        Prove = "",
                        Customer = new
                        {
                            CustomerEmail = "",
                            Password = "",
                            CustomerName = "",
                            CustomerAddress = "",
                            CustomerPhone = 0
                        },
                        EstimateNoNavigation = new
                        {
                            Id = 0,
                            EstimateNo = model.Estimation.EstimateNo.ToString(),
                            VehicleName = model.Estimation.VehicleName,
                            VehicleModel = model.Estimation.VehicleModel,
                            VehicleVersion = model.Estimation.VehicleVersion,
                            PolicyId = model.Estimation.PolicyId,
                            EstimateDate = model.Estimation.EstimateDate,
                            PolicyDate = model.Estimation.PolicyDate,
                            PolicyDuration = model.Estimation.PolicyDuration,
                            Premium = model.Estimation.Premium
                        }

                    }), Encoding.UTF8, "application/json");
                    var response = client.PostAsync(Program.ApiAddress + "/Certificate/CreateCertificate", stringContent).Result;
                    var data = response.Content.ReadAsStringAsync().Result;
                    result = JsonSerializer.Deserialize<int>(data);
                    if (result >= 0)
                    {
                        return policyNo;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return -1;
        }

    }
}
