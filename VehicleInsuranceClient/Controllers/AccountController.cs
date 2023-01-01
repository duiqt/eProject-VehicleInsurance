using DataAccess.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Protocol.Plugins;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using VehicleInsuranceClient.Models.Contants;
using VehicleInsuranceClient.Models.Dtos;
//using DataAccess.Models;

namespace VehicleInsuranceClient.Controllers
{

    public class AccountController : Controller
    {
        private string urlCustomer = "https://localhost:7008/api/Customer/";
        private string urlAccount = "https://localhost:7008/api/Account/";


        HttpClient client = new HttpClient();

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Login")]
        public IActionResult Login(string? returnUrl)
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

        [HttpPost("Login")]
        public async Task<IActionResult> Login(CustomerDto cus, string? returnUrl)
        {
            try
            {
                client.BaseAddress = new Uri(urlCustomer);
                if (cus.CustomerEmail != null)
                {

                    var req = new LoginDto
                    {
                        CustomerEmail = cus.CustomerEmail,
                        Password = cus.Password
                    };
                    if (cus.CustomerEmail == req.CustomerEmail && cus.Password == req.Password)
                    {
                        var res = await client.PostAsync(RequestUriContants.Login, new StringContent(JsonConvert.SerializeObject(req), Encoding.UTF8, "application/json"));
                        var customer = JsonConvert.DeserializeObject<CustomerDto>(await res.Content.ReadAsStringAsync());
                        if (customer != null)
                        {
                            //luu session
                            var str = JsonConvert.SerializeObject(customer);
                            HttpContext.Session.SetString("user", str);
                            ViewBag.msg = string.Format("Login successfull");
                            ViewBag.user = customer;
                            if (returnUrl != null) 
                            {
                                return Redirect(returnUrl);
                            }
                            return RedirectToAction("Index", "Certificate");

                        }
                        else
                        {
                            ViewBag.msg = string.Format("Customer email or password incorrect.Please re-enter");
                            return View();
                        }
                    }
                    ViewBag.msg = string.Format("Customer email or password incorrect.Please re-enter");
                    return View();
                }

                return View();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("user");
            ViewBag.user = null;
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignUp(SignUpDto signUpDto)
        {
            var response = client.PostAsJsonAsync(urlCustomer + "SignUp", signUpDto).Result;
            if (response.IsSuccessStatusCode)
            {
                ViewBag.msg = string.Format("Create New Account Successfully");
                return RedirectToAction("Login", "Account");
            }
            ViewBag.msg = string.Format("Your Account was available");
            return View();
        }


        [HttpGet]
        public IActionResult Details()
        {
            var userString = HttpContext.Session.GetString("user");
            if (userString == null)
            {
                return RedirectToAction("Login", "Account");
            }
            // lay ID ra
            var obj = JsonConvert.DeserializeObject<Customer>(userString);
            var userResult = JsonConvert.DeserializeObject<CustomerDto>
                (client.GetStringAsync(urlCustomer + obj.Id).Result);
            return View(userResult);
        }


        [HttpGet]
        public IActionResult ChangePassword()
        {
            var userString = HttpContext.Session.GetString("user");
            if (userString == null)
            {
                return RedirectToAction("Login", "Account");
            }
            // lay ID ra
            var obj = JsonConvert.DeserializeObject<Customer>(userString);
            var model = JsonConvert.DeserializeObject<ChangePasswordDto>(client.GetStringAsync(urlCustomer + obj.Id).Result);
            ChangePasswordDto changePassword = new ChangePasswordDto
            {
                Id = model.Id,
                Password = model.Password

            };
            return View(changePassword);
        }

        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordDto changePasswordDto)
        {

            if (string.Compare(changePasswordDto.ChangePassword, changePasswordDto.ConfirmPassword) == 0)
            {
                //lay session ra
                var userString = HttpContext.Session.GetString("user");
                if (userString == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                // lay ID ra
                var obj = JsonConvert.DeserializeObject<Customer>(userString);
                changePasswordDto.Id = obj.Id;

                var model = client.PostAsJsonAsync<ChangePasswordDto>(urlCustomer + "NewPassword", changePasswordDto).Result;

                if (model.IsSuccessStatusCode)
                {
                    ViewBag.msg = string.Format("Update Product Successfully");
                    return RedirectToAction("Index", "Certificate");
                }
                return View();

            }
            ViewBag.msg = string.Format("Password and Confirm Password does not match");
            return View();
        }
    }
}
