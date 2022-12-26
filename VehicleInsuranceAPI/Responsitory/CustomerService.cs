using Azure.Identity;
using DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using VehicleInsuranceAPI.IResponsitory;
using VehicleInsuranceAPI.Models;
using VehicleInsuranceAPI.Models.Dtos;

namespace VehicleInsuranceAPI.Responsitory
{
    public class CustomerService : ICustomer
    {
        public readonly VipDbContext db;
        private readonly UserManager<Customer> _userManager;
        public CustomerService(UserManager<Customer> userManager, VipDbContext dbContext)
        {
            db = dbContext ??
                 throw new ArgumentNullException(nameof(dbContext));

            _userManager = userManager;
        }

        public CustomerService(VipDbContext db)
        {
            this.db = db;
        }

        public async Task<Customer> CheckLogin(string customeremail, string password)
        {
            var model = await db.Customers.SingleOrDefaultAsync(c => c.CustomerEmail.Equals(customeremail) && c.Password.Equals(password));
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
            var customer = await db.Customers.SingleOrDefaultAsync(c => c.CustomerEmail.Equals(req.CustomerEmail));
            if (customer != null)
            {
                if (customer.Password == req.Password)
                {
                    result = new CustomerDto
                    {
                        Id = customer.Id,
                        CustomerEmail = customer.CustomerEmail,
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

        public async Task<Customer> SignUpCustomer(Customer objCustomer)
        {
            var customer = await db.Customers.FirstOrDefaultAsync(x => x.CustomerEmail.Equals(objCustomer.CustomerEmail));
            if (customer != null)
            {
                return null;
            }
            db.Customers.Add(objCustomer);
            await db.SaveChangesAsync();

            return objCustomer;
        }

        public async Task<List<Customer>> GetOneById(int customerId)
        {
            return await db.Customers.Where(x => x.Id.Equals(customerId)).ToListAsync();
        }

        //public async Task<Customer> EditCustomer(Customer editCustomer, int Code)
        //{
        //    var customer = db.Customers.SingleOrDefault(p => p.Id ==Code );
        //    if (customer != null)
        //    {
        //        customer.CustomerAddress = editCustomer.CustomerAddress;
        //        customer.CustomerName = editCustomer.CustomerName;
        //        customer.CustomerPhone = editCustomer.CustomerPhone;
        //        customer.CustomerEmail = editCustomer.CustomerEmail;
        //        db.Customers.Update(customer);
        //        db.SaveChanges();
        //        return customer;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        public async Task<Customer> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            var customer = db.Customers.SingleOrDefault(p => p.Id == changePasswordDto.Id);

            if (customer != null)
            {
                if (customer.Password == changePasswordDto.Password)
                {
                    customer.Password = changePasswordDto.ChangePassword;
                    db.Customers.Update(customer);
                    db.Entry(customer).State = EntityState.Modified;
                    db.SaveChanges();
                    return customer;
                }
            }
            return null;
        }
    }
}
