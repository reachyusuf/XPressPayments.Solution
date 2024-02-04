using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using XPressPayments.Common.Helpers;
using XPressPayments.Jobs.Service.Dtos;
using XPressPayments.Jobs.Service.Extensions;
using XPressPayments.Jobs.Service.Services.Interface;

namespace XPressPayments.Jobs.Service.Services
{
    public class BackgroundJobService : IBackgroundJobService
    {
        private readonly AppSettingsDto? _appSettings = null;

        public BackgroundJobService(IOptions<AppSettingsDto> appSettings)
        {
            this._appSettings = appSettings.Value ?? throw new Exception("Appsettings.json file is not found");
        }

        public async Task SendDailyWelcomeEmail()
        {
            var endpoint = _appSettings.GetFullUrl(_appSettings?.SendWelcomeEmailUrl);
            //_logger.LogInformation($"Started Mware-Processor-{nameof(this.SendWelcomeEmail)}->({endpoint})");
            var response = await FlurlHttpClientUtil.GetJSON<object>(endpoint, headers: GetAuthHeader());
            var resultString = JsonConvert.SerializeObject(response);
            //_logger.LogInformation($"Finished Mware-Processor-{nameof(this.RunDailySettlements)}->({endpoint}) Response is {resultString}");
        }

        public async Task SendDailyReport()
        {
            var endpoint = _appSettings.GetFullUrl(_appSettings?.SendReportUrl);
            //_logger.LogInformation($"Started Mware-Processor-{nameof(this.VerifyDailySettlements)}->({endpoint})");
            var response = await FlurlHttpClientUtil.GetJSON<object>(endpoint, headers: GetAuthHeader());
            var resultString = JsonConvert.SerializeObject(response);
            //_logger.LogInformation($"Finished Mware-Processor-{nameof(this.VerifyDailySettlements)}->({endpoint}) Response is {resultString}");
        }

        private dynamic GetAuthHeader()
        {
            return new { Content_Type = "application/json", ApiKey = $"{_appSettings?.ClientKey}" };
        }
    }

}
