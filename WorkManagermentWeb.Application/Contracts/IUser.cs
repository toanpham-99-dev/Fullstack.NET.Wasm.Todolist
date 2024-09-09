using WorkManagermentWeb.Application.DTOs;
using WorkManagermentWeb.Application.DTOs.Responses;

namespace WorkManagermentWeb.Application.Contracts
{
    /// <summary>
    /// IUser
    /// </summary>
    public interface IUser
    {
        /// <summary>
        /// RegisterAsync
        /// </summary>
        /// <param name="registerUserDTO"></param>
        /// <returns></returns>
        Task<RegistrationResponse> RegisterAsync(RegisterUserDTO registerUserDTO);

        /// <summary>
        /// LoginAsync
        /// </summary>
        /// <param name="loginDTO"></param>
        /// <returns></returns>
        Task<GenerateTokenResponse> LoginAsync(LoginDTO loginDTO);

        /// <summary>
        /// ExternalLoginAsync
        /// </summary>
        /// <param name="externalLoginDTO"></param>
        /// <returns></returns>
        Task<GenerateTokenResponse> ExternalLoginAsync(ExternalLoginDTO externalLoginDTO);

        /// <summary>
        /// GetListAsync
        /// </summary>
        /// <param name="usersDTO"></param>
        /// <returns></returns>
        Task<UsersResponse> GetListAsync(UsersDTO usersDTO);

        /// <summary>
        /// GetByIdAsync
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProfileResponse> GetByIdAsync(string id);

        /// <summary>
        /// UpdateRolesAsync
        /// </summary>
        /// <param name="roleDTO"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<PostPutResponse> UpdateRolesAsync(UserRoleDTO roleDTO, string id);

        /// <summary>
        /// ChangepassWordAsync
        /// </summary>
        /// <param name="passwordDTO"></param>
        /// <returns></returns>
        Task<PostPutResponse> ChangePasswordAsync(PasswordDTO passwordDTO);

        /// <summary>
        /// UpdateAsync
        /// </summary>
        /// <param name="userDTO"></param>
        /// <returns></returns>
        Task<PostPutResponse> UpdateAsync(UpdateUserDTO userDTO);

        /// <summary>
        /// ChangeActiveStatusAsync
        /// </summary>
        /// <param name="activeStatusDTO"></param>
        /// <returns></returns>
        Task<PostPutResponse> ChangeActiveStatusAsync(ActiveStatusDTO activeStatusDTO);

        /// <summary>
        /// ValidatePhoneNumberAsync
        /// </summary>
        /// <param name="phoneNumberDTO"></param>
        /// <returns></returns>
        Task<CheckingDataResponse> ValidatePhoneNumberAsync(PhoneNumberDTO phoneNumberDTO);

        /// <summary>
        /// SendPhoneValidateTokenAsync
        /// </summary>
        /// <param name="phoneNumberDTO"></param>
        /// <returns></returns>
        Task<CheckingDataResponse> SendPhoneValidateTokenAsync(PhoneNumberDTO phoneNumberDTO);

        /// <summary>
        /// ValidateEmailAsync
        /// </summary>
        /// <param name="emailDTO"></param>
        /// <returns></returns>
        Task<CheckingDataResponse> ValidateEmailAsync(EmailDTO emailDTO);

        /// <summary>
        /// SendEmailValidateTokenAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<CheckingDataResponse> SendEmailValidateTokenAsync(string userId);

        /// <summary>
        /// SendPasswordResetTokenAsync
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<CheckingDataResponse> SendPasswordResetTokenAsync(string email);

        /// <summary>
        /// IsPasswordResetTokenAlive
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<CheckingDataResponse> IsPasswordResetTokenAlive(string userId, string token);

        /// <summary>
        /// ResetPasswordAsync
        /// </summary>
        /// <param name="resetPasswordDTO"></param>
        /// <returns></returns>
        Task<PostPutResponse> ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO);

        /// <summary>
        /// GetRoles
        /// </summary>
        /// <returns></returns>
        Task<GetRolesResponse> GetRolesAsync();

        /// <summary>
        /// RemindNearingDuesAsync
        /// </summary>
        /// <returns></returns>
        Task RemindNearingDuesAsync();
    }
}
