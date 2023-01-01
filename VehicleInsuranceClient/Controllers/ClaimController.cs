using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Policy;
using VehicleInsuranceClient.Models.Dtos;
using Microsoft.AspNetCore.Hosting;
using VehicleInsuranceClient.Models;
using Microsoft.AspNetCore.Routing.Matching;

namespace VehicleInsuranceClient.Controllers
{
    public class ClaimController : Controller
    {
        private string urlClaim = "https://localhost:7008/api/Claim/";
        private string urlAccount = "https://localhost:7008/api/Account/";
        private string urlCustomer = "https://localhost:7008/api/Customer/";
        HttpClient client = new HttpClient();
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;

        public static List<ClaimViewModel> myClaims = new List<ClaimViewModel>();

        public ClaimController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        //[HttpGet]
        //public IActionResult Index()
        //{
        //    var userString = HttpContext.Session.GetString("user");
        //    if (userString == null)
        //    {
        //        return RedirectToAction("Login", "Account");
        //    }
        //    var model = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<ClaimDto>>(client.GetStringAsync(urlClaim).Result);
        //    return View(model);
        //}

        //[HttpGet]
        //public IActionResult ClaimDetailById()
        //{
        //    //lay session ra
        //    var userString = HttpContext.Session.GetString("user");
        //    if (userString == null)
        //    {
        //        return RedirectToAction("Login", "Account");
        //    }
        //    // lay ID ra
        //    var obj = JsonConvert.DeserializeObject<Customer>(userString);
        //    var model = JsonConvert.DeserializeObject<IEnumerable<ClaimDto>>(client.GetStringAsync(urlClaim + "GetClaim/" + obj.Id).Result);
        //    return View(model);
        //}

        [HttpGet]
        public IActionResult ClaimDetailById(int CertId)
        {
            var model = JsonConvert.DeserializeObject<IEnumerable<ClaimDto>>(client.GetStringAsync(urlClaim + "GetClaim/" + CertId).Result);
            return View(model);
        }

        [HttpGet]
        public IActionResult CreateClaim(int policyNo)
        {
            var userString = HttpContext.Session.GetString("user");
            if (userString == null)
            {
                return RedirectToAction("Login", "Account");
            }
            //  var model = JsonConvert.DeserializeObject<IEnumerable<CreateClaimDto>>(client.GetStringAsync(urlClaim).Result);
            ViewBag.policyNo = policyNo;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateClaim(CreateClaimDto createClaimDto)
        {
            var userString = HttpContext.Session.GetString("user");
            if (userString == null)
            {
                return RedirectToAction("Login", "Account");
            }
            // lay ID ra
            var obj = JsonConvert.DeserializeObject<Customer>(userString);

            if (createClaimDto.ImageUpload != null)
            {
                foreach (var item in ModelState)
                {
                    if (item.Key == "ImageUpload")
                    {
                        if (item.Value.ValidationState == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid)
                        {
                            ViewBag.msg = "Only accept extemtion image: .jpg, .png";
                            return RedirectToAction("CreateClaim", "Claim");
                        }
                    }
                }

                string uploadDir = Path.Combine(_hostingEnvironment.WebRootPath, "img/Claim");
                string filePath = Path.Combine(uploadDir, createClaimDto.ImageUpload.FileName);
                FileStream fs = new FileStream(filePath, FileMode.Create);
                await createClaimDto.ImageUpload.CopyToAsync(fs);
                fs.Close();
                createClaimDto.Image = createClaimDto.ImageUpload.FileName;

            }
            createClaimDto.CustomerId = obj.Id;
            var response = client.PostAsJsonAsync(urlClaim + "CreateClaim/", createClaimDto).Result;
            if (response.IsSuccessStatusCode)
            {
                ViewBag.msg = string.Format("Create New Claim Successfully");
                return RedirectToAction("MyClaims", "Claim", new { policyNo = createClaimDto.PolicyNo} );
            }
            ViewBag.msg = string.Format("Your Claim was fail");
            return View();
            //}
            //ViewBag.msg = string.Format("You do not have certificate");
            //return View();
        }



        [HttpGet]
        public IActionResult MyClaims(int policyNo)
        {
            var userString = HttpContext.Session.GetString("user");
            var obj = JsonConvert.DeserializeObject<Customer>(userString);
            var userResult = JsonConvert.DeserializeObject<CustomerDto>
                (client.GetStringAsync(Program.ApiAddress + "/Customer/" + obj.Id).Result);

            ViewBag.userID = userResult.Id;
            ViewBag.policyNo = policyNo;
            return View();
        }

        [HttpPost]
        public IActionResult GetMyClaims([FromBody] List<ClaimViewModel> model)
        {
            myClaims = model;
            return View("ListMyClaims", model);
        }
    }
}
