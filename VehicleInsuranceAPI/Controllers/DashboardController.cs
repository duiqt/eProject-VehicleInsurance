using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using VehicleInsuranceAPI.Models.Dtos;

namespace VehicleInsuranceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly VipDbContext _context;

        public DashboardController(VipDbContext context)
        {
            this._context = context;
        }

        [HttpGet("GetClaim")]
        public async Task<ActionResult<List<DashboardDto>>> GetClaim()
        {
            DateTime now = DateTime.Now;
            var startDate = new DateTime(now.Year, now.Month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            var customerBill = _context.Claims.
                                Where(p => p.DateOfAccident.Year == DateTime.Now.Year).
                                GroupBy(j => j.DateOfAccident.Month).
                                Select(k => new
                                {
                                    month = k.First().DateOfAccident.Month,
                                    totalAmount = k.Sum(a => a.ClaimableAmount),
                                });


            var listData = new List<DashboardDto>();
            if (customerBill != null && customerBill.ToList().Count > 0)
            {
                foreach (var item in customerBill)
                {
                    listData.Add(new DashboardDto
                    {
                        DateOfAccident = item.month,
                        ClaimableAmount = (decimal)item.totalAmount
                    });
                }
            }

            return listData;
        }

        [HttpGet("GetReport")]
        public async Task<ActionResult<DashboardOverviewDto>> GetReport()
        {
            DateTime now = DateTime.Now;
            var startDate = new DateTime(now.Year, now.Month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            var income = _context.CustomerBills.Where(p => p.Date.Value.Year == DateTime.Now.Year).ToList();
            var expense = _context.Claims.Where(j => j.DateOfAccident.Year == DateTime.Now.Year).ToList();
            var monthlySell = _context.CustomerBills.
                                Where(k => (k.Date >= startDate) && (k.Date <= endDate)).ToList();


            var data = new DashboardOverviewDto(
                (decimal)income.Sum(a => a.Amount),
                (decimal)expense.Sum(b => b.ClaimableAmount),
                (decimal)((decimal)income.Sum(a => a.Amount) - expense.Sum(b => b.ClaimableAmount)),
                (decimal)monthlySell.Sum(c => c.Amount)
            );

            return data;
        }

        [HttpGet("GetVehicleReport")]
        public async Task<ActionResult<List<CertificateDto>>> GetVehicleReport()
        {
            List<CertificateDto> model = new List<CertificateDto>();
            var dataJoin = (from cer in _context.Certificates
                            join est in _context.Estimates on cer.EstimateNo equals est.EstimateNo
                            select new
                            {
                                CertificateNo = cer.Id,
                                PolicyDate = est.PolicyDate,
                                VehicleName = est.VehicleName,
                            }).ToList();

            if (null == dataJoin)
            {
                return Ok(model);
            }

            for (int year = DateTime.Now.Year; year > DateTime.Now.Year - 3; year--)
            {
                var vehicleReport = dataJoin.Where(p => p.PolicyDate.Year == year).
                      GroupBy(j => j.VehicleName).
                      Select(k => new
                      {
                          vehicleCount = k.Count(a => a.CertificateNo != null),
                          vehicleName = k.First().VehicleName,
                      }).ToList();

                if (null == vehicleReport)
                {
                    continue;
                }

                foreach (var v in vehicleReport)
                {
                    decimal percenData = ((decimal)v.vehicleCount / (decimal)vehicleReport.Count()) * 100;
                    bool existData = model.Any(item => item.VehicleName == v.vehicleName);
                    if (existData)
                    {
                        var vehicleInfo = model.FirstOrDefault(j => j.VehicleName == v.vehicleName);

                        if (year == DateTime.Now.Year)
                        {
                            vehicleInfo.VehicleCountYearA = v.vehicleCount;
                            vehicleInfo.VehiclePercenYearA = percenData;
                        }
                        else if (year == DateTime.Now.Year - 1)
                        {
                            vehicleInfo.VehicleCountYearB = v.vehicleCount;
                            vehicleInfo.VehiclePercenYearB = percenData;
                        }
                        else
                        {
                            vehicleInfo.VehicleCountYearC = v.vehicleCount;
                            vehicleInfo.VehiclePercenYearC = percenData;
                        }
                    }
                    else
                    {
                        var vehicle = new CertificateDto
                        {
                            VehicleName = v.vehicleName,
                            VehicleCountYearA = year == DateTime.Now.Year ? v.vehicleCount : 0,
                            VehicleCountYearB = year == DateTime.Now.Year - 1 ? v.vehicleCount : 0,
                            VehicleCountYearC = year == DateTime.Now.Year - 2 ? v.vehicleCount : 0,
                            VehiclePercenYearA = year == DateTime.Now.Year ? percenData : 0,
                            VehiclePercenYearB = year == DateTime.Now.Year - 1 ? percenData : 0,
                            VehiclePercenYearC = year == DateTime.Now.Year - 2 ? percenData : 0,
                        };

                        model.Add(vehicle);
                    }
                }
            }


            return Ok(model);
        }
    }
}
