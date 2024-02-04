namespace XPressPayments.Jobs.Service.Dtos
{
    public class AppSettingsDto
    {
        public string AppName { get; set; }
        public string HangFireConnectionString { get; set; }
        public string ClientKey { get; set; }
        public string CoreBaseUrl { get; set; }
        public string SendWelcomeEmailUrl { get; set; }
        public string SendReportUrl { get; set; }
        public string SendDailyWelcomeEmailCronExpression { get; set; }
        public string SendReportCronExpression { get; set; }
        public Seq Seq { get; set; }
    }

    public class Seq
    {
        public string ServerUrl { get; set; }
        public string ApiKey { get; set; }
    }
}
