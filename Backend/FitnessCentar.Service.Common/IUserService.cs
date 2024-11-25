using FitnessCentar.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCentar.Service.Common
{
    public interface IUserService
    {
        Task<IUser> GetUserAsync();
        Task<bool> CreateUserAsync(IUser user);
        Task<bool> UpdateUserAsync(IUser user);
        Task<bool> DeleteUserAsync(Guid id);
        Task<IUser> ValidateUserAsync(string email, string password);
        Task<IUser> ValidateUserByPasswordAsync(Guid id, string password);
        Task<string> GetRoleTypeByRoleIdAsync(Guid id);
        Task<string> GetUserEmailByIdAsync(Guid id);
        Task<bool> UpdatePasswordAsync(string passwordNew, string passwordOld);
    }
}
