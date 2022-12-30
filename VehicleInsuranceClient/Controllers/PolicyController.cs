using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VehicleInsuranceClient.Models;

namespace VehicleInsuranceClient.Controllers
{
    public class PolicyController : Controller
    {
        public static List<PolicyModel> Policies = new List<PolicyModel>();
        public HttpClient client = new HttpClient();

        public IActionResult Index()
        {
            var userString = HttpContext.Session.GetString("user");
            if (userString == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        /// <summary>
        /// Create view for ListPolicies
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetPolicies([FromBody] List<PolicyModel> model)
        {
            Policies = model;
            return View("ListPolicies", model);
        }

        public IActionResult CompleteCareCI()
        {   //Get Policy Data through API Controller and display it to View
            var model = JsonConvert.DeserializeObject<IEnumerable<PolicyModel>>(client.GetStringAsync(Program.ApiAddress + "/Policy/GetPolicies").Result);
            return View(model);
        }

        public IActionResult ComprehensiveCI()
        {   //Get Policy Data through API Controller and display it to View
            var model = JsonConvert.DeserializeObject<IEnumerable<PolicyModel>>(client.GetStringAsync(Program.ApiAddress + "/Policy/GetPolicies").Result);
            return View(model);
        }

        public IActionResult ThirdPartyFireAndTheftCI()
        {   //Get Policy Data through API Controller and display it to View
            var model = JsonConvert.DeserializeObject<IEnumerable<PolicyModel>>(client.GetStringAsync(Program.ApiAddress + "/Policy/GetPolicies").Result);
            return View(model);
        }

        public IActionResult ThirdPartyPropertyDamageCI()
        {
            //Get Policy Data through API Controller and display it to View
            var model = JsonConvert.DeserializeObject<IEnumerable<PolicyModel>>(client.GetStringAsync(Program.ApiAddress + "/Policy/GetPolicies").Result);
            return View(model);
        }
    }
}
