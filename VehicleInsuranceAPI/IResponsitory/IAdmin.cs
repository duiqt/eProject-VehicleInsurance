using DataAccess.Models;
using VehicleInsuranceAPI.Models.Dtos;

namespace VehicleInsuranceAPI.IResponsitory
{
    public interface IAdmin
    {
        public Task<Admin> CheckLogin(string username, string password);
        public Task<AdminDto> Login(LoginAdminDto req);
    }
}
