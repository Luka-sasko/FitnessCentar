using AutoMapper;
using FitnessCentar.Model;
using FitnessCentar.Model.Common;
using FitnessCentar.Service.Common;
using FitnessCentar.WebAPI.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.UI;

namespace FitnessCentar.WebAPI.Controllers
{
    public class UserController : ApiController
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        
        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper= mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,User")]

        public async Task<HttpResponseMessage> GetUserAsync()
        {
            try
            {
                IUser profile = await _userService.GetUserAsync();

                if (profile == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
                var profileView = _mapper.Map<UserView>(profile);
                profileView.Role = await _userService.GetRoleTypeByRoleIdAsync(profile.RoleId);
                return Request.CreateResponse(HttpStatusCode.OK, profileView);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        //[Authorize(Roles = "Admin,User")]
        public async Task<HttpResponseMessage> CreateUserAsync(UserRegistered userRegistered)
        {
            if(userRegistered==null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            IUser profile=new User();
            profile.Firstname = userRegistered.Firstname;
            profile.Lastname = userRegistered.Lastname;
            profile.Email = userRegistered.Email;
            profile.Password = userRegistered.Password;
            profile.Contact=userRegistered.Contact;
            profile.Birthdate= userRegistered.Birthdate;
            profile.Weight=userRegistered.Weight;
            profile.Height=userRegistered.Height;
            
            try
            {
                bool created = await _userService.CreateUserAsync(profile);
                if(created) { return Request.CreateResponse(HttpStatusCode.OK); }
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPut]
        [Authorize(Roles ="Admin,User")]
        public async Task<HttpResponseMessage> UpdateUserAsync(UserUpdated updatedProfile)
        {
            if (updatedProfile == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            IUser profileInBase = await _userService.GetUserAsync();
            if(profileInBase == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            IUser user = new User
            {
                Id = profileInBase.Id, 
                Firstname = updatedProfile.Firstname,
                Lastname = updatedProfile.Lastname,
                Email = updatedProfile.Email,
                Password = updatedProfile.Password,
                Contact = updatedProfile.Contact,
                RoleId = profileInBase.RoleId, 
                CreatedBy = profileInBase.CreatedBy,
                DateCreated = profileInBase.DateCreated,
                UpdatedBy = profileInBase.Id, 
                DatedUpdated = DateTime.UtcNow,
            };

            try
            {
                bool updated = await _userService.UpdateUserAsync(user);
                if (updated)
                {
                    return Request.CreateResponse(HttpStatusCode.OK,"User updated!");
                }
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }

        }

        [HttpDelete]
        [Authorize(Roles = "Admin,User")]
        public async Task<HttpResponseMessage> DeleteUserAsync(Guid id)
        {
            try
            {
                
                bool deleted = await _userService.DeleteUserAsync(id);

                if (deleted)
                {
                    
                    return Request.CreateResponse(HttpStatusCode.OK, "User has been successfully deactivated.");
                }

                
                return Request.CreateResponse(HttpStatusCode.NotFound, "User not found or already deactivated.");
            }
            catch (Exception ex)
            {
                
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,User")]
        [Route("validate")]
        public async Task<HttpResponseMessage> ValidateUserAsync([FromBody] UserLoginRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Email and password must be provided.");
            }

            try
            {
                var user = await _userService.ValidateUserAsync(request.Email, request.Password);

                if (user == null)
                {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, "Invalid email or password.");
                }

                return Request.CreateResponse(HttpStatusCode.OK, "User authorized successfully.");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        [HttpPut]
        [Authorize(Roles = "Admin,User")]
        [Route("updatePassword")]
        public async Task<HttpResponseMessage> UpdatePasswordAsync([FromBody] PasswordUpdateModel passwordUpdateModel)
        {
            if (passwordUpdateModel == null || string.IsNullOrEmpty(passwordUpdateModel.PasswordNew))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            IUser profileInBase = await _userService.GetUserAsync();
            if (profileInBase == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            try
            {
                bool updated = await _userService.UpdatePasswordAsync(passwordUpdateModel.PasswordNew, passwordUpdateModel.PasswordOld);
                if (updated) return Request.CreateResponse(HttpStatusCode.OK, "Password updated");
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }


    }
}
