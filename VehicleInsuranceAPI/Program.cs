using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using VehicleInsuranceAPI.IResponsitory;
using VehicleInsuranceAPI.Responsitory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
builder.Services.AddRouting();
builder.Services.AddDbContext<VipDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionStrings")));

builder.Services.AddScoped<IAdmin, AdminService>();
builder.Services.AddScoped<ICustomer, CustomerService>();

builder.Services.AddCors(option =>
{
    option.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
