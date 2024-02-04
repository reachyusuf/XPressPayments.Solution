using XPressPayments.Common.Dtos;
using XPressPayments.Core.Api.Services.Interface;

namespace XPressPayments.Core.Api.Services
{
    public class JobService : IJobService
    {
        public async Task<OperationResult<bool>> SendDailyReport()
        {
            throw new NotImplementedException();
        }

        public async Task<OperationResult<bool>> SendDailyWelcomeEmail()
        {
            throw new NotImplementedException();
        }
    }
}
