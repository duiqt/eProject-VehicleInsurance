using DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace VehicleInsuranceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PolicyController : ControllerBase
    {
        private readonly VipDbContext _db;
        public PolicyController(VipDbContext db)
        {
            _db = db;
        }

        //[HttpGet]
        //[Route("GetPolicies")]
        //public IActionResult GetPolicies()
        //{
        //    using (var _db = new VipDbContext())
        //    {
        //        try
        //        {
        //            return Ok(_db.Policies.Select(p => new
        //            {
        //                PolicyId = p.Id,
        //                PolicyType = p.PolicyType,
        //                Description = p.Description,
        //                Coverage = p.Coverage,
        //                Annually = p.Annually
        //            }).ToList());
        //        }
        //        catch (Exception ex)
        //        {
        //            return BadRequest(ex.Message);
        //        }
        //    }
        //}

        /// <summary>
        /// Get Type of Insurance Policies
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPolicies")]
        public async Task<ActionResult<IEnumerable<Policy>>> GetPolicies()
        {
            return await _db.Policies.ToListAsync();
        }

        //[HttpGet("{id}")]
        //public async Task<ActionResult<Policy>> GetPolicyById(int id)
        //{
        //    var policy = await _db.Policies.FindAsync(id);

        //    if (policy == null)
        //    {
        //        return NotFound();
        //    }

        //    return policy;
        //}
    }
}
