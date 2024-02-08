
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using XPressPayments.Business.Implementations;
using XPressPayments.Business.Interfaces;
using XPressPayments.Common.Dtos;
using XPressPayments.Common.Dtos.AuthService;
using XPressPayments.Core.Api.Middlewares;
using XPressPayments.Data.Dapper;
using XPressPayments.Data.Dapper.Interface;
using XPressPayments.Data.DataAccess;
using XPressPayments.Data.EFRepository;
using XPressPayments.Data.Entities;
using XPressPayments.Data.UnitOfWork;

namespace XPressPayments.Core.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var config = new ConfigurationBuilder()
                        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                        .AddJsonFile("appsettings.json", optional: true)
                        .AddJsonFile($"appsettings.{envName}.json", optional: true)
                        .AddEnvironmentVariables()
                        .Build();

            var appSettings = new AppSettingsDto();
            config.Bind(appSettings);

            if (appSettings is null || string.IsNullOrEmpty(appSettings?.AppName) is true)
                throw new Exception("Invalid AppSettings or couldn't bind appsettings.json file");
            //--

            //--
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Warning()
            .WriteTo.Seq(serverUrl: appSettings.Seq.ServerUrl, apiKey: appSettings.Seq.ApiKey)
            .CreateLogger();
            //--
            // Add services to the container.

            //--

            builder.Services.AddControllers().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<RegisterDto>());

            // Learn more about configuring Swagger/OpenAPI


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IUnitofWork, UnitofWork>();
            builder.Services.AddScoped<IUserDapperRepository, UserDapperRepository>();

            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IJobService, JobService>();
            builder.Services.AddScoped<IUserService, UserService>();

            builder.Services.AddMemoryCache();

            //--
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(appSettings.DBConnectionString));
            builder.Services.AddIdentity<UserInfo, Role>(options =>
            {
                // Configure identity options if needed
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders()
            .AddUserManager<UserManager<UserInfo>>()
            .AddSignInManager<SignInManager<UserInfo>>();
            //--


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //--register middlewares
            app.UseMiddleware<RateLimitMiddleware>(TimeSpan.FromMinutes(appSettings.RateLimit.Minutes), appSettings.RateLimit.Request); // Example: 100 requests per minute

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
