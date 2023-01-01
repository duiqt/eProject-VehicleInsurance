using DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using VehicleInsuranceAPI.Models;

namespace VehicleInsuranceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerBillController : ControllerBase
    {
        private readonly VipDbContext _db;
        public CustomerBillController(VipDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Get all Customer Bill
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllCustomerBill")]
        public IActionResult GetAllCustomerBill()
        {
            List<CustomerBillModel> model = new List<CustomerBillModel>();
            model = (from cusbill in _db.CustomerBills 
                     join est in _db.Estimates on cusbill.Id equals est.Id
                     select new CustomerBillModel
                     {
                         Id = cusbill.Id,
                         BillNo = cusbill.BillNo,
                         Status = cusbill.Status,
                         Date = cusbill.Date,
                         PolicyNo = cusbill.PolicyNo,
                         Amount = est.Premium,
                         //Premium = est.Premium,
                     }).ToList();
            return Ok(model);
        }

        [HttpGet]
        [Route("GetDetail/{BillId}")]
        public async Task<CustomerBillViewModel> GetDetail(int BillId)
        {
            var records = await _db.CustomerBills.Where(c => c.Id == BillId).Select(c => new CustomerBillViewModel()
            {
                Id = c.Id,
                BillNo = c.BillNo,
                Status = c.Status,
                Date = c.Date,
                PolicyNo = c.PolicyNo,
                Amount = c.Amount,
            }).FirstOrDefaultAsync();

            return records;

        }

        [HttpPost]
        [Route("UpdateBill")]
        public async Task UpdateBill(CustomerBillViewModel model)
        {
            var bill = new CustomerBill()
            {
                Id = model.Id,
                BillNo= model.BillNo,
                Status = model.SelectedStatus,
                Date = model.Date,
                PolicyNo = model.PolicyNo,
                Amount = model.Amount,
            };

            _db.CustomerBills.Update(bill);
            await _db.SaveChangesAsync();
        }
    }
}
