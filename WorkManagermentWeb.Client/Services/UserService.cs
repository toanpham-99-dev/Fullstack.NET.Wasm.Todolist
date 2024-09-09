using System.Net.Http.Json;
using WorkManagermentWeb.Application.Contracts;
using WorkManagermentWeb.Application.DTOs.Responses;
using WorkManagermentWeb.Application.DTOs;
using Blazored.LocalStorage;
using WorkManagermentWeb.Application.GenericModels;
using WorkManagermentWeb.Application.Constants;

namespace WorkManagermentWeb.Client.Services
{
    /// <summary>
    /// UserService
    /// </summary>
    public class UserService : IUser
    {
        /// <summary>
        /// httpClient
        /// </summary>
        private readonly HttpClient _httpClient;

        /// <summary>
        /// ILocalStorageService
        /// </summary>
        private readonly ILocalStorageService _localStorageService;

        /// <summary>
        /// UserUrlPrefix
        /// </summary>
        private readonly string _userUrlPrefix;

        /// <summary>
        /// UserService
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="localStorageService"></param>
        public UserService(HttpClient httpClient, ILocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
            _userUrlPrefix = $"{ApiRouteConstants.PrefixUrl}/{ApiRouteConstants.User}";
        }

        /// <summary>
        /// ExternalLoginAsync
        /// </summary>
        /// <param name="externalLoginDTO"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<GenerateTokenResponse> ExternalLoginAsync(ExternalLoginDTO externalLoginDTO)
        {
            var httpResponse = await _httpClient.PostAsJsonAsync($"{ApiRouteConstants.PrefixUrl}/{ApiRouteConstants.Auth}/{ApiRouteConstants.ExternalLogin}", externalLoginDTO);
            var response = await httpResponse.Content.ReadFromJsonAsync<GenerateTokenResponse>();
            return response!;
        }

        /// <summary>
        /// LoginAsync
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<GenerateTokenResponse> LoginAsync(LoginDTO model)
        {
            var httpResponse = await _httpClient.PostAsJsonAsync($"{ApiRouteConstants.PrefixUrl}/{ApiRouteConstants.Auth}/{ApiRouteConstants.Login}", model);
            var response = await httpResponse.Content.ReadFromJsonAsync<GenerateTokenResponse>();
            return response!;
        }

        /// <summary>
        /// RegisterAsync
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<RegistrationResponse> RegisterAsync(RegisterUserDTO model)
        {
            var httpResponse = await _httpClient.PostAsJsonAsync($"{ApiRouteConstants.PrefixUrl}/{ApiRouteConstants.Auth}/{ApiRouteConstants.Register}", model);
            var response = await httpResponse.Content.ReadFromJsonAsync<RegistrationResponse>();
            return response!;
        }

        /// <summary>
        /// GetListAsync
        /// </summary>
        /// <param name="usersDTO"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<UsersResponse> GetListAsync(UsersDTO usersDTO)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            await AddTokenToHeaderAsync();
            string url = $"{_userUrlPrefix}" +
                $"?{nameof(usersDTO.Paging)}.{nameof(usersDTO.Paging.Skip)}={usersDTO.Paging.Skip}" +
                $"&{nameof(usersDTO.Paging)}.{nameof(usersDTO.Paging.Take)}={usersDTO.Paging.Take}";

            if (!string.IsNullOrEmpty(usersDTO.SearchInput))
            {
                url += $"&{nameof(usersDTO.SearchInput)}={usersDTO.SearchInput}";
            }
            if (!string.IsNullOrEmpty(usersDTO.RoleName))
            {
                url += $"&{nameof(usersDTO.RoleName)}={usersDTO.RoleName}";
            }
            if (usersDTO.ActiveStatus is not null)
            {
                url += $"&{nameof(usersDTO.ActiveStatus)}={usersDTO.ActiveStatus}";
            }
            if (usersDTO.BoardId != Guid.Empty)
            {
                url += $"&{nameof(usersDTO.BoardId)}={usersDTO.BoardId}";
            }
            var httpResponse = await _httpClient.GetAsync(url.ToLower());

            //Read Response
            if (!httpResponse.IsSuccessStatusCode)
                return null!;

            var result = await httpResponse.Content.ReadAsStringAsync();
            return Generics.DeserializeJsonString<UsersResponse>(result);
        }

        /// <summary>
        /// GetByIdAsync
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ProfileResponse> GetByIdAsync(string id)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            await AddTokenToHeaderAsync();
            var httpResponse = await _httpClient.GetAsync($"{_userUrlPrefix}/{id}");

            //Read Response
            if (!httpResponse.IsSuccessStatusCode)
                return null!;

            var result = await httpResponse.Content.ReadAsStringAsync();
            return Generics.DeserializeJsonString<ProfileResponse>(result);
        }

        /// <summary>
        /// RemindNearingDuesAsync
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task RemindNearingDuesAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// UpdateRolesAsync
        /// </summary>
        /// <param name="roleDTO"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PostPutResponse> UpdateRolesAsync(UserRoleDTO roleDTO, string id)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            await AddTokenToHeaderAsync();
            var httpResponse = await _httpClient.PutAsJsonAsync($"{_userUrlPrefix}/{id}/{ApiRouteConstants.Roles}", roleDTO);
            var response = await httpResponse.Content.ReadFromJsonAsync<PostPutResponse>();
            return response!;
        }

        /// <summary>
        /// ChangePasswordAsync
        /// </summary>
        /// <param name="passwordDTO"></param>
        /// <returns></returns>
        public async Task<PostPutResponse> ChangePasswordAsync(PasswordDTO passwordDTO)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            await AddTokenToHeaderAsync();
            var httpResponse = await _httpClient.PutAsJsonAsync($"{_userUrlPrefix}/{ApiRouteConstants.ChangePassword}", passwordDTO);
            var response = await httpResponse.Content.ReadFromJsonAsync<PostPutResponse>();
            return response!;
        }

        /// <summary>
        /// UpdateAsync
        /// </summary>
        /// <param name="userDTO"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PostPutResponse> UpdateAsync(UpdateUserDTO userDTO)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            await AddTokenToHeaderAsync();
            var httpResponse = await _httpClient.PutAsJsonAsync($"{_userUrlPrefix}", userDTO);
            var response = await httpResponse.Content.ReadFromJsonAsync<PostPutResponse>();
            return response!;
        }

        /// <summary>
        /// ChangeActiveStatusAsync
        /// </summary>
        /// <param name="activeStatusDTO"></param>
        /// <returns></returns>
        public async Task<PostPutResponse> ChangeActiveStatusAsync(ActiveStatusDTO activeStatusDTO)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            await AddTokenToHeaderAsync();
            var httpResponse = await _httpClient.PutAsJsonAsync($"{_userUrlPrefix}/{ApiRouteConstants.ChangeStatus}", activeStatusDTO);
            var response = await httpResponse.Content.ReadFromJsonAsync<PostPutResponse>();
            return response!;
        }

        /// <summary>
        /// ValidatePhoneNumberAsync
        /// </summary>
        /// <param name="phoneNumberDTO"></param>
        /// <returns></returns>
        public async Task<CheckingDataResponse> ValidatePhoneNumberAsync(PhoneNumberDTO phoneNumberDTO)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            await AddTokenToHeaderAsync();
            string url = $"{_userUrlPrefix}/{phoneNumberDTO.UserId}/{ApiRouteConstants.ValidatePhoneNumber}" +
                $"?{nameof(phoneNumberDTO.PhoneNumber)}={phoneNumberDTO.PhoneNumber}" +
                $"&{nameof(phoneNumberDTO.Token)}={phoneNumberDTO.Token}";
            var httpResponse = await _httpClient.GetAsync(url);

            var result = await httpResponse.Content.ReadAsStringAsync();
            return Generics.DeserializeJsonString<CheckingDataResponse>(result);
        }

        /// <summary>
        /// GetRolesAsync
        /// </summary>
        /// <returns></returns>
        public async Task<GetRolesResponse> GetRolesAsync()
        {
            _httpClient.DefaultRequestHeaders.Clear();
            await AddTokenToHeaderAsync();
            string url = $"{_userUrlPrefix}/{ApiRouteConstants.Roles}";
            var httpResponse = await _httpClient.GetAsync(url);

            //Read Response
            if (!httpResponse.IsSuccessStatusCode)
                return null!;

            var result = await httpResponse.Content.ReadAsStringAsync();
            return Generics.DeserializeJsonString<GetRolesResponse>(result);
        }

        /// <summary>
        /// SendPhoneValidateTokenAsync
        /// </summary>
        /// <param name="phoneNumberDTO"></param>
        /// <returns></returns>
        public async Task<CheckingDataResponse> SendPhoneValidateTokenAsync(PhoneNumberDTO phoneNumberDTO)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            await AddTokenToHeaderAsync();
            string url = $"{_userUrlPrefix}/{phoneNumberDTO.UserId}/{ApiRouteConstants.RequestPhoneVerifyCode}" +
                $"?{nameof(phoneNumberDTO.PhoneNumber)}={phoneNumberDTO.PhoneNumber}";
            var httpResponse = await _httpClient.GetAsync(url);

            var result = await httpResponse.Content.ReadAsStringAsync();
            return Generics.DeserializeJsonString<CheckingDataResponse>(result);
        }


        /// <summary>
        /// SendEmailValidateTokenAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<CheckingDataResponse> SendEmailValidateTokenAsync(string userId)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            await AddTokenToHeaderAsync();
            string url = $"{_userUrlPrefix}/{userId}/{ApiRouteConstants.RequestEmailVerifyCode}";
            var httpResponse = await _httpClient.GetAsync(url);

            var result = await httpResponse.Content.ReadAsStringAsync();
            return Generics.DeserializeJsonString<CheckingDataResponse>(result);
        }

        public Task<CheckingDataResponse> ValidateEmailAsync(EmailDTO emailDTO)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// AddTokenToHeaderAsync
        /// </summary>
        private async Task AddTokenToHeaderAsync()
        {
            string? token = await _localStorageService.GetItemAsStringAsync(AuthorizationConstants.Token);
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers
                .AuthenticationHeaderValue(AuthorizationConstants.Bearer, token);
        }

        /// <summary>
        /// SendPasswordResetTokenAsync
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<CheckingDataResponse> SendPasswordResetTokenAsync(string email)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            string url = $"{ApiRouteConstants.PrefixUrl}/{ApiRouteConstants.Auth}/{ApiRouteConstants.RequestResetPasswordToken}" +
                $"?{nameof(email)}={email}";
            var httpResponse = await _httpClient.GetAsync(url);

            var result = await httpResponse.Content.ReadAsStringAsync();
            return Generics.DeserializeJsonString<CheckingDataResponse>(result);
        }

        /// <summary>
        /// ResetPasswordAsync
        /// </summary>
        /// <param name="resetPasswordDTO"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PostPutResponse> ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            var httpResponse = await _httpClient.PutAsJsonAsync($"{ApiRouteConstants.PrefixUrl}/{ApiRouteConstants.Auth}/{ApiRouteConstants.Password}", resetPasswordDTO);
            var response = await httpResponse.Content.ReadFromJsonAsync<PostPutResponse>();
            return response!;
        }

        /// <summary>
        /// IsPasswordResetTokenAlive
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<CheckingDataResponse> IsPasswordResetTokenAlive(string userId, string token)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            string url = $"{ApiRouteConstants.PrefixUrl}/{ApiRouteConstants.Auth}/{ApiRouteConstants.IsPasswordResetTokenAlive}" +
                $"?{nameof(userId)}={userId}&{nameof(token)}={token}";
            var httpResponse = await _httpClient.GetAsync(url);

            var result = await httpResponse.Content.ReadAsStringAsync();
            return Generics.DeserializeJsonString<CheckingDataResponse>(result);
        }
    }
}
