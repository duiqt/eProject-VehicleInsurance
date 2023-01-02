using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using VehicleInsuranceAPI.Models;

namespace VehicleInsuranceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillController : ControllerBase
    {
        private readonly VipDbContext _db;

        public BillController(VipDbContext db)
        {
            _db = db;
        }

        // GET: api/Bill
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerBill>>> GetCustomerBills()
        {
            return await _db.CustomerBills.ToListAsync();
        }

        // GET: api/Bill/5
        [HttpGet("{policyNo}")]
        public IActionResult Bill(int policyNo)
        {
            var customerBill = _db.CustomerBills.Where(b => b.PolicyNo == policyNo && b.Status.Equals("Completed")).FirstOrDefault();

            if (customerBill == null)
            {
                return Ok(-1);
            }

            return Ok(customerBill.BillNo);
        }

        // PUT: api/Bill/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomerBill(int id, CustomerBill customerBill)
        {
            if (id != customerBill.Id)
            {
                return BadRequest();
            }

            _db.Entry(customerBill).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerBillExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Bill
        [HttpPost]
        [Route("PostCustomerBill")]
        public IActionResult PostCustomerBill(CustomerBillModel model)
        {
            try
            {
                Certificate certificate = _db.Certificates.Where(c => c.PolicyNo == model.PolicyNo).FirstOrDefault();
                if (certificate == null)
                {
                    return NoContent();
                }
            ;
                //if (_db.CustomerBills.FirstOrDefault(b => b.BillNo == model.BillNo) != null)
                //{
                //    return Ok(-1);
                //}
                certificate.VehicleWarranty = "Pending";
                CustomerBill bill = new CustomerBill()
                {
                    BillNo = model.BillNo,
                    PolicyNo = model.PolicyNo,
                    Status = model.Status,
                    Date = model.Date,
                    Amount = model.Amount,
                    PolicyNoNavigation = certificate
                };
                _db.CustomerBills.Add(bill);
                _db.Entry(bill.PolicyNoNavigation).State = EntityState.Modified;
                if (_db.SaveChanges() > 0)
                {
                    _db.Dispose();
                    return Ok(model.BillNo);
                }
                return Ok(-1);
            }
            catch (Exception)
            {
                return BadRequest();
            }
            finally
            {
                _db.Dispose();
            }

        }

        // DELETE: api/Bill/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomerBill(int id)
        {
            var customerBill = await _db.CustomerBills.FindAsync(id);
            if (customerBill == null)
            {
                return NotFound();
            }

            _db.CustomerBills.Remove(customerBill);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerBillExists(int id)
        {
            return _db.CustomerBills.Any(e => e.Id == id);
        }
    }
}
