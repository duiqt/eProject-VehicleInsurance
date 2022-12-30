using DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        Task<Customer> ChangePassword(ChangePasswordDto changePasswordDto);
        Task<CreateClaimDto> CreateClaimCustomer(CreateClaimDto objCreateClaim);
    }
}
