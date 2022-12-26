using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Contracts;
using System.Reflection;
using System.Text.Json;
using VehicleInsuranceClient.Models;
namespace VehicleInsuranceClient.Controllers
{
    public class PaymentController : Controller
    {
        public IActionResult Index(string estimateNo)
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
            ContractModel certificate = JsonSerializer.Deserialize<ContractModel>(contractCookie.Normalize())!;
            ViewBag.PolicyType = EstimateController.Instance.GetPolicies().Where(p => p.PolicyId == certificate.Estimation.PolicyId).Select(p => p.PolicyType).FirstOrDefault();
            return View(certificate);
        }
    }
}
