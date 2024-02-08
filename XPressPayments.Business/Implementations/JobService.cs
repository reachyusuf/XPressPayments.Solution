using XPressPayments.Business.Interfaces;
using XPressPayments.Common.Dtos;

namespace XPressPayments.Business.Implementations
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
