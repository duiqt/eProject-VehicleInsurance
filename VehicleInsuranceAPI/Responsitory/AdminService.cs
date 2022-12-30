using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using VehicleInsuranceAPI.IResponsitory;
using VehicleInsuranceAPI.Models.Dtos;

namespace VehicleInsuranceAPI.Responsitory
{
    public class AdminService : IAdmin
    {
        private VipDbContext db;

        public AdminService(VipDbContext db)
        {
            this.db = db;
        }
        public async Task<Admin> CheckLogin(string username, string password)
        {
            return await db.Admins.SingleOrDefaultAsync(a => a.UserName.Equals(username) && a.Password.Equals(password));
        }
        public async Task<AdminDto> Login(LoginAdminDto req)
        {
            AdminDto result = new AdminDto();
            var admin = await db.Admins.SingleOrDefaultAsync(c => c.UserName.Equals(req.UserName));
            if (admin != null)
            {
                if (admin.Password == req.Password)
                {
                    result = new AdminDto
                    {
                        AdminId = admin.AdminId,
                        UserName = admin.UserName,
                        Password = admin.Password,

                    };
                    return result;
                }

            }
            return null;
        }

    }
}
