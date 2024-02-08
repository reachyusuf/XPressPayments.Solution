using XPressPayments.Data.Entities;

namespace XPressPayments.Data.Dapper.Interface
{
    public interface IUserDapperRepository
    {
        Task<IEnumerable<UserInfo>> Users(int pageNumber = 1, int pageSize = 10, string search = null);
    }
}
