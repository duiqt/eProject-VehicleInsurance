using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.Language.Extensions;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;
using VehicleInsuranceClient.Areas.Employee.Models;
using VehicleInsuranceClient.Models.Contants;

namespace VehicleInsuranceClient.Areas.Employee.Controllers
{
    [Area("Employee")]
    public class ClaimController : Controller
    {
        public ClaimViewModel Claim = new ClaimViewModel();
        //public ClaimModel RelatedClaim = new ClaimModel();

        public HttpClient client = new HttpClient();

        [HttpGet]
        public IActionResult Index()
        {
            var model = JsonConvert.DeserializeObject<IEnumerable<ClaimModel>>(client.GetStringAsync(Program.ApiAddress + "/Claim/GetClaimsAdmin").Result);
            return View(model);
        }

        public ClaimViewModel GetDetailsClaim(int claimNo)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = client.GetAsync(Program.ApiAddress + "/Claim/GetDetail/" + claimNo).Result;
                    var data = response.Content.ReadAsStringAsync().Result;
                    if (data != null)
                    {
                        ClaimViewModel claim = System.Text.Json.JsonSerializer.Deserialize<ClaimViewModel>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        Claim = claim;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpGet("claimNo")]
        public IActionResult Details(int claimNo)
        {
            if (ModelState.IsValid) 
            {
         
                GetDetailsClaim(claimNo);

                ViewBag.Status = new List<SelectListItem>
                {
                    new SelectListItem {Text = "Lodged", Value = "Lodged"},
                    new SelectListItem {Text = "Inspecting", Value = "Inspecting"},
                    new SelectListItem {Text = "Approved", Value = "Approved"},
                    new SelectListItem {Text = "Insufficient", Value = "Insufficient"},
                    new SelectListItem {Text = "Rejected", Value = "Rejected"},
                };
            }
            return View(Claim);
        }


        [HttpPost]
        public IActionResult UpdateClaim(ClaimViewModel model)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var data = client.PostAsJsonAsync<ClaimViewModel>(Program.ApiAddress + "/Claim/UpdateClaim", model).Result;
                    if(data != null)
                    {
                        return RedirectToAction("Index", "Claim");
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return NoContent();
        }
    }
}
