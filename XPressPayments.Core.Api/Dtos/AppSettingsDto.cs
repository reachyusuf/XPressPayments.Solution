namespace XPressPayments.Core.Api.Dtos
{
    public class AppSettingsDto
    {
        public string AppName { get; set; }
        public string DBConnectionString { get; set; }
        public string ClientKey { get; set; }
        public Seq Seq { get; set; }
    }

    public class Seq
    {
        public string ServerUrl { get; set; }
        public string ApiKey { get; set; }
    }
}
