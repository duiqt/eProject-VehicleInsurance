using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            List<CertificateModel> model = new List<CertificateModel>();
            model = (from cer in _db.Certificates
                     join est in _db.Estimates on cer.EstimateNo equals est.EstimateNo
                     join cus in _db.Customers on cer.CustomerId equals cus.Id
                     join pol in _db.Policies on est.PolicyId equals pol.Id
                     where cer.CustomerId == id
                     select new CertificateModel
                     {
                         Id = cer.Id,
                         EstimateNo = est.EstimateNo,
                         CustomerEmail = cus.CustomerEmail,
                         CustomerName = cus.CustomerName,
                         CustomerAddress = cus.CustomerAddress,
                         CustomerPhone = cus.CustomerPhone,
                         PolicyNo = cer.PolicyNo,
                         PolicyType = pol.PolicyType,
                         PolicyDate = est.PolicyDate,
                         PolicyDuration = est.PolicyDuration,
                         VehicleName = est.VehicleName,
                         VehicleModel = est.VehicleModel,
                         VehicleVersion = est.VehicleVersion,
                         VehicleNumber = cer.VehicleNumber,
                         VehicleBodyNumber = cer.VehicleBodyNumber,
                         VehicleEngineNumber = cer.VehicleEngineNumber,
                         VehicleWarranty = cer.VehicleWarranty,
                         Premium = est.Premium,
                         Prove = cer.Prove,
                     }).ToList();
            _db.Dispose();
            return Ok(model);
        }

        /// <summary>
        /// Get all certificates - Admin page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllCertificates")]
        public IActionResult GetAllCertificates()
        {
            List<CertificateModel> model = new List<CertificateModel>();
            model = (from cer in _db.Certificates
                     join est in _db.Estimates on cer.EstimateNo equals est.EstimateNo
                     join cus in _db.Customers on cer.CustomerId equals cus.Id
                     join pol in _db.Policies on est.PolicyId equals pol.Id
                     select new CertificateModel
                     {
                         Id = cer.Id,
                         CustomerId = cus.Id,
                         CustomerName = cus.CustomerName,
                         CustomerAddress = cus.CustomerAddress,
                         CustomerPhone = cus.CustomerPhone,
                         PolicyNo = cer.PolicyNo,
                         PolicyType = pol.PolicyType,
                         PolicyDate = est.PolicyDate,
                         PolicyDuration = est.PolicyDuration,
                         VehicleName = est.VehicleName,
                         VehicleModel = est.VehicleModel,
                         VehicleVersion = est.VehicleVersion,
                         VehicleNumber = cer.VehicleNumber,
                         VehicleBodyNumber = cer.VehicleBodyNumber,
                         VehicleEngineNumber = cer.VehicleEngineNumber,
                         VehicleWarranty = cer.VehicleWarranty,
                         Prove = cer.Prove,
                         Premium = est.Premium
                     }).ToList();
            return Ok(model);
        }

        /// <summary>
        /// Get Certificate Detail Based on Certificate Id - Admin Page
        /// </summary>
        /// <param name="CertId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCertificateDetail/{CertId}")]
        public IActionResult GetCertificateDetail(int CertId)
        {
            CertificateModel model = new CertificateModel();
            model = (from cer in _db.Certificates
                     join est in _db.Estimates on cer.EstimateNo equals est.EstimateNo
                     join cus in _db.Customers on cer.CustomerId equals cus.Id
                     join pol in _db.Policies on est.PolicyId equals pol.Id
                     where cer.Id == CertId
                     select new CertificateModel
                     {
                         Id = cer.Id,
                         CustomerId = cus.Id,
                         CustomerName = cus.CustomerName,
                         CustomerAddress = cus.CustomerAddress,
                         CustomerPhone = cus.CustomerPhone,
                         PolicyNo = cer.PolicyNo,
                         PolicyType = pol.PolicyType,
                         PolicyDate = est.PolicyDate,
                         PolicyDuration = est.PolicyDuration,
                         VehicleName = est.VehicleName,
                         VehicleModel = est.VehicleModel,
                         VehicleVersion = est.VehicleVersion,
                         VehicleNumber = cer.VehicleNumber,
                         VehicleBodyNumber = cer.VehicleBodyNumber,
                         VehicleEngineNumber = cer.VehicleEngineNumber,
                         VehicleWarranty = cer.VehicleWarranty,
                         Prove = cer.Prove,
                         Premium = est.Premium
                     }).FirstOrDefault()!;
            return Ok(model);
        }

        [HttpPost]
        [Route("CreateCertificate")]
        public IActionResult CreateCertificate(Certificate model)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(model.Customer).State = EntityState.Unchanged;
                _db.Entry(model.EstimateNoNavigation.Policy).State = EntityState.Unchanged;
                _db.Certificates.Add(model);
                if (_db.SaveChanges() > 0)
                {
                    return Ok(model.PolicyNo);
                }
                return Ok(-1);
            }
            return BadRequest();
        }

        [HttpPut]
        [Route("UpdateCertificate")]
        public IActionResult UpdateCertificate(UpdateCertModel model)
        {
            var cert = _db.Certificates.Where(p => p.PolicyNo == model.PolicyNo).FirstOrDefault();
            if (cert == null)
            {
                return NotFound();
            }
            var est = _db.Estimates.Where(e => e.EstimateNo == cert.EstimateNo).FirstOrDefault();
            if (est == null)
            {
                return NotFound();
            }
            cert.VehicleNumber = model.VehicleNumber;
            cert.VehicleBodyNumber = model.VehicleBodyNumber;
            cert.VehicleEngineNumber = model.VehicleEngineNumber;
            if (!String.IsNullOrWhiteSpace(model.VehicleWarranty))
            {
                cert.VehicleWarranty = model.VehicleWarranty;
            }
            est.PolicyDate = model.PolicyDate;
            _db.Update(est);
            _db.Update(cert);
            if (_db.SaveChanges() <= 0)
            {
                return Ok("Fail");
            }
            return Ok("Success");
        }

        /// <summary>
        /// Get Claim Detail Based on Certificate Id
        /// </summary>
        /// <param name="policyNo"></param>
        /// <returns></returns>
        [HttpGet("GetClaimDetail/{policyNo}")]
        public IActionResult GetClaimDetail(int policyNo)
        {
            List<ClaimDetailAdminModel> model = new List<ClaimDetailAdminModel>();
            model = (from cla in _db.Claims
                     join cer in _db.Certificates on cla.PolicyNo equals cer.PolicyNo
                     //join cusbill in _db.CustomerBills on cer.PolicyNo equals cusbill.PolicyNo
                     join est in _db.Estimates on cer.EstimateNo equals est.EstimateNo
                     join pol in _db.Policies on est.PolicyId equals pol.Id
                     join cus in _db.Customers on cer.CustomerId equals cus.Id
                     //where est.CustomerId == id
                     select new ClaimDetailAdminModel
                     {
                         Id = cer.Id,
                         CustomerId = cus.Id,
                         CustomerName = cus.CustomerName,
                         CustomerAddress = cus.CustomerAddress,
                         CustomerPhone = cus.CustomerPhone,
                         PolicyNo = cla.PolicyNo,
                         PolicyType = pol.PolicyType,
                         PolicyDate = est.PolicyDate,
                         PolicyDuration = est.PolicyDuration,
                         VehicleName = est.VehicleName,
                         VehicleModel = est.VehicleModel,
                         VehicleVersion = est.VehicleVersion,
                         VehicleNumber = cer.VehicleNumber,
                         VehicleBodyNumber = cer.VehicleBodyNumber,
                         VehicleEngineNumber = cer.VehicleEngineNumber,
                         VehicleWarranty = cer.VehicleWarranty,
                         //Prove = cer.Prove,
                         //Amount = cusbill.Amount,
                         ClaimNo = cla.ClaimNo,
                         PlaceOfAccident = cla.PlaceOfAccident,
                         DateOfAccident = cla.DateOfAccident,
                         InsuredAmount = cla.InsuredAmount,
                         ClaimableAmount = cla.ClaimableAmount,
                         Status = cla.Status,
                     }).ToList();
            return Ok(model.Where(u => u.PolicyNo == policyNo));
        }
    }
}
