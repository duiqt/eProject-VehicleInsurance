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
using System.Net.Http;
using Microsoft.AspNetCore.Identity;

namespace VehicleInsuranceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly VipDbContext _context;
        private readonly ICustomer service;
        public CustomerController(ICustomer _service, VipDbContext context)
        {
            this._context = context;
            this.service = _service;

        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginAdmin(LoginDto req)
        {
            return Ok(await service.Login(req));
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp(Customer customer)
        {
            var result = await service.SignUpCustomer(customer);

            if (result == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Sign Up New Account Unsuccessfully");
            }
            return Ok("Sign Up New Account Successfully");
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = await _context.Customers.Where(p => p.Id == id).FirstOrDefaultAsync();

            if (customer == null)
            {
                return BadRequest();
            }

            return customer;
        }

        [HttpPost("EditEmployee")]
        public async Task<ActionResult<Customer>> EditEmployee(Customer customer)
        {
            _context.Entry(customer).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return customer;
        }

        [HttpPost("NewPassword")]
        public async Task<ActionResult<Customer>> NewPassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            var result = await service.ChangePassword(changePasswordDto);
            return result;
        }
    }
}