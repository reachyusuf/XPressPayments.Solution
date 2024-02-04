using XPressPayments.Jobs.Service.Dtos;

namespace XPressPayments.Jobs.Service.Extensions
{
    public static class AppSettingsDtoExtensions
    {
        public static string GetFullUrl(this AppSettingsDto settings, string endpoint)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            if (string.IsNullOrEmpty(endpoint))
                throw new ArgumentException("Endpoint URL cannot be null or empty", nameof(endpoint));

            return settings.CoreBaseUrl.TrimEnd('/') + "/" + endpoint.TrimStart('/');
        }
    }

}
