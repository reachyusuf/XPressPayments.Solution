using XPressPayments.Data.Dapper.Interface;
using XPressPayments.Data.Entities;

namespace XPressPayments.Data.Dapper
{
    public class UserDapperRepository : DapperBase, IUserDapperRepository
    {
        public UserDapperRepository(string conString) : base(conString)
        {

        }

        public async Task<IEnumerable<UserInfo>> Users(int pageNumber = 1, int pageSize = 10, string search = null)
        {
            //--this can be converted to a stored procedure for better performance
            string sql = "SELECT Id, Email, Role, ProfileName, CreatedDate From AspNetUsers " +
                         " WHERE ID + Email + Role + ProfileName LIKE (@Search) ORDER BY ProfileName ASC " +
                         " OFFSET (@PageNumber - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY ";

            search = (string.IsNullOrEmpty(search)) ? "%%" : $"%{search}%";
            var _params = new { PageNumber = pageNumber, PageSize = pageSize, Search = search };
            var results = await QueryAllAsync<UserInfo>(sql, _params);
            return results;
        }
    }
}
