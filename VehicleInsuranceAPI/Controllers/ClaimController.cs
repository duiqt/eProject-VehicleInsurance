using DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VehicleInsuranceAPI.Models;

namespace VehicleInsuranceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimController : ControllerBase
    {
        private readonly VipDbContext _db;
        public ClaimController(VipDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        [Route("GetClaims")]
        public IActionResult GetClaims()
        {
            List<ClaimModel> model = new List<ClaimModel>();
            model = (from cer in _db.Certificates
                     join cusbill in _db.CustomerBills on cer.PolicyNo equals cusbill.PolicyNo
                     join est in _db.Estimates on cer.EstimateNo equals est.EstimateNo
                     join cus in _db.Customers on cer.CustomerId equals cus.Id
                     join pol in _db.Policies on est.PolicyId equals pol.Id
                     join cla in _db.Claims on cer.PolicyNo equals cla.PolicyNo
                     select new ClaimModel
                     {
                         Id = cla.Id,
                         ClaimNo = cla.ClaimNo,
                         PolicyNo = cla.PolicyNo,
                         PlaceOfAccident = cla.PlaceOfAccident,
                         DateOfAccident = cla.DateOfAccident,
                         Status = cla.Status,
                         Description = cla.Description,
                         InsuredAmount = cla.InsuredAmount,
                         ClaimableAmount = cla.ClaimableAmount,
                     }).ToList();
            return Ok(model);
        }

        [HttpGet]
        [Route("GetDetail/{policyNo}")]
        public IActionResult GetDetail(int policyNo)
        {
            List<ClaimModel> model = new List<ClaimModel>();
            model = (from cer in _db.Certificates
                     join cusbill in _db.CustomerBills on cer.PolicyNo equals cusbill.PolicyNo
                     join est in _db.Estimates on cer.EstimateNo equals est.EstimateNo
                     join cus in _db.Customers on cer.CustomerId equals cus.Id
                     join pol in _db.Policies on est.PolicyId equals pol.Id
                     join cla in _db.Claims on cer.PolicyNo equals cla.PolicyNo
                     where cla.PolicyNo == policyNo
                     select new ClaimModel
                     {
                         Id = cla.Id,
                         ClaimNo = cla.ClaimNo,
                         PolicyNo = cla.PolicyNo,
                         PlaceOfAccident = cla.PlaceOfAccident,
                         DateOfAccident = cla.DateOfAccident,
                         Status = cla.Status,
                         Description = cla.Description,
                         Image = cla.Image,
                         InsuredAmount = cla.InsuredAmount,
                         ClaimableAmount = cla.ClaimableAmount,
                         CustomerName = cus.CustomerName,
                         CustomerAddress = cus.CustomerAddress,
                         CustomerPhone = cus.CustomerPhone,
                         PolicyType = pol.PolicyType,
                         VehicleName = est.VehicleName,
                         VehicleModel = est.VehicleModel,
                         VehicleVersion = est.VehicleVersion,
                         VehicleNumber = cer.VehicleNumber,
                         VehicleBodyNumber = cer.VehicleBodyNumber,
                         VehicleEngineNumber = cer.VehicleEngineNumber,
                         VehicleWarranty = cer.VehicleWarranty,
                     }).ToList();
            return Ok(model);
        }

    }
}
