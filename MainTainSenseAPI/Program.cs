using Microsoft.EntityFrameworkCore;
using MainTainSenseAPI.Filters;
using Serilog;
using MainTainSenseAPI.Models;


var builder = WebApplication.CreateBuilder(args);
var logger = Log.Logger;

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Add services to the container.
builder.Services.AddDbContext<MainTainSenseDataContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("MTSDb")));

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>() // Assuming custom Identity classes
        .AddEntityFrameworkStores<MainTainSenseDataContext>();

builder.Services.AddAuthorization(options =>
{
    // You'll define your authorization policies here 
});
builder.Services.AddControllers(options =>
{
    options.Filters.Add(new AllowAllAuthorizationFilter()); // Your filter here
    options.Filters.Add(new GlobalExceptionFilter(logger));
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();

var app = builder.Build();

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration) // Read from appsettings.json
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs/myapp-{Date}.txt")
    .CreateLogger();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
