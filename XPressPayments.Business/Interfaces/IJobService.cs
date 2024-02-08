using XPressPayments.Common.Dtos;

namespace XPressPayments.Business.Interfaces
{
    public interface IJobService
    {
        Task<OperationResult<bool>> SendDailyWelcomeEmail();
        Task<OperationResult<bool>> SendDailyReport();
    }
}
