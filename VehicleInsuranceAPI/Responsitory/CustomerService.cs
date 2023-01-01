using Azure.Identity;
using DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text;
using VehicleInsuranceAPI.IResponsitory;
using VehicleInsuranceAPI.Models;
using VehicleInsuranceAPI.Models.Dtos;


namespace VehicleInsuranceAPI.Responsitory
{
    public class CustomerService : ICustomer
    {
        public readonly VipDbContext db;
        //public CustomerService(UserManager<Customer> userManager, VipDbContext dbContext)
        //{
        //    db = dbContext ??
        //         throw new ArgumentNullException(nameof(dbContext));

        //    _userManager = userManager;

        //}

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

        /// <summary>
        /// Register new account
        /// </summary>
        /// <param name="objCustomer"></param>
        /// <returns></returns>

        public async Task<Customer> SignUpCustomer(Customer objCustomer)
        {
            var customer = await db.Customers.FirstOrDefaultAsync(x => x.CustomerEmail.Equals(objCustomer.CustomerEmail));
            if (customer == null)
            {
                db.Customers.Add(objCustomer);
                await db.SaveChangesAsync();
                return objCustomer;
            }
            else
            {
                return null;
            }


        }

        /// <summary>
        /// create form Claim
        /// </summary>
        /// <param name="objClaim"></param>
        /// <returns></returns>
        public async Task<CreateClaimDto> CreateClaimCustomer(CreateClaimDto objCreateClaim)
        {
            //Certificate cer = new Certificate();
            var cer = await db.Certificates.AnyAsync(c => c.CustomerId == objCreateClaim.CustomerId);
            if (cer == false)
            {
                return null;
            }

            var claim = new Claim
            {
                PolicyNo = objCreateClaim.PolicyNo,
                ClaimNo = this.GenerateNumber(),
                PlaceOfAccident = objCreateClaim.PlaceOfAccident,
                DateOfAccident = objCreateClaim.DateOfAccident,
                Description = objCreateClaim.Description,
                Status = "Lodged",
                Image = objCreateClaim.Image,
                InsuredAmount = objCreateClaim.InsuredAmount,
                ClaimableAmount = 0
            };

            db.Claims.Add(claim);
            await db.SaveChangesAsync();
            return objCreateClaim;
        }

        public int GenerateNumber()
        {
            StringBuilder builder = new StringBuilder();
            byte digits = 9;
            foreach (char c in Guid.NewGuid().ToString())
            {
                builder.Append((short)c);
                if (builder.Length >= digits)
                {
                    break;
                }
            }
            return int.Parse(builder.ToString(0, digits));
        }

        public async Task<List<Customer>> GetOneById(int customerId)
        {
            return await db.Customers.Where(x => x.Id.Equals(customerId)).ToListAsync();
        }

        //public async Task<Customer> EditCustomer(Customer editCustomer, int Code)
        //{
        //    var claim = db.Customers.SingleOrDefault(p => p.Id ==Code );
        //    if (claim != null)
        //    {
        //        claim.CustomerAddress = editCustomer.CustomerAddress;
        //        claim.CustomerName = editCustomer.CustomerName;
        //        claim.CustomerPhone = editCustomer.CustomerPhone;
        //        claim.CustomerEmail = editCustomer.CustomerEmail;
        //        db.Customers.Update(claim);
        //        db.SaveChanges();
        //        return claim;
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
