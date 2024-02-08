using Microsoft.EntityFrameworkCore;
using XPressPayments.Business.Interfaces;
using XPressPayments.Common.Dtos;
using XPressPayments.Common.Helpers;
using XPressPayments.Data.Dapper.Interface;
using XPressPayments.Data.DataAccess;
using XPressPayments.Data.Entities;
using XPressPayments.Data.UnitOfWork;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace XPressPayments.Business.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUnitofWork _unitofwork;
        private readonly IUserDapperRepository _dapper;
        private readonly AppDbContext _dbContext;
        private const int pageSize = 10;

        public UserService(IUnitofWork _unitofwork, IUserDapperRepository _dapper, AppDbContext _dbContext)
        {
            this._unitofwork = _unitofwork;
            this._dapper = _dapper;
            this._dbContext = _dbContext;
        }

        public async Task<OperationResult<PagedList<UserInfo>>> List(int pageIndex = 1, string? search = null)
        {
            var predicate = PredicateBuilder.True<UserInfo>();
            if (string.IsNullOrEmpty(search) is false) predicate = predicate.And(i => (i.Id + i.Email + i.ProfileName + i.Role).Contains(search));
            var query = _unitofwork.Repository<UserInfo>().FindAll(predicate, asNoTracking: true);
            var pagedResult = PagedList<UserInfo>.ToPagedList(query, pageIndex: pageIndex);
            return OperationResult<PagedList<UserInfo>>.ReturnSuccess(pagedResult);
        }

        public async Task<OperationResult<PagedList<UserInfo>>> ListByLinq(int pageIndex = 1, string? search = null)
        {
            var query = _dbContext.Users.Where(u => (u.Id + u.Email + u.ProfileName).Contains(search));
            int itemsToSkip = (pageIndex - 1) * pageSize;
            var users = query
                        .Select(u => new UserInfo { Id = u.Id, UserName = u.UserName, Email = u.Email, ProfileName = u.ProfileName })
                        .OrderBy(u => u.ProfileName)
                        .Skip(itemsToSkip)
                        .Take(pageSize)
                        .ToList();
            var pagedResult = PagedList<UserInfo>.ToPagedList(users, pageIndex: pageIndex);
            return OperationResult<PagedList<UserInfo>>.ReturnSuccess(pagedResult);
        }

        public async Task<OperationResult<PagedList<UserInfo>>> ListByStoredProc(int pageIndex = 1, string? search = null)
        {
            var result = await _dapper.Users(pageIndex, search: search);
            var pagedResult = PagedList<UserInfo>.ToPagedList(result, pageIndex: pageIndex);
            return OperationResult<PagedList<UserInfo>>.ReturnSuccess(pagedResult);
        }
    }

}
