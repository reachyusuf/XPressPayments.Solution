using XPressPayments.Data.Dapper.Interface;

namespace XPressPayments.Data.Dapper
{
    public class UserDapperRepository : DapperBase, IUserDapperRepository
    {
        public UserDapperRepository(string conString) : base(conString)
        {

        }

        public async Task<IEnumerable<dynamic>> Users(int pageNumber = 1, int pageSize = 10, string search = null)
        {
            string sql = "SELECT U.ID, U.Email, U.EmailConfirmed, U.PhoneNumber, " +
                         " U.PhoneNumberConfirmed, U.ProfileName FROM AspNetUsers U " +
                         " WHERE U.ID + U.UserName + U.Email + U.Role + U.ProfileName LIKE (@Search) ORDER BY U.ProfileName ASC " +
                         " OFFSET (@PageNumber - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY ";

            search = (string.IsNullOrEmpty(search)) ? "%%" : $"%{search}%";
            var _params = new { PageNumber = pageNumber, PageSize = pageSize, Search = search };
            var results = await QueryAllAsync<dynamic>(sql, _params);
            return results;
        }
    }
}
