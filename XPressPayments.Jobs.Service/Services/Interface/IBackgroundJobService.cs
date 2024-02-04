namespace XPressPayments.Jobs.Service.Services.Interface
{
    public interface IBackgroundJobService
    {
        Task SendDailyWelcomeEmail();
        Task SendDailyReport();
    }
}
