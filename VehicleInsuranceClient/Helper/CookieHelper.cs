using System.Text.Json;
using VehicleInsuranceClient.Models;
namespace VehicleInsuranceClient.Helper
{
    public static class CookieHelper
    {
        /// <summary>
        /// This method is to create Cookie based on key and value 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void CreateCookie(HttpContext context, string key, ContractModel model)
        {
            TimeSpan remain = DateTime.Now.Subtract((DateTime)model.Estimation.EstimateDate);
            CookieOptions options = new CookieOptions()
            {
                Expires = DateTime.Now.AddDays(Program.CookieEstimateDuration - remain.TotalDays),
                Secure = true,
                SameSite = SameSiteMode.None
            };
            context.Response.Cookies.Append(key, JsonSerializer.Serialize(model), options);
        }

        /// <summary>
        /// This method is to remove Cookie based on key
        /// </summary>
        /// <param name="key"></param>
        public static void RemoveCookie(HttpContext context, string key)
        {
            CookieOptions options = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(-1)
            };
            context.Response.Cookies.Append(key, String.Empty, options);
        }
    }
}
