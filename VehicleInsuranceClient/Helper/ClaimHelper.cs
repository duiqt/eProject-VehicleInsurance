namespace VehicleInsuranceClient.Helper
{
    public static class ClaimHelper
    {
        public static string GetUserId(this HttpContext context)
        {
            var user = context.User;
            if (user != null && user.Identity.IsAuthenticated)
            {
                return GetClaim(context, "userId");
            }
            return string.Empty;
        }
        public static string GetClaim(this HttpContext context, string claimType)
        {
            var claim = context.User.Claims.FirstOrDefault(c => c.Type == claimType);
            return claim?.Value ?? string.Empty;
        }
    }
}
