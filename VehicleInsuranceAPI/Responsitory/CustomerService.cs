using Azure.Identity;
using DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;
using VehicleInsuranceAPI.IResponsitory;
using VehicleInsuranceAPI.Models;
using VehicleInsuranceAPI.Models.Dtos;

namespace VehicleInsuranceAPI.Responsitory
{
    public class CustomerService : ICustomer
    {
        public VipDbContext db;
        private readonly UserManager<Customer> _userManager;
        //private readonly SignInManager<Customer> _signInManager;
        public CustomerService(UserManager<Customer> userManager)
        {
            _userManager = userManager;
            //_signInManager = signInManager;
        }

        public CustomerService(VipDbContext db)
        {
            this.db = db;
        }
        public async Task<Customer> CheckLogin(string username, string password)
        {
            var model = await db.Customers.SingleOrDefaultAsync(c => c.Username.Equals(username) && c.Password.Equals(password));
            if (model != null)
            {
                return model;
            }
            else
            {
                return null;
            }
        }
        public async Task<CustomerDto> Login(LoginDto req)
        {
            CustomerDto result = new CustomerDto();
            var customer = await db.Customers.SingleOrDefaultAsync(c => c.Username.Equals(req.Username));
            if (customer != null)
            {
                if (customer.Password == req.Password)
                {
                    result = new CustomerDto
                    {
                        Id = customer.Id,
                        Username = customer.Username,
                        Password = customer.Password,
                        CustomerAddress = customer.CustomerAddress,
                        CustomerName = customer.CustomerName,
                        CustomerPhone = customer.CustomerPhone
                    };
                    return result;
                }

            }
            return null;
        }

        public Task<IdentityResult> SignUpAsync(SignUpDto signUpDto)
        {
            throw new NotImplementedException();
        }

        //public async Task<IdentityResult> SignUpAsync(SignUpDto signUpDto)
        //{
        //    var user = new ApplicationUser()
        //    {

        //        Password = signUpDto.Password,
        //        CustomerName = signUpDto.CustomerName,
        //        CustomerPhone = signUpDto.CustomerPhone
        //    };
        //    return await _userManager.CreateAsync(user, signUpDto.Password);
        //}
    }
}
