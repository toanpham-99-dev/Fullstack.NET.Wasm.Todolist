using Microsoft.AspNetCore.Mvc;
using WorkManagermentWeb.Application.Constants;
using WorkManagermentWeb.Application.Contracts;
using WorkManagermentWeb.Application.DTOs.Responses;
using WorkManagermentWeb.Application.DTOs;

namespace WorkManagermentWeb.WebApi.Controllers
{
    /// <summary>
    /// AuthController
    /// </summary>
    [Route($"{ApiRouteConstants.PrefixUrl}/{ApiRouteConstants.Auth}")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        /// <summary>
        /// IUser
        /// </summary>
        private readonly IUser _user;

        /// <summary>
        /// AuthController
        /// </summary>
        /// <param name="user"></param>
        public AuthController(IUser user)
        {
            _user = user;
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="loginDTO"></param>
        /// <returns></returns>
        [HttpPost(ApiRouteConstants.ExternalLogin)]
        public async Task<ActionResult<GenerateTokenResponse>> ExternalLogin(ExternalLoginDTO loginDTO)
        {
            var result = await _user.ExternalLoginAsync(loginDTO);
            if (!result.Flag)
                return NotFound(result);
            return Ok(result);
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="loginDTO"></param>
        /// <returns></returns>
        [HttpPost(ApiRouteConstants.Login)]
        public async Task<ActionResult<GenerateTokenResponse>> Login(LoginDTO loginDTO)
        {
            var result = await _user.LoginAsync(loginDTO);
            if (!result.Flag)
                return NotFound(result);
            return Ok(result);
        }

        /// <summary>
        /// Register
        /// </summary>
        /// <param name="registerUserDTO"></param>
        /// <returns></returns>
        [HttpPost(ApiRouteConstants.Register)]
        public async Task<ActionResult<RegistrationResponse>> Register(RegisterUserDTO registerUserDTO)
        {
            var result = await _user.RegisterAsync(registerUserDTO);
            if (!result.Flag)
                return NotFound(result);
            return Ok(result);
        }

        /// <summary>
        /// SendTempPaswordAsync
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet($"{ApiRouteConstants.RequestResetPasswordToken}")]
        public async Task<ActionResult<CheckingDataResponse>> SendPasswordResetTokenAsync(string email)
        {
            var result = await _user.SendPasswordResetTokenAsync(email)
                .ConfigureAwait(false);
            if (!result.Flag)
                return NotFound(result);
            return Ok(result);
        }

        /// <summary>
        /// SendTempPaswordAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet($"{ApiRouteConstants.IsPasswordResetTokenAlive}")]
        public async Task<ActionResult<CheckingDataResponse>> IsPasswordResetTokenAlive(string userId, string token)
        {
            var result = await _user.IsPasswordResetTokenAlive(userId, token)
                .ConfigureAwait(false);
            if (!result.Flag)
                return NotFound(result);
            return Ok(result);
        }

        /// <summary>
        /// SendTempPaswordAsync
        /// </summary>
        /// <param name="resetPasswordDTO"></param>
        /// <returns></returns>
        [HttpPut($"{ApiRouteConstants.Password}")]
        public async Task<ActionResult<PostPutResponse>> ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO)
        {
            var result = await _user.ResetPasswordAsync(resetPasswordDTO)
                .ConfigureAwait(false);
            if (!result.Flag)
                return NotFound(result);
            return Ok(result);
        }
    }
}
