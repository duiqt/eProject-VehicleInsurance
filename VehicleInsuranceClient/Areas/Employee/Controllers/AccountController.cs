
using DataAccess.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Protocol;
using System.Data;
using System.Security.Claims;
using System.Security.Policy;
using VehicleInsuranceClient.Areas.Employee.Models.Dtos;

namespace VehicleInsuranceClient.Areas.Employee.Controllers
{
    [Area("Employee")]
    [Authorize(Roles = "Admin", AuthenticationSchemes = "AdminAuth")]
    public class AccountController : Controller
    {
        private string urlAdmin = "https://localhost:7008/api/Admin/";
        private HttpClient client = new HttpClient();
        Random random = new Random();
        public IActionResult Index()
        {
            var userString = HttpContext.Session.GetString("admin");
            if (userString == null)
            {
                return RedirectToAction("LoginAdmin", "Account");
            }
            return View();
        }

        [HttpGet("loginAdmin")]
        [AllowAnonymous]
        public IActionResult LoginAdmin(string? returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl))
            {
                ViewBag.returnUrl = returnUrl;
                return View();
            }
            else
            {
                ViewBag.returnUrl = "";
                return View();
            }

        }
        [HttpPost("loginAdmin")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAdminAsync(LoginAdminDtos admin)
        {
            await HttpContext.SignOutAsync();
            try
            {
                if (admin.UserName != null && admin.Password != null)
                {
                    var res = JsonConvert.DeserializeObject<LoginAdminDtos>(client.GetStringAsync(urlAdmin + admin.UserName + "/" + admin.Password).Result);
                    if (res != null)
                    {
                        //luu session
                        var str = JsonConvert.SerializeObject(res);
                        HttpContext.Session.SetString("admin", str);
                        ViewBag.msg = string.Format("Login successfull");
                        return RedirectToAction("Index", "Home");
                    }
                }

                return View();
            }
            catch (Exception e)
            {
                return View();
            }

        }

        //[HttpGet("logoutAdmin")]
        //public async Task<IActionResult> logoutAdmin()
        //{
        //    await HttpContext.SignOutAsync();
        //    return RedirectToAction("loginAdmin", "Account");
        //}
        [HttpGet("logoutAdmin")]
        [AllowAnonymous]
        public IActionResult logoutAdmin()
        {
            HttpContext.Session.Remove("admin");
            return RedirectToAction("LoginAdmin", "Account");
        }

    }


}
