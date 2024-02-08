namespace XPressPayments.Common.Dtos
{
    public class AppSettingsDto
    {
        public string AppName { get; set; }
        public string DBConnectionString { get; set; }
        public string ClientKey { get; set; }

        public Jwt Jwt { get; set; }
        public Seq Seq { get; set; }

        public RateLimit RateLimit { get; set; }

    }

    public class Jwt
    {
        public string JwtIssuer { get; set; }
        public string JwtSecretKey { get; set; }
        public int JwtTokenExpiredTime { get; set; }
    }


    public class Seq
    {
        public string ServerUrl { get; set; }
        public string ApiKey { get; set; }
    }

    public class RateLimit
    {
        public int Minutes { get; set; }
        public int Request { get; set; }
    }
}
