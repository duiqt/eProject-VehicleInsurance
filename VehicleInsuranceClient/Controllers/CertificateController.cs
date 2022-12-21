
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using VehicleInsuranceClient.Models;

namespace VehicleInsuranceClient.Controllers
{
    public class CertificateController : Controller
    {
        public static List<CertificatesModel> Certificates = new List<CertificatesModel>();

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult DetailsCertificate()
        {
            return View();
        }

        /// <summary>
        /// Create view for ListCertificates
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetCertificates([FromBody] List<CertificatesModel> model)
        {
            Certificates = model;
            return View("ListCertificates", model);
        }

    }
}
