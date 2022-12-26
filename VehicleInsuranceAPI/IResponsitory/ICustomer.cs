using DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using VehicleInsuranceAPI.Models;
using VehicleInsuranceAPI.Models.Dtos;

namespace VehicleInsuranceAPI.IResponsitory
{
    public interface ICustomer
    {
        public Task<Customer> CheckLogin(string username, string password);
        public Task<CustomerDto> Login(LoginDto req);
        Task<Customer> SignUpCustomer(Customer objCustomer);
        Task<List<Customer>> GetOneById(int customerId);
        //Task<Customer> EditCustomer(Customer editCustomer, int Code);
        Task<Customer> ChangePassword(ChangePasswordDto changePasswordDto);
    }
}
