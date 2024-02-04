using XPressPayments.Common.Dtos;

namespace XPressPayments.Core.Api.Services.Interface
{
    public interface IJobService
    {
        Task<OperationResult<bool>> SendDailyWelcomeEmail();
        Task<OperationResult<bool>> SendDailyReport();

    }
}
