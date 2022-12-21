using DataAccess.Models;

namespace VehicleInsuranceAPI.IResponsitory
{
    public interface IAdmin
    {
        //public Task<Admin> CheckLogin(string username, string password);
        Task<Admin> Get();
    }
}
