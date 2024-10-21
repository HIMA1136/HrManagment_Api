using DataAccess.Config;
using Application;
using HrManagment_Api.Helper;
using Application.Model.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.FileProviders;
using System.Globalization;
using DataAccess.DataContext;
using Domain.Entites;
using Microsoft.EntityFrameworkCore;
using DataAccess.DataContext.Seed;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers().AddJsonOptions(options => { options.JsonSerializerOptions.PropertyNamingPolicy = null; });
builder.Services.ConfigureHttpJsonOptions(options => options.SerializerOptions.PropertyNamingPolicy = null);
builder.Services.AddEndpointsApiExplorer();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.ConfigureApplication();
builder.Services.ConfigureDataAccess(connectionString);
builder.Services.ConfigureCorsPolicy();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureJwt(builder.Configuration);
builder.Services.Configure<JWTSetting>(builder.Configuration.GetSection("JWTSetting"));


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    try
    {
        var _dataContext = scope.ServiceProvider.GetService<DataContextApp>();
        var _userManager = scope.ServiceProvider.GetService<UserManager<AppUser>>();

        _dataContext?.Database.EnsureCreated();
        _dataContext?.Database.Migrate();
        var dataSeed = new DataSeed();
        dataSeed.SeedAsync(_dataContext, _userManager).Wait();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred while migrating the database: {ex.Message}");
    }
}
var options = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("ar-EG"),
    SupportedCultures = new List<CultureInfo>() { new CultureInfo("ar-EG"), new CultureInfo("en-US") },
};
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseRequestLocalization(options);
app.UseStaticFiles();
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors(builder => builder
       .AllowAnyHeader()
       .AllowAnyMethod()
       .AllowAnyOrigin()
    );
app.UseRouting();
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<GlobalExceptionHandler>();
app.UseMiddleware<AuthJwtMiddleware>();
//app.MapControllers();
app.MapEndPoints();

app.Run();