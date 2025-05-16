using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using CustomerRetentionAPI.Services;
using CustomerRetentionAPI.Data;
using CustomerRetentionAPI.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Customer Retention API", Version = "v1" });
});

// Register repositories and services
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerChurnDataRepository, CustomerChurnDataRepository>();
builder.Services.AddScoped<ICustomerChurnPredictionRepository, CustomerChurnPredictionRepository>();
builder.Services.AddScoped<CustomerPredictionService>();

builder.Services.AddDbContext<CRMContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Customer Retention API v1"));
}

app.UseHttpsRedirection();
app.UseCors("AllowAllOrigins"); // Apply the CORS policy
app.MapControllers();

app.Run();