using Serilog;
using XPressPayments.Auth.Service.Services;
using XPressPayments.Auth.Service.Services.Interface;

namespace XPressPayments.Auth.Service
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

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IAuthService, AuthService>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
