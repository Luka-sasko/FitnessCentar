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

namespace FitnessCentar.WebAPI.Controllers
{
    public class UserController : ApiController
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("api/GetUserId")]
        // [Authorize(Roles = "Admin,User")]
        public async Task<HttpResponseMessage> GetUserIdAsync()
        {
            try
            {
                var userId = await _userService.GetUserIdAsync();

                if (userId == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound, "User ID not found.");

                return Request.CreateResponse(HttpStatusCode.OK, userId);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        // [Authorize(Roles = "Admin,User")]
        public async Task<HttpResponseMessage> GetUserAsync()
        {
            try
            {
                var user = await _userService.GetUserAsync();
                if (user == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound, "User not found.");

                var viewModel = _mapper.Map<UserView>(user);
                viewModel.Role = await _userService.GetRoleTypeByRoleIdAsync(user.RoleId);

                return Request.CreateResponse(HttpStatusCode.OK, viewModel);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        //[Authorize(Roles = "Admin,User")]
        public async Task<HttpResponseMessage> CreateUserAsync(UserRegistered model)
        {
            if (model == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid user data.");

            try
            {
                var user = _mapper.Map<User>(model);
                var created = await _userService.CreateUserAsync(user);

                if (created)
                    return Request.CreateResponse(HttpStatusCode.OK, "User registered successfully.");

                return Request.CreateResponse(HttpStatusCode.BadRequest, "Registration failed.");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        //[Authorize(Roles = "Admin,User")]
        public async Task<HttpResponseMessage> UpdateUserAsync(UserUpdated updatedProfile)
        {
            if (updatedProfile == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid request body.");

            var user = await _userService.GetUserAsync();
            if (user == null)
                return Request.CreateResponse(HttpStatusCode.NotFound, "User not found.");

            _mapper.Map(updatedProfile, user);
            user.DatedUpdated = DateTime.UtcNow;
            user.UpdatedBy = user.Id;

            try
            {
                var updated = await _userService.UpdateUserAsync(user);
                if (updated)
                    return Request.CreateResponse(HttpStatusCode.OK, "User updated successfully.");

                return Request.CreateResponse(HttpStatusCode.BadRequest, "Update failed.");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("updatePassword")]
        //[Authorize(Roles = "Admin,User")]
        public async Task<HttpResponseMessage> UpdatePasswordAsync([FromBody] PasswordUpdateModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.PasswordNew))
                return Request.CreateResponse(HttpStatusCode.BadRequest, "New password must be provided.");

            var user = await _userService.GetUserAsync();
            if (user == null)
                return Request.CreateResponse(HttpStatusCode.NotFound, "User not found.");

            try
            {
                var updated = await _userService.UpdatePasswordAsync(model.PasswordNew, model.PasswordOld);
                if (updated)
                    return Request.CreateResponse(HttpStatusCode.OK, "Password updated.");

                return Request.CreateResponse(HttpStatusCode.BadRequest, "Incorrect current password.");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("validate")]
        //[Authorize(Roles = "Admin,User")]
        public async Task<HttpResponseMessage> ValidateUserAsync([FromBody] UserLoginRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Email and password must be provided.");

            try
            {
                var user = await _userService.ValidateUserAsync(request.Email, request.Password);

                if (user == null)
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, "Invalid email or password.");

                return Request.CreateResponse(HttpStatusCode.OK, "User authorized successfully.");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        //[Authorize(Roles = "Admin,User")]
        public async Task<HttpResponseMessage> DeleteUserAsync(Guid id)
        {
            try
            {
                var deleted = await _userService.DeleteUserAsync(id);
                if (deleted)
                    return Request.CreateResponse(HttpStatusCode.OK, "User has been successfully deactivated.");

                return Request.CreateResponse(HttpStatusCode.NotFound, "User not found or already deactivated.");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
