using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using VehicleInsuranceAPI.IResponsitory;

namespace VehicleInsuranceAPI.Responsitory
{
    public class AdminService : IAdmin
    {
        private VipDbContext db;

        public AdminService(VipDbContext db)
        {
            this.db = db;
        }
        //public async Task<Admin> CheckLogin(string username, string password)
        //{
        //    return await db.Admins.SingleOrDefaultAsync(a => a.UserName.Equals(username) && a.Password.Equals(password));
        //}

        public async Task<Admin> Get()
        {
            return await db.Admins.FirstOrDefaultAsync();
        }
    }
}
