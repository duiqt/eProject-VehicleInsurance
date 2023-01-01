using DataAccess.Models;

namespace VehicleInsuranceAPI.IResponsitory
{
    public interface IDashBoard
    {
        Task<Policy> PolicyDashboard(Policy objDashboard);
    }
}
