using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace XPressPayments.Data.Dapper
{
    public class DapperBase
    {
        private readonly string _connectionString;

        public DapperBase(string _connectionString)
        {
            this._connectionString = _connectionString;
        }

        public async Task<T> QuerySingle<T>(string sql, object parameters = null)
        {
            var flag = default(T);
            using (var connection = CreateConnection())
            {
                flag = await connection.QueryFirstOrDefaultAsync<T>(sql, parameters, commandType: CommandType.Text);
            }
            return flag;
        }

        public async Task<IEnumerable<T>> QueryAllAsync<T>(string sql, object parameters = null)
        {
            var flag = default(IEnumerable<T>);
            using (var connection = CreateConnection())
            {
                flag = await connection.QueryAsync<T>(sql, parameters, commandType: CommandType.Text);
            }
            return flag;
        }

        public async Task<int> ExecuteQueryAsync(string sql, object parameters = null)
        {
            int flag = 0;
            using (var connection = CreateConnection())
            {
                flag = await connection.ExecuteAsync(sql, parameters, commandType: CommandType.Text);
            }
            return flag;
        }

        private IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }

}
