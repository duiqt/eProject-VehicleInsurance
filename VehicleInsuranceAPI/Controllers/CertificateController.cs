using DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VehicleInsuranceAPI.Models;

namespace VehicleInsuranceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertificateController : ControllerBase
    {
        private readonly VipDbContext _db;
        public CertificateController(VipDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Get certificates of a customer
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCertificates")]
        public IActionResult GetCertificates(int id)
        {
            List<CertificatesModel> model = new List<CertificatesModel>();
            model = (from cer in _db.Certificates
                     join cusbill in _db.CustomerBills on cer.Id equals cusbill.CertificateId
                     join est in _db.Estimates on cer.EstimateId equals est.Id
                     join cus in _db.Customers on est.CustomerId equals cus.Id
                     join pol in _db.Policies on est.PolicyId equals pol.Id
                     where est.CustomerId == id
                     select new CertificatesModel
                     {
                         Id = cer.Id,
                         CustomerId = cus.Id,
                         PolicyType = pol.PolicyType,
                         VehicleName = est.VehicleName,
                         VehicleModel = est.VehicleModel,
                         VehicleVersion = est.VehicleVersion,
                         PolicyDuration = est.PolicyDuration,
                         PolicyDate = est.PolicyDate.AddMonths(12).AddDays(-1),
                         PolicyNo = cer.PolicyNo,
                     }).ToList();
            return Ok(model);
        }
    }
}
