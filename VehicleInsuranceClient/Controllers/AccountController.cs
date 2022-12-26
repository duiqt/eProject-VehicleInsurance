using DataAccess.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
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
                ViewBag.returnUrdl = returnUrl;
                return View();
            }
            else
            {
                ViewBag.returnUrdl = "";
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

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("user");
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
            var response = client.PostAsJsonAsync(urlCustomer, signUpDto).Result;
            if (response.IsSuccessStatusCode)
            {
                ViewBag.msg = string.Format("Create New Account Successfully");
                return RedirectToAction("Login", "Account");
            }
            ViewBag.msg = string.Format("Your Account was available");
            return View();
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            //lay session ra
            var userString = HttpContext.Session.GetString("user");
            if (userString == null)
            {
                return RedirectToAction("Login", "Account");
            }
            // lay ID ra
            var obj = JsonConvert.DeserializeObject<Customer>(userString);
            id = obj.Id;
            var userResult = JsonConvert.DeserializeObject<Customer>
                (client.GetStringAsync(urlCustomer + id).Result);
            return View(userResult);
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            var model = JsonConvert.DeserializeObject<Customer>(client.GetStringAsync(urlCustomer + Id).Result);
            ChangePasswordDto changePassword = new ChangePasswordDto
            {
                Id = model.Id,
                Password = model.Password
            };
            return View(changePassword);
        }

        [HttpPost]
        public IActionResult Edit(ChangePasswordDto changePasswordDto)
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
