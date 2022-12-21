using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net;
using VehicleInsuranceAPI.IResponsitory;
using VehicleInsuranceAPI.Models;
using VehicleInsuranceAPI.Models.Dtos;

namespace VehicleInsuranceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly VipDbContext _context;
        private readonly ICustomer service;
        private readonly IConfiguration _configuration;
        public CustomerController(ICustomer service)
        {
            this.service = service;
            this._configuration = _configuration;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto req)
        {
            return Ok(await service.Login(req));
        }

        //        [HttpGet]
        //        public async Task<ActionResult<Customer>> GetRegister()
        //        {
        //            return await _context.Customers.FirstOrDefaultAsync();
        //        }

        //        [HttpPost("Register")]
        //        public async Task<ActionResult<Register>> Register(Register register)
        //        {
        //            _context.Entry(register).State = EntityState.Added;
        //// _context.Customers.Add(customer);
        //            await _context.SaveChangesAsync();
        //            return register;
        //            //return CreatedAtAction("GetRegister", new { id = customer.Id }, customer);

        //        }

        //[HttpPost]
        //[AllowAnonymous]
        //public async Task<IActionResult> Register([FromForm] RegisterDto request)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    var result = await service.Register(request);
        //    if (!result)
        //    {
        //        return BadRequest("Register is unsuccessful.");
        //    }
        //    return Ok();
        //}


        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromForm] SignUpDto signUpDto)
        {
            var result = await service.SignUpAsync(signUpDto);
            if (result.Succeeded)
            {
                return Ok(result.Succeeded);
            }
            return Unauthorized();
        }

        //[HttpPost("signup")]
        //public string registration(Customer customer)
        //{
        //    SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString());
        //    SqlCommand cmd = new SqlCommand("INSERT INTO CUSTOMER(Username,Password,CustomerEmail,CustomerName, CustomerAddress, CustomerPhone)VALUES('" + customer.Username + "', '" + customer.Password + "', '" + customer.CustomerEmail + "', '" + customer.CustomerName + "','" + customer.CustomerAddress + "', '" + customer.CustomerEmail + "')", con);
        //    int i = cmd.ExecuteNonQuery();
        //    if (i > 0)
        //    {
        //        return "New member inserted";
        //    }
        //    return "Error";
        //}

    }
}
