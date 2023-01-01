using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using VehicleInsuranceAPI.IResponsitory;
using VehicleInsuranceAPI.Models.Dtos;

namespace VehicleInsuranceAPI.Responsitory
{
    //public class DashboardService : IDashBoard
    //{
    //    public readonly VipDbContext db;

    //    public async Task<DashboardDto> PolicyDashboard(DashboardDto objDashboard)
    //    {
    //        var dashboard = await db.Policies.FirstOrDefaultAsync(x => x.Id == objDashboard.Id);       
    //        if (dashboard == null)
    //        {
    //            db.Policies.Add(objDashboard);
    //            await db.SaveChangesAsync();
    //            return objDashboard;
    //        }
    //        else
    //        {
    //            return null;
    //        }


    //    }
    //}
}
