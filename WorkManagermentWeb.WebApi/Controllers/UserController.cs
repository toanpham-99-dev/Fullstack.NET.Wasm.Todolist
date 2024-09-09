using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WorkManagermentWeb.Application.Constants;
using WorkManagermentWeb.Application.Contracts;
using WorkManagermentWeb.Application.DTOs;
using WorkManagermentWeb.Application.DTOs.Responses;
using WorkManagermentWeb.Infrastructure.Options;

namespace WorkManagermentWeb.WebApi.Controllers
{
    /// <summary>
    /// UserController
    /// </summary>
    [Route($"{ApiRouteConstants.PrefixUrl}/{ApiRouteConstants.User}")]
    [ApiController]
    public class UserController : ControllerBase
    {
        /// <summary>
        /// IUser
        /// </summary>
        private readonly IUser _user;

        /// <summary>
        /// CommonOptions
        /// </summary>
        private readonly CommonOptions _commonOptions;

        /// <summary>
        /// UserController
        /// </summary>
        /// <param name="user"></param>
        /// <param name="commonOptions"></param>
        public UserController(IUser user, IOptions<CommonOptions> commonOptions)
        {
            _user = user;
            _commonOptions = commonOptions.Value;
        }

        /// <summary>
        /// GetListAsync
        /// </summary>
        /// <param name="usersDTO"></param>
        /// <returns></returns>
        [Authorize(AuthorizationConstants.TwoAuthPolicy)]
        [HttpGet]
        public async Task<ActionResult<UsersResponse>> GetListAsync([FromQuery] UsersDTO usersDTO)
        {
            var result = await _user.GetListAsync(usersDTO);
            return Ok(result);
        }

        /// <summary>
        /// GetByIdAsync
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(AuthorizationConstants.TwoAuthPolicy)]
        [HttpGet($"{{{ApiRouteConstants.Id}}}")]
        public async Task<ActionResult<ProfileResponse>> GetByIdAsync(string id)
        {
            var result = await _user.GetByIdAsync(id);
            if (!result.Flag)
                return NotFound(result);
            return Ok(result);
        }

        /// <summary>
        /// UpdateRolesAsync
        /// </summary>
        /// <param name="roleDTO"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = $"{UserRoleConstants.CEO}")]
        [Authorize(AuthorizationConstants.TwoAuthPolicy)]
        [HttpPut($"{{{ApiRouteConstants.Id}}}/{ApiRouteConstants.Roles}")]
        public async Task<ActionResult<PostPutResponse>> UpdateRolesAsync(UserRoleDTO roleDTO, string id)
        {
            var result = await _user.UpdateRolesAsync(roleDTO, id)
                .ConfigureAwait(false);
            if (!result.Flag)
                return NotFound(result);
            return Ok(result);
        }

        /// <summary>
        /// ChangePasswordAsync
        /// </summary>
        /// <param name="passwordDTO"></param>
        /// <returns></returns>
        [Authorize(AuthorizationConstants.TwoAuthPolicy)]
        [HttpPut(ApiRouteConstants.ChangePassword)]
        public async Task<ActionResult<PostPutResponse>> ChangePasswordAsync(PasswordDTO passwordDTO)
        {
            var result = await _user.ChangePasswordAsync(passwordDTO)
                .ConfigureAwait(false);
            if (!result.Flag)
                return NotFound(result);
            return Ok(result);
        }

        /// <summary>
        /// UpdateAsync
        /// </summary>
        /// <param name="userDTO"></param>
        /// <returns></returns>
        [Authorize(AuthorizationConstants.TwoAuthPolicy)]
        [HttpPut]
        public async Task<ActionResult<PostPutResponse>> UpdateAsync(UpdateUserDTO userDTO)
        {
            var result = await _user.UpdateAsync(userDTO)
                .ConfigureAwait(false);
            if (!result.Flag)
                return NotFound(result);
            return Ok(result);
        }

        /// <summary>
        /// ChangeActiveStatusAsync
        /// </summary>
        /// <param name="activeStatusDTO"></param>
        /// <returns></returns>
        [Authorize(AuthorizationConstants.TwoAuthPolicy)]
        [Authorize(Roles = $"{UserRoleConstants.CEO}")]
        [HttpPut(ApiRouteConstants.ChangeStatus)]
        public async Task<ActionResult<PostPutResponse>> ChangeActiveStatusAsync(ActiveStatusDTO activeStatusDTO)
        {
            var result = await _user.ChangeActiveStatusAsync(activeStatusDTO)
                .ConfigureAwait(false);
            if (!result.Flag)
                return NotFound(result);
            return Ok(result);
        }

        /// <summary>
        /// GetRoles
        /// </summary>
        /// <returns></returns>
        [Authorize(AuthorizationConstants.TwoAuthPolicy)]
        [HttpGet(ApiRouteConstants.Roles)]
        public async Task<ActionResult<GetRolesResponse>> GetRoles()
        {
            GetRolesResponse response = await _user.GetRolesAsync().ConfigureAwait(false);
            return Ok(response);
        }

        /// <summary>
        /// ValidatePhoneNumberAsync
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [Authorize(AuthorizationConstants.TwoAuthPolicy)]
        [HttpGet($"{{{ApiRouteConstants.Id}}}/{ApiRouteConstants.ValidatePhoneNumber}")]
        public async Task<ActionResult<CheckingDataResponse>> ValidatePhoneNumberAsync(string id, [FromQuery] string phoneNumber, [FromQuery] string token)
        {
            var result = await _user.ValidatePhoneNumberAsync(new PhoneNumberDTO
            {
                UserId = id,
                PhoneNumber = phoneNumber,
                Token = token
            }).ConfigureAwait(false);
            if (!result.Flag)
                return NotFound(result);
            return Ok(result);
        }

        /// <summary>
        /// SendPhoneValidateTokenAsync
        /// </summary>
        /// <param name="id"></param>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        [Authorize(AuthorizationConstants.TwoAuthPolicy)]
        [HttpGet($"{{{ApiRouteConstants.Id}}}/{ApiRouteConstants.RequestPhoneVerifyCode}")]
        public async Task<ActionResult<CheckingDataResponse>> SendPhoneValidateTokenAsync(string id, [FromQuery] string phoneNumber)
        {
            var result = await _user.SendPhoneValidateTokenAsync(new PhoneNumberDTO
            {
                UserId = id,
                PhoneNumber = phoneNumber
            })
            .ConfigureAwait(false);
            if (!result.Flag)
                return NotFound(result);
            return Ok(result);
        }

        /// <summary>
        /// ValidateEmailAsync
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet($"{{{ApiRouteConstants.Id}}}/{ApiRouteConstants.VerifyEmail}")]
        public async Task<RedirectResult> ValidateEmailAsync(string id, [FromQuery] string token)
        {
            var result = await _user.ValidateEmailAsync(new EmailDTO
            {
                UserId = id,
                Token = token
            }).ConfigureAwait(false);
            if (!result.Flag)
            {
                return Redirect($"{_commonOptions.Domain}/not-found");
            }
            return Redirect($"{_commonOptions.Domain}/email-comfirmed?flag=true");
        }

        /// <summary>
        /// SendEmailValidateTokenAsync
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(AuthorizationConstants.TwoAuthPolicy)]
        [HttpGet($"{{{ApiRouteConstants.Id}}}/{ApiRouteConstants.RequestEmailVerifyCode}")]
        public async Task<ActionResult<CheckingDataResponse>> SendEmailValidateTokenAsync(string id)
        {
            var result = await _user.SendEmailValidateTokenAsync(id)
            .ConfigureAwait(false);
            if (!result.Flag)
                return NotFound(result);
            return Ok(result);
        }
    }
}
