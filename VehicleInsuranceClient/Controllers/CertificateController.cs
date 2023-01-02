using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using VehicleInsuranceClient.Helper;
using VehicleInsuranceClient.Models;
using VehicleInsuranceClient.Models.Dtos;

namespace VehicleInsuranceClient.Controllers
{
    public class CertificateController : Controller
    {
        public static List<CertificateModel> Certificates = new List<CertificateModel>();
        private readonly IWebHostEnvironment _env;
        HttpClient httpClient = new HttpClient();

        public CertificateController(IWebHostEnvironment env)
        {
            _env = env;
        }

        public IActionResult Index()
        {
            var userString = HttpContext.Session.GetString("user");
            if (userString == null)
            {
                string returnUrl = HttpContext.Request.Path;
                return RedirectToAction("Login", "Account", new { returnUrl = returnUrl });
            }

            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<Customer>(userString);
            var userResult = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerDto>
                (httpClient.GetStringAsync(@Program.ApiAddress + "/Customer/" + obj.Id).Result);
            ViewBag.userID = userResult.Id;
            ViewBag.userName = userResult.CustomerName;

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
                Certificates = InitializeCertificates();
            }

            // Check Login
            var userString = HttpContext.Session.GetString("user");
            if (userString == null)
            {
                string returnUrl = HttpContext.Request.Path;
                return RedirectToAction("Login", "Account", new { returnUrl = returnUrl });
            }

            CertificateModel model = Certificates.Where(c => c.Id == id).FirstOrDefault();

            if (model == null)
            {
                return RedirectToAction("Index");
            }

            return View(model);
        }

        private List<CertificateModel> InitializeCertificates()
        {
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
                var userString = HttpContext.Session.GetString("user");
                if (userString == null)
                {
                    string returnUrl = HttpContext.Request.Path + $"/{estimateNo}";
                    return RedirectToAction("Login", "Account", new { returnUrl = returnUrl });
                }
                CustomerDto customer = JsonSerializer.Deserialize<CustomerDto>(userString);

                if (customer == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                contract = JsonSerializer.Deserialize<ContractModel>(contractCookie.Normalize())!;

                contract.Contract.CustomerName = customer.CustomerName;
                contract.Contract.CustomerPhone = customer.CustomerPhone;
                contract.Contract.CustomerAddress = customer.CustomerAddress;

                ViewBag.PolicyType = EstimateController.Instance.GetPolicies().Where(p => p.PolicyId == contract.Estimation.PolicyId).Select(m => m.PolicyType).FirstOrDefault();
            }
            catch (Exception)
            {
                TempData["EstimateNoErrMessage"] = "Your estimate number is invalid! Sorry for the inconvenience! Please get another!";
                return RedirectToAction("Index", "Estimate");
            }

            return View(contract);
        }

        /// <summary>
        /// Create Certificate after customer submit Contract form
        /// </summary>
        /// <param name="model">Contract</param>
        /// <returns>Certificates of a customer</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Contract(ContractModel model)
        {
            var userString = HttpContext.Session.GetString("user");
            if (userString == null)
            {
                string returnUrl = HttpContext.Request.Path;
                return RedirectToAction("Login", "Account", new { returnUrl = returnUrl });
            }
            // CHECKLOGIN
            if (!ModelState.IsValid)
            {
                return View(model);
            }

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

            // Save the input information of the contract into the same cookie. BUT without IFormFile type
            // Purpose: In case errors occur while creating Certificate, customer will not loss the contract form information they did input.
            IFormFileCollection proveFiles = model.Contract.Prove;
            if (proveFiles == null)
            {
                return View(model);
            }
            model.Contract.Prove = null;
            CookieHelper.CreateCookie(HttpContext, estimateNo.ToString(), model);

            try
            {
                #region Upload prove images
                string pathImages = SaveImages(proveFiles);
                if (String.IsNullOrWhiteSpace(pathImages))
                {
                    return View(model);
                }
                #endregion Upload prove images

                #region Post create Certificate to database
                int policyNo = StoreCertificate(model, pathImages);
                if (policyNo <= 0)
                {
                    TempData["EstimateNoErrMessage"] = "Something wrong with your estimate! Please get another!";
                    return View();
                }
                #endregion Post create Certificate to database
            }
            catch (Exception)
            {
                TempData["EstimateNoErrMessage"] = "Something wrong with your estimate! Please get another!";
                return RedirectToAction("Index", "Estimate");
            }
            // In case create certificate and upload prove images successfully. Remove the cookie.
            CookieHelper.RemoveCookie(HttpContext, estimateNo.ToString());
            return RedirectToAction("Index");
        }
        private string SaveImages(IFormFileCollection files)
        {
            string resultImagesPath = String.Empty;
            if (files == null) return resultImagesPath;
            Regex regex = new Regex(@"[~^$*+`?()>;[\]{}\|/&!@#%]");
            foreach (IFormFile file in files)
            {
                if (file.Length == 0 || file == null)
                {
                    return resultImagesPath;
                }
                if (regex.IsMatch(file.FileName))
                {
                    ViewBag.InvalidNameImage = "Image name cannot contain special letters";
                    return resultImagesPath;
                }
                string checkExtention = Path.GetExtension(file.FileName);
                if (!(checkExtention.Equals(".png") || checkExtention.Equals(".jpg") || checkExtention.Equals(".jpeg")))
                {
                    ViewBag.InvalidNameImage = "Only accept image types of jpg, png and jpeg";
                    return resultImagesPath;
                }
            }
            // Check if directory for prove images didn't exist in Web App. Create new one
            string datePath = DateTime.Now.ToString("yyyMMdd");
            string directory = Path.Combine("img", "proveGallery", datePath);
            string filePath = Path.Combine(_env.WebRootPath, directory);
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            string guid = Guid.NewGuid().ToString("N");
            foreach (IFormFile file in files)
            {
                string extension = String.Empty;
                if (!Path.HasExtension(file.FileName))
                {
                    extension = Path.GetExtension(file.FileName);
                }
                string imgPath = Path.Combine(filePath, guid + "&" + file.FileName + extension);
                using (var fileStream = new FileStream(imgPath, FileMode.Create, FileAccess.Write))
                {
                    file.CopyTo(fileStream);
                }
                resultImagesPath += "&" + file.FileName + extension;
            }

            resultImagesPath = datePath + "/" + guid + resultImagesPath;

            return resultImagesPath;
        }
        private int StoreCertificate(ContractModel model, string pathImages)
        {
            int result;
            int policyNo;
            byte digits = 9;
            // Get from Session
            var userString = HttpContext.Session.GetString("user");
            CustomerDto customer = JsonSerializer.Deserialize<CustomerDto>(userString);
            int customerId = customer.Id;

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
                using var client = new HttpClient();
                StringContent stringContent = new StringContent(JsonSerializer.Serialize(new
                {
                    PolicyNo = policyNo,
                    EstimateNo = model.Estimation.EstimateNo,
                    CustomerId = customerId,
                    VehicleNumber = model.Contract.VehicleNumber,
                    VehicleBodyNumber = model.Contract.VehicleBodyNumber,
                    VehicleEngineNumber = model.Contract.VehicleEngineNumber,
                    VehicleWarranty = "Not Available",
                    Prove = pathImages,
                    Customer = new
                    {
                        Id = customerId,
                        CustomerEmail = "",
                        Password = "",
                        CustomerName = "",
                        CustomerAddress = "",
                        CustomerPhone = 0
                    },
                    EstimateNoNavigation = new
                    {
                        Id = 0,
                        EstimateNo = model.Estimation.EstimateNo,
                        VehicleName = model.Estimation.VehicleName,
                        VehicleModel = model.Estimation.VehicleModel,
                        VehicleVersion = model.Estimation.VehicleVersion,
                        PolicyId = model.Estimation.PolicyId,
                        EstimateDate = model.Estimation.EstimateDate,
                        PolicyDate = model.Estimation.PolicyDate,
                        PolicyDuration = model.Estimation.PolicyDuration,
                        Premium = model.Estimation.Premium,
                        Policy = new
                        {
                            Id = model.Estimation.PolicyId,
                            PolicyType = "",
                            Description = "",
                            Coverage = "",
                            Annually = 0
                        }
                    }
                }), Encoding.UTF8, "application/json"); // End StringContent

                var response = client.PostAsync(Program.ApiAddress + "/Certificate/CreateCertificate", stringContent).Result;
                var data = response.Content.ReadAsStringAsync().Result;
                result = JsonSerializer.Deserialize<int>(data);
                if (result >= 0)
                {
                    CookieHelper.RemoveCookie(HttpContext, model.Estimation.EstimateNo.ToString());
                    return policyNo;
                }
            }
            catch (Exception)
            {

                throw;
            }
            return -1;
        }
    }// End of Controller
}