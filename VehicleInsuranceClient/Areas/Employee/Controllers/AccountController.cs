﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Security.Claims;
using System.Security.Policy;
using VehicleInsuranceClient.Areas.Employee.Models.Dtos;

namespace VehicleInsuranceClient.Areas.Employee.Controllers
{
    [Area("Employee")]
    //[Authorize(Roles = "Admin", AuthenticationSchemes = "AdminAuth")]
    public class AccountController : Controller
    {
        private string urlAdmin = "https://localhost:7008/api/Admin/";
        private HttpClient client = new HttpClient();
        Random random = new Random();
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("loginAdmin")]
        [AllowAnonymous]
        public IActionResult LoginAdmin(string? returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl))
            {
                ViewBag.returnUrdl = returnUrl;
                return View();
            }
            else
            {
                ViewBag.returnUrdl = "";
                return View();
            }

        }
        [HttpPost("loginAdmin")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAdminAsync(string UserName, string Password, string? returnUrl)
        {
            await HttpContext.SignOutAsync();
            //try
            //{
            var res = JsonConvert.DeserializeObject<AdminDto>(client.GetStringAsync(urlAdmin + UserName + "/" + Password).Result);
            if (res != null)
            {
                var claim = new List<Claim>();
                claim.Add(new Claim(ClaimTypes.Name, res.UserName));
                claim.Add(new Claim(ClaimTypes.NameIdentifier, res.AdminId.ToString()));
                claim.Add(new Claim(ClaimTypes.Role, "Admin"));
                var claimIdentify = new ClaimsIdentity(claim, "AdminAuth");
                var claimPrincipal = new ClaimsPrincipal(claimIdentify);
                await HttpContext.SignInAsync("AdminAuth", claimPrincipal);
                if (returnUrl != null)
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
            //}
            //catch (Exception e)
            //{
            //    throw e;
            //}

        }

        [HttpGet("logoutAdmin")]
        public async Task<IActionResult> logoutAdmin()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("loginAdmin", "Account");
        }

      

    }


}