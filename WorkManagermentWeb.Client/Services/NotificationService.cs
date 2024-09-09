using Blazored.LocalStorage;
using System.Net.Http;
using System.Net.Http.Json;
using WorkManagermentWeb.Application.Constants;
using WorkManagermentWeb.Application.Contracts;
using WorkManagermentWeb.Application.DTOs;
using WorkManagermentWeb.Application.DTOs.Responses;
using WorkManagermentWeb.Application.GenericModels;

namespace WorkManagermentWeb.Client.Services
{
    /// <summary>
    /// NotificationService
    /// </summary>
    public class NotificationService : INotification
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
        /// _urlPrefix
        /// </summary>
        private readonly string _urlPrefix;

        /// <summary>
        /// NotificationService
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="localStorageService"></param>
        public NotificationService(HttpClient httpClient, ILocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
            _urlPrefix = $"{ApiRouteConstants.PrefixUrl}/{ApiRouteConstants.Notification}";
        }

        /// <summary>
        /// GetListAsync
        /// </summary>
        /// <param name="getListNotiDTO"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<GetListNotiResponse> GetListAsync(GetListNotiDTO getListNotiDTO)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            await AddTokenToHeaderAsync();
            AddLanguageToHeader(getListNotiDTO.Language);
            string url = $"{_urlPrefix}" +
                $"?{nameof(getListNotiDTO.Paging.Skip)}={getListNotiDTO.Paging.Skip}" +
                $"&{nameof(getListNotiDTO.Paging.Take)}={getListNotiDTO.Paging.Take}";

            var httpResponse = await _httpClient.GetAsync(url.ToLower());
            //Read Response
            if (!httpResponse.IsSuccessStatusCode)
                return null!;

            var result = await httpResponse.Content.ReadAsStringAsync();
            return Generics.DeserializeJsonString<GetListNotiResponse>(result);
        }

        /// <summary>
        /// MarkAllAsReadAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<MarkAllAsReadResponse> MarkAllAsReadAsync(string userId)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            await AddTokenToHeaderAsync();
            var httpResponse = await _httpClient.PutAsync($"{_urlPrefix}/{ApiRouteConstants.MarkAsRead}", null);
            var response = await httpResponse.Content.ReadFromJsonAsync<MarkAllAsReadResponse>();
            return response!;
        }

        /// <summary>
        /// MarkAsReadAsync
        /// </summary>
        /// <param name="notificationId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<MarkAsReadResponse> MarkAsReadAsync(Guid notificationId)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            await AddTokenToHeaderAsync();
            var httpResponse = await _httpClient.PutAsync($"{_urlPrefix}/{notificationId}?action={HttpConstants.MarkAsReadAction}", null);
            var response = await httpResponse.Content.ReadFromJsonAsync<MarkAsReadResponse>();
            return response!;
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
        /// AddLanguageToHeader
        /// </summary>
        /// <param name="lang"></param>
        private void AddLanguageToHeader(string lang)
        {
            _httpClient.DefaultRequestHeaders.Add(nameof(GetListNotiDTO.Language).ToLower(), lang);
        }
    }
}
