using FitnessCentar.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCentar.Repository.Common
{
    public interface IUserRepository
    {
        Task<IUser> GetByIdAsync (Guid id);
        Task<bool> CreateAsync (IUser user);
        Task<bool> UpdateAsync(Guid Id,IUser user);
        Task<bool> DeleteAsync(Guid Id);
        Task<IUser> ValidateUserAsync (string username, string password);


    }
}
