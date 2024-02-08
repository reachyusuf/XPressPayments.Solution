using XPressPayments.Common.Dtos;
using XPressPayments.Data.Entities;

namespace XPressPayments.Business.Interfaces
{
    public interface IUserService
    {
        Task<OperationResult<PagedList<UserInfo>>> List(int pageIndex = 1, string? search = null);
        Task<OperationResult<PagedList<UserInfo>>> ListByLinq(int pageIndex = 1, string? search = null);
        Task<OperationResult<PagedList<UserInfo>>> ListByStoredProc(int pageIndex = 1, string? search = null);


    }
}
