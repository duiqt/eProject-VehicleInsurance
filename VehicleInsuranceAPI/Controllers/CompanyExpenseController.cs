using DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.ConstrainedExecution;
using VehicleInsuranceAPI.Models;

namespace VehicleInsuranceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyExpenseController : ControllerBase
    {
        private readonly VipDbContext _db;
        public CompanyExpenseController(VipDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Get all Company Expense
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllCompanyExpense")]
        public IActionResult GetCompanyExpense()
        {
            List<CompanyExpenseMonthlyModel> model = new List<CompanyExpenseMonthlyModel>();
            model = ( from cla in _db.Claims
                      join cer in _db.Certificates on cla.PolicyNo equals cer.PolicyNo
                      join est in _db.Estimates on cer.EstimateNo equals est.EstimateNo
                      join pol in _db.Policies on est.PolicyId equals pol.Id
                      //from cer in _db.Certificates
                      //     join cusbill in _db.CustomerBills on cer.Id equals cusbill.Id
                      //     join est in _db.Estimates on cer.Id equals est.Id
                      //     join pol in _db.Policies on est.PolicyId equals pol.Id
                      //     join cus in _db.Customers on cer.CustomerId equals cus.Id
                      //     join ver in _db.Vehicles on cer.Id equals ver.Id
                      //     join cla in _db.Claims on cer.PolicyNo equals cla.PolicyNo
                      //     join ce in _db.CompanyExpenses on cer.Id equals ce.Id
                      //where est.CustomerId == id
                      select new CompanyExpenseMonthlyModel
                      {
                          Id = cer.Id,
                          ClaimNo = cla.ClaimNo,
                          DateOfExpense = cla.DateOfAccident,
                          TypeOfExpense = pol.PolicyType,
                          AmountOfExpense = cla.ClaimableAmount,
                          PolicyType = pol.PolicyType,
                          DateOfAccident = cla.DateOfAccident,
                          ClaimableAmount = cla.ClaimableAmount,
                          Status = cla.Status
                      }).ToList();
            return Ok(model.Where(u => u.Status == "Approved"));
        }

        /// <summary>
        /// Get Company Expense Detail by using claimNo to find Claim that are approved
        /// </summary>
        /// <param name="CEId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCompanyExpenseDetail/{claimNo}")]
        public IActionResult GetClaimDetail(int claimNo)
        {
            List<ClaimDetailCompanyExpenseAdminModel> model = new List<ClaimDetailCompanyExpenseAdminModel>();
            model = (
                from cla in _db.Claims
                    join cer in _db.Certificates on cla.PolicyNo equals cer.PolicyNo
                    join cus in _db.Customers on cer.CustomerId equals cus.Id
                    join est in _db.Estimates on cer.EstimateNo equals est.EstimateNo
                    join pol in _db.Policies on est.PolicyId equals pol.Id

                //from cer in _db.Certificates
                //     join cusbill in _db.CustomerBills on cer.Id equals cusbill.Id
                //     join est in _db.Estimates on cer.Id equals est.Id
                //     join pol in _db.Policies on est.PolicyId equals pol.Id
                //     join cus in _db.Customers on cer.CustomerId equals cus.Id
                //     join ver in _db.Vehicles on cer.Id equals ver.Id
                //     join cla in _db.Claims on cer.PolicyNo equals cla.PolicyNo
                //     join ce in _db.CompanyExpenses on cer.Id equals ce.Id
                     //where est.CustomerId == id
                     select new ClaimDetailCompanyExpenseAdminModel
                     {
                         Id = cer.Id,
                         DateOfExpense = cla.DateOfAccident,
                         TypeOfExpense = pol.PolicyType,
                         AmountOfExpense = cla.ClaimableAmount,
                         CustomerId = cus.Id,
                         CustomerName = cus.CustomerName,
                         CustomerAddress = cus.CustomerAddress,
                         CustomerPhone = cus.CustomerPhone,
                         PolicyNo = cer.PolicyNo,
                         PolicyType = pol.PolicyType,
                         PolicyDate = est.PolicyDate,
                         PolicyDuration = est.PolicyDuration,
                         ExpiryDate = est.PolicyDate.AddMonths(12).AddDays(-1),
                         VehicleName = est.VehicleName,
                         VehicleModel = est.VehicleModel,
                         VehicleVersion = est.VehicleVersion,
                         VehicleNumber = cer.VehicleNumber,
                         VehicleBodyNumber = cer.VehicleBodyNumber,
                         VehicleEngineNumber = cer.VehicleEngineNumber,
                         VehicleWarranty = cer.VehicleWarranty,
                         Image = cla.Image,
                         Prove = cer.Prove,
                         Amount = est.Premium,
                         ClaimNo = cla.ClaimNo,
                         PlaceOfAccident = cla.PlaceOfAccident,
                         DateOfAccident = cla.DateOfAccident,
                         InsuredAmount = cla.InsuredAmount,
                         ClaimableAmount = cla.ClaimableAmount,
                         Status = cla.Status
                     }).ToList();
            return Ok(model.Where(u => u.ClaimNo == claimNo));
        }
    }
}
