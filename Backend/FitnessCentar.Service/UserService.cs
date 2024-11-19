using FitnessCentar.Model.Common;
using FitnessCentar.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCentar.Service
{
    public class UserService:IUserService
    {
        
        public async Task<IUser> GetUserAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CreateUserAsync(IUser user)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateUserAsync(IUser user)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteUserAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IUser> ValidateUserAsync(string username, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetRoleTypeByRoleIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetUserEmailByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdatePasswordAsync(string passwordNew, string passwordOld)
        {
            throw new NotImplementedException();
        }

        
    }
}
