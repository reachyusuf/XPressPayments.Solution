using Hangfire;
using Microsoft.AspNetCore.Authentication.Cookies;
using Serilog;
using XPressPayments.Jobs.Service.Dtos;
using XPressPayments.Jobs.Service.Filters;
using XPressPayments.Jobs.Service.Services;
using XPressPayments.Jobs.Service.Services.Interface;

namespace XPressPayments.Jobs.Service
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

            //--
            builder.Services.AddControllersWithViews();
            // Add services to the container.
            //builder.Services.Configure<AppSettingsDto>(appSettings);
            builder.Services.Configure<AppSettingsDto>(config.GetSection("AppSettings"));
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddScoped<IBackgroundJobService, BackgroundJobService>();

            //builder.Services.AddScoped<IRabbitMqService, RabbitMqService>();
            //builder.Services.AddScoped<ILogService, LogService>();
            //builder.Services.AddScoped<ICapricornService, CapricornService>();
            //builder.Services.AddScoped<IPaymentService, PaymentService>();
            //builder.Services.AddScoped<IAuthService, AuthService>();

            //--hang fire using sql server
            builder.Services.AddHangfire(x => x.UseSqlServerStorage(appSettings.HangFireConnectionString));
            builder.Services.AddHangfireServer();
            //--hang fire using sql server

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Index"; // Set the login page path
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment() is false)
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            //--hangfire
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new HangfireAuthorizationFilter() }
            });

            //app.MapRazorPages();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");


            RecurringJob.AddOrUpdate<IBackgroundJobService>(
            "XPress-Pay-Jobs-SendDailyWelcomeEmail",
            x => x.SendDailyWelcomeEmail(),
            appSettings.SendDailyWelcomeEmailCronExpression,
            TimeZoneInfo.FindSystemTimeZoneById("W. Central Africa Standard Time")
        ).RetryEvery(TimeSpan.FromMinutes(5)).OnException((exception, context) =>
        {
            Console.WriteLine($"Job failed: {exception.Message}. Retrying...");
            // Log or handle the exception if needed
        });

            RecurringJob.AddOrUpdate<IBackgroundJobService>(
            "XPress-Pay-Jobs-SendDailyWelcomeEmail",
            x => x.SendDailyReport(),
            appSettings.SendReportCronExpression,
            TimeZoneInfo.FindSystemTimeZoneById("W. Central Africa Standard Time")
        ).RetryEvery(TimeSpan.FromMinutes(5)).OnException((exception, context) =>
        {
            Console.WriteLine($"Job failed: {exception.Message}. Retrying...");
            // Log or handle the exception if needed
        });

            //RecurringJob.AddOrUpdate<IBackgroundJobService>("XPress-Pay-Jobs-SendDailyWelcomeEmail", x => x.SendDailyWelcomeEmail(), appSettings.SendDailyWelcomeEmailCronExpression, TimeZoneInfo.FindSystemTimeZoneById("W. Central Africa Standard Time"));
            //RecurringJob.AddOrUpdate<IBackgroundJobService>("XPress-Pay-Jobs-SendDailyReport", x => x.SendDailyReport(), appSettings.SendReportCronExpression, TimeZoneInfo.FindSystemTimeZoneById("W. Central Africa Standard Time"));

            app.Run();
        }
    }



    //public class Program
    //{
    //    public static void Main(string[] args)
    //    {
    //        var builder = WebApplication.CreateBuilder(args);

    //        // Add services to the container.
    //        builder.Services.AddControllersWithViews();

    //        var app = builder.Build();

    //        // Configure the HTTP request pipeline.
    //        if (!app.Environment.IsDevelopment())
    //        {
    //            app.UseExceptionHandler("/Home/Error");
    //            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    //            app.UseHsts();
    //        }

    //        app.UseHttpsRedirection();
    //        app.UseStaticFiles();
    //        app.UseRouting();
    //        app.UseAuthorization();
    //        app.MapControllerRoute(
    //            name: "default",
    //            pattern: "{controller=Home}/{action=Index}/{id?}");

    //        app.Run();
    //    }
    //}
}
