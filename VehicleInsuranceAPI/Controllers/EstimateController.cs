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
        /// Store the estimate to database if customer pay the policy
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Estimate number</returns>
        [HttpPost]
        [Route("StoreEstimate")]
        public IActionResult StoreEstimate(Estimate model)
        {
            if (ModelState.IsValid)
            {
                var policy = _db.Policies.Where(p => p.Id == model.PolicyId).FirstOrDefault();
                _db.Attach(policy);
                policy.Estimates.Add(model);
                //_db.Estimates.Add(model);
                if (_db.SaveChanges() > 0)
                {
                    return Ok(model.EstimateNo);
                }
                //resultInfo = new ResultInfo();
                //resultInfo.Status = false;
                //resultInfo.Message = "Cannot store this Estimate";
                //resultInfo.Data = model;
                return Ok(-1);
            }
            return BadRequest();
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
        /// Business Logic for Estimate car insurance premium
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

            if (vehicleRate > 70000)
            {
                return 0.015m;
            }
            else if (vehicleRate > 50000)
            {
                return 0.012m;
            }
            else if (vehicleRate > 30000)
            {
                return 0.01m;
            }
            return 0;
        }
        //decimal GetRateOnVersion(string verion)
        //{
        //    int thisYear = System.DateTime.Now.Year;

        //    switch (thisYear - Int16.Parse(verion))
        //    {
        //        case 0:
        //        case 1:
        //            return 0;
        //            break;
        //        case 2:
        //        case 3:
        //            return 0.015m;
        //            break;
        //        case 4:
        //        case 5:
        //            return 0.016m;
        //            break;
        //        case 6:
        //        case 7:
        //        case 8:
        //        case 9:
        //            return 0.0175m;
        //            break;
        //        default:
        //            return 0.019m;
        //            break;
        //    }
        //}

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
