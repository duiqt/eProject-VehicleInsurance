using Microsoft.AspNetCore.Authentication.Cookies;

namespace VehicleInsuranceClient
{
    public class Program
    {
        public static string ApiAddress = "https://localhost:7008/api";
        public static short CookieEstimateDuration = 14;
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
            builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(15);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            builder.Services.AddMvc();
            builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = "UserAuth";
            })
                    .AddCookie("UserAuth", options =>
                    {
                        options.LoginPath = "/Login";
                        options.AccessDeniedPath = "/AccessDenied";
                        options.Events = new CookieAuthenticationEvents()
                        {
                            OnSigningIn = async context =>
                            {
                                await Task.CompletedTask;
                            },
                            OnSignedIn = async context =>
                            {
                                await Task.CompletedTask;
                            },
                            OnValidatePrincipal = async context =>
                            {
                                await Task.CompletedTask;
                            }
                        };

                    })
                    .AddCookie("AdminAuth", options =>
                    {
                        options.LoginPath = "/loginAdmin";
                        options.AccessDeniedPath = "/AccessDenied";
                        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                    });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();

            app.UseCors(x => x.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                  name: "areas",
                  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.Run();
        }
    }
}