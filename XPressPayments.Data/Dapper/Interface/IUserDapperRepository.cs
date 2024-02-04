namespace XPressPayments.Data.Dapper.Interface
{
    public interface IUserDapperRepository
    {
        Task<IEnumerable<dynamic>> Users(int pageNumber = 1, int pageSize = 10, string search = null);
    }
}
