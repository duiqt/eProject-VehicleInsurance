using DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using VehicleInsuranceAPI.Models.Dtos;

namespace VehicleInsuranceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PolicyLapsedController : ControllerBase
    {
        private readonly VipDbContext _db;
        public PolicyLapsedController(VipDbContext db)
        {
            _db = db;
        }

        [HttpGet("GetPolicyLapsed")]
        public async Task<ActionResult<List<PolicyLapsedDto>>> GetPolicyDue()
        {
            var listData = new List<PolicyLapsedDto>();
            listData = (from cer in _db.Certificates
                        join est in _db.Estimates on cer.Id equals est.Id
                        select new PolicyLapsedDto
                        {
                            PolicyNo = cer.PolicyNo,
                            PolicyDate = est.PolicyDate.AddMonths(12).AddDays(-1),
                        }).ToList();

            return listData;
        }
    }
}
