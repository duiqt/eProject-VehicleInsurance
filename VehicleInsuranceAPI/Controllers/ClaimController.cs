using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VehicleInsuranceAPI.IResponsitory;
using VehicleInsuranceAPI.Models.Dtos;
using VehicleInsuranceAPI.Models;

namespace VehicleInsuranceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimController : ControllerBase
    {
        private readonly VipDbContext _db;
        private readonly ICustomer service;

        public ClaimController(VipDbContext db, ICustomer _service)
        {
            _db = db;
            service = _service;
        }

        // GET: api/Claim
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Claim>>> GetClaims()
        {
            return await _db.Claims.ToListAsync();
        }


        // GET: api/Claim/5
        [HttpGet]
        [Route("GetClaim")]
        public IActionResult GetClaim(int id)
        {
            List<ClaimDto> model = new List<ClaimDto>();
            model = (from cla in _db.Claims
                     join cer in _db.Certificates on cla.PolicyNo equals cer.PolicyNo
                     join cusbill in _db.CustomerBills on cla.Status equals cusbill.Status
                     select new ClaimDto
                     {
                         Id = cla.Id,
                         ClaimNo = cla.Id,
                         PlaceOfAccident = cla.PlaceOfAccident,
                         DateOfAccident = cla.DateOfAccident,
                         Description = cla.Description,
                         Status = cusbill.Status,
                         Image = cla.Image,
                         InsuredAmount = cla.InsuredAmount,
                         ClaimableAmount = cla.ClaimableAmount,

                     }).ToList();
            return Ok(model.Where(u => u.Id == id));
        }

        // PUT: api/Claim/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("CreateClaim")]
        public async Task<IActionResult> CreateClaim(CreateClaimDto createClaimDto)
        {
            var result = await service.CreateClaimCustomer(createClaimDto);

            if (result == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Create New Claim Unsuccessfully");
            }
            return Ok("Create New Claim Unsuccessfully");
        }

        /// <summary>
        /// Get Claims - Admin page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetClaimsAdmin")]
        public IActionResult GetClaimsAdmin()
        {
            List<ClaimModel> model = new List<ClaimModel>();
            model = (from cla in _db.Claims
                     join cer in _db.Certificates on cla.PolicyNo equals cer.PolicyNo
                     join cus in _db.Customers on cer.CustomerId equals cus.Id
                     join est in _db.Estimates on cer.EstimateNo equals est.EstimateNo
                     join pol in _db.Policies on est.PolicyId equals pol.Id
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
                     }).ToList();
            return Ok(model);
        }

        /// <summary>
        /// Claim's details - Admin page
        /// </summary>
        /// <param name="claimNo"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetDetail/{claimNo}")]
        public async Task<ClaimViewModel> GetDetail(int claimNo)
        {
            var records = await _db.Claims.Where(c => c.ClaimNo == claimNo).Select(c => new ClaimViewModel()
            {
               Id = c.Id,
               ClaimNo = c.ClaimNo,
               PolicyNo = c.PolicyNo,
               PlaceOfAccident = c.PlaceOfAccident,
               DateOfAccident = c.DateOfAccident,
               Description = c.Description,
               Status = c.Status,
               Image = c.Image,
               InsuredAmount = c.InsuredAmount,
               ClaimableAmount = c.ClaimableAmount
            }).FirstOrDefaultAsync();

            return records;

        }


        /// <summary>
        /// My Claims - Customer page
        /// </summary>
        /// <param name="claimNo"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("MyClaims")]
        public IActionResult MyClaims(int id, int policyNo)
        {
            List<ClaimViewModel> model = new List<ClaimViewModel>();

            model = (from cla in _db.Claims
                     join cer in _db.Certificates on cla.PolicyNo equals cer.PolicyNo
                     join cus in _db.Customers on cer.CustomerId equals cus.Id
                     where cus.Id == id && cer.PolicyNo == policyNo
                     select new ClaimViewModel
                     {
                         ClaimNo = cla.ClaimNo,
                         PolicyNo = cla.PolicyNo,
                         PlaceOfAccident = cla.PlaceOfAccident,
                         DateOfAccident = cla.DateOfAccident,
                         Status = cla.Status,
                         Description = cla.Description,
                         Image = cla.Image,
                         InsuredAmount = cla.InsuredAmount,
                         ClaimableAmount = cla.ClaimableAmount,
                     }).ToList();
            return Ok(model);

        }


        /// <summary>
        /// Update claim - Admin page
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateClaim")]
        public async Task<IActionResult> UpdateClaim(ClaimViewModel model)
        {
            //var claim = _db.Claims.FirstOrDefaultAsync(c => c.ClaimNo == model.ClaimNo);

            var claim = new Claim()
            {
                Id = model.Id,
                ClaimNo = model.ClaimNo,
                PolicyNo = model.PolicyNo,
                PlaceOfAccident = model.PlaceOfAccident,
                DateOfAccident = model.DateOfAccident,
                Description = model.Description,
                Status = model.SelectedStatus,
                Image = model.Image,
                InsuredAmount = model.InsuredAmount,
                ClaimableAmount = model.ClaimableAmount,
            };

            _db.Claims.Update(claim);
            await _db.SaveChangesAsync();
            return Ok("Update Status Successfully");
        }

    }
}
