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

        //public Task<bool> Register(SignUpDto request);
        //Task<AddNewModel> GetNewByIdAsync(int newId);

        Task<IdentityResult> SignUpAsync(SignUpDto signUpDto);
        //Task<int> AddNewAsync(AddNewModel addNewModel);

    }
}
