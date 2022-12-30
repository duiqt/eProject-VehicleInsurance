using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using VehicleInsuranceAPI.Models;
namespace VehicleInsuranceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstimateController : ControllerBase
    {
        private readonly VipDbContext _db;
        public EstimateController(VipDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Estimate car insurance premium
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Estimate(EstimateModel model)
        {

            return Ok(GetEstimate(model));
        }

        /// <summary>
        /// Calculate Estimate car insurance premium
        /// Simple logic to calculate depending on PolicyTypes and Vehicle price only
        /// </summary>
        /// <param name="model">Estimate Model</param>
        /// <returns>estimated premium typeof decimal</returns>
        decimal GetEstimate(EstimateModel model)
        {
            decimal premium = 0;
            try
            {
                Policy policy = _db.Policies.Where(p => p.Id.Equals(model.PolicyId)).FirstOrDefault()!;
                if (policy != null)
                {
                    premium += policy.Annually;
                }
                premium += GetRateOnPrice(model.VehicleRate) * model.VehicleRate;
                //premium += GetRateOnVersion(model.VehicleVersion) * model.VehicleRate;
            }
            catch (ArgumentNullException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
            return premium;
        }
        /// <summary>
        /// Calculate the increased rate depending on vehicleRate (Price)
        /// </summary>
        /// <param name="vehicleRate"></param>
        /// <returns>Increased rate</returns>
        decimal GetRateOnPrice(decimal vehicleRate)
        {

            if (vehicleRate > 110000)
            {
                return 0.018m;
            }
            else if (vehicleRate > 90000)
            {
                return 0.016m;
            }
            else if (vehicleRate > 60000)
            {
                return 0.015m;
            }
            else if (vehicleRate > 50000)
            {
                return 0.014m;
            }
            else if (vehicleRate > 40000)
            {
                return 0.012m;
            }
            else if (vehicleRate > 30000)
            {
                return 0.01m;
            }
            return 0;
        }

        /// <summary>
        /// Get vehicles list for estimate view
        /// </summary>
        /// <returns>List of vehicles</returns>
        [HttpGet]
        [Route("GetVehicles")]
        public IActionResult GetVehicles()
        {
            //List<Vehicle> vehicles = new List<Vehicle>();
            var data = _db.Vehicles.Select(v => new
            {
                VehicleName = v.VehicleName,
                VehicleModel = v.VehicleModel,
                VehicleVersion = v.VehicleVersion
            }).ToList();

            return Ok(data);
        }
        /// <summary>
        /// Get policies list for estimate view
        /// </summary>
        /// <returns>List of policies</returns>
        [HttpGet]
        [Route("GetPolicies")]
        public IActionResult GetPolicies()
        {
            using (var _db = new VipDbContext())
            {
                try
                {
                    return Ok(_db.Policies.Select(p => new
                    {
                        PolicyId = p.Id,
                        PolicyType = p.PolicyType,
                        Description = p.Description,
                        Coverage = p.Coverage,
                        Annually = p.Annually
                    }).ToList());
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpPost]
        [Route("CreateVehicle")]
        public IActionResult CreateVehicle(Vehicle model)
        {
            _db.Vehicles.Add(model);
            if (_db.SaveChanges() > 0)
            {
                return Ok(model);
            }
            return BadRequest();
        }
    }
}
