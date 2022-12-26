using DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VehicleInsuranceAPI.IResponsitory;

namespace VehicleInsuranceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private IAdmin service;
        public AdminController(IAdmin service)
        {
            this.service = service;
        }

        [HttpGet("{username}/{password}")]
        public async Task<Admin> Get(string username, string password)
        {
            return await service.CheckLogin(username, password);
        }
    }
}
