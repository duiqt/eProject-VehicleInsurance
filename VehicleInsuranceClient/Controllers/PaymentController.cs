using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using VehicleInsuranceClient.Models;
namespace VehicleInsuranceClient.Controllers
{
    public class PaymentController : Controller
    {
        /// <summary>
        /// Pay the bill for employee later by cash
        /// </summary>
        /// <returns>CustomerBill View</returns>
        [HttpPost]
        public IActionResult PayLater([FromForm] int PolicyNo, [FromForm] decimal Premium)
        {
            if (PolicyNo <= 0 || Premium <= 0)
            {
                ViewBag.ErrorMessage = "Invalid payment. Please dial (028)38460846 for more details";
                return RedirectToAction("Index", "Certificate");
            }
            try
            {
                using (var client = new HttpClient())
                {
                    int billNo = GenerateNumber();
                    StringContent stringContent = new StringContent(JsonSerializer.Serialize(new
                    {
                        BillNo = billNo.ToString(),
                        PolicyNo = PolicyNo,
                        Status = "Pending",
                        Date = DateTime.Now.Date.ToString("yyyy-MM-dd"),
                        Amount = Premium.ToString()
                    }), Encoding.UTF8, "application/json");
                    var response = client.PostAsync(Program.ApiAddress + "/Bill/PostCustomerBill", stringContent).Result;
                    var data = response.Content.ReadAsStringAsync().Result;
                    int result = JsonSerializer.Deserialize<int>(data);
                    if (result >= 0)
                    {
                        return RedirectToAction("Index", "Certificate");
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            ViewBag.ErrorMessage = "Pay later is not available now, please choose another payment method!";
            return View("Details", "Certificate");
        }

        //[Authorize]
        //public async Task<IActionResult> PayPaypal()
        //{
        //    var cookie = Request.Cookies["525010055"];
        //    EstimationContractModel model = System.Text.Json.JsonSerializer.Deserialize<EstimationContractModel>(cookie.Normalize())!;
        //    // **** 2.2 Setup payment config
        //    var environment = new SandboxEnvironment(_clientId, _secretKey);
        //    var client = new PayPalHttpClient(environment);

        //    #region Create Paypal Order
        //    var itemList = new ItemList()
        //    {
        //        Items = new List<Item>()
        //    };
        //    //var total = Math.Round((decimal)model.Estimation.Premium, 2);


        //    itemList.Items.Add(new Item()
        //    {
        //        Name = GenerateNumber().ToString(),
        //        Currency = "USD",
        //        Price = "0.02",
        //        Quantity = "1",
        //        Sku = "sku",
        //        Tax = "0"
        //    });

        //    #endregion
        //    var rand = GenerateNumber();
        //    var payment = new Payment()
        //    {
        //        Intent = "sale",
        //        Transactions = new List<Transaction>()
        //        {
        //            new Transaction()
        //            {
        //                Amount = new Amount()
        //                {
        //                    Total = "0.02",
        //                    Currency = "USD",
        //                    Details =  new AmountDetails{
        //                        Tax = "0",
        //                        Shipping = "0",
        //                        Subtotal = "0.02"
        //                    }
        //                },
        //                ItemList = itemList,
        //                Description = $"Pay{rand}",
        //                InvoiceNumber = $"BillNo{rand}"


        //            }
        //        },
        //        RedirectUrls = new RedirectUrls()
        //        {
        //            CancelUrl = $"{Program.ClientAddress}/Payment/CheckoutFail",
        //            ReturnUrl = $"{Program.ClientAddress}/Payment/CheckoutSuccess"
        //        },
        //        Payer = new Payer()
        //        {
        //            PaymentMethod = "paypal"
        //        }
        //    };

        //    // **** 2.3 Process payment
        //    PaymentCreateRequest request = new PaymentCreateRequest();
        //    request.RequestBody(payment);
        //    try
        //    {
        //        var response = await client.Execute(request);
        //        var statusCode = response.StatusCode;
        //        Payment result = response.Result<Payment>();

        //        var links = result.Links.GetEnumerator();
        //        string paypalRedirectUrl = null;
        //        while (links.MoveNext())
        //        {
        //            LinkDescriptionObject lnk = links.Current;
        //            if (lnk.Rel.ToLower().Trim().Equals("approval_url"))
        //            {
        //                //saving the payapalredirect URL to which user will be redirected for payment  
        //                paypalRedirectUrl = lnk.Href;
        //            }
        //        }

        //        return Redirect(paypalRedirectUrl);
        //    }
        //    catch (HttpException httpException)
        //    {
        //        var statusCode = httpException.StatusCode;
        //        var debugId = httpException.Headers.GetValues("PayPal-Debug-Id").FirstOrDefault();

        //        //Process when Checkout with Paypal fails
        //        return Redirect("/Payment/CheckoutFail");
        //    }
        //    return View();
        //}

        //[HttpPost]
        //public IActionResult PayLater([FromBody] CustomerBill model)
        //{
        //    CustomerBill customerBill = new CustomerBill();

        //    try
        //    {
        //        using (var client = new HttpClient())
        //        {
        //            StringContent stringContent = new StringContent(JsonSerializer.Serialize(new
        //            {
        //                BillNo = GenerateNumber(),
        //                PolicyNo = 5556,
        //                Status = "Pending",
        //                Date = DateTime.Now.Date,
        //                Amount = 1
        //            }), Encoding.UTF8, "application/json");
        //            var response = client.PostAsync(Program.ApiAddress + "/Bill/PostCustomerBill", stringContent).Result;
        //            var data = response.Content.ReadAsStringAsync().Result;
        //            int result = JsonSerializer.Deserialize<int>(data);
        //            if (result >= 0)
        //            {
        //                return View();
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //    return View("CustomerBill", customerBill);
        //}

        public IActionResult CheckoutFail()
        {
            return View();
        }
        public IActionResult CheckoutSuccess()
        {
            return View();
        }

        /// <summary>
        /// Generate unique 9 digits number used for Policy Number/ Bill Number
        /// </summary>
        /// <returns>Policy number</returns>
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
    }
}
