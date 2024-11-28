using FitnessCentar.Model.Common;
using FitnessCentar.Repository.Common;
using FitnessCentar.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;




namespace FitnessCentar.Service
{
    public class UserService: IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleTypeRepository _roleTypeRepository;

        public UserService(IUserRepository userRepository, IRoleTypeRepository roleTypeRepository)
        {
            _userRepository = userRepository;
            _roleTypeRepository = roleTypeRepository;
        }

        public async Task<Model.Common.IUser> GetUserAsync()
        {
            var userId = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
            try
            {
                return await _userRepository.GetByIdAsync(userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> CreateUserAsync(Model.Common.IUser user)
        {
            var id=Guid.NewGuid();
            user.CreatedBy = id;
            user.UpdatedBy= id;
            user.Id= id;
            user.IsActive = true;
            user.CoachId = Guid.Parse("151b32b8-16c1-49f5-91fb-cb6b4bff141c");
            user.SubscriptionId = Guid.Parse("00000000-0000-0000-0000-000000000000");
            try
            {
                return await _userRepository.CreateAsync(user);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public async Task<bool> UpdateUserAsync(Model.Common.IUser profile)
        {
            try
            {
                var userIdString = HttpContext.Current.User.Identity.GetUserId();

                var userId = Guid.Parse(userIdString);

                return await _userRepository.UpdateAsync(userId, profile);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            var userId = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
            try
            {
                return await _userRepository.DeleteAsync(id,userId,DateTime.UtcNow);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<Model.Common.IUser> ValidateUserAsync(string email, string password)
        {
            try
            {
                var a= await _userRepository.ValidateUserAsync(email, password);
                return a;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public async Task<string> GetRoleTypeByRoleIdAsync(Guid id)
        {
            try
            {
                return await _roleTypeRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> GetUserEmailByIdAsync(Guid id)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(id);
                return user.Email;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UpdatePasswordAsync(string passwordNew, string passwordOld)
        {
            var id = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
            try
            {
                return await _userRepository.UpdatePasswordAsync(id, passwordNew, passwordOld);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
