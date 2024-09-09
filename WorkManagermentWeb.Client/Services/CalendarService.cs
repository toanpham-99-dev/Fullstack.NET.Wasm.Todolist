using Blazored.LocalStorage;
using System.Net.Http.Json;
using WorkManagermentWeb.Application.Constants;
using WorkManagermentWeb.Application.Contracts;
using WorkManagermentWeb.Application.DTOs;
using WorkManagermentWeb.Application.DTOs.Responses;
using WorkManagermentWeb.Application.GenericModels;

namespace WorkManagermentWeb.Client.Services
{
    /// <summary>
    /// CalendarService
    /// </summary>
    public class CalendarService : ICalendar
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
        /// CalendarService
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="localStorageService"></param>
        public CalendarService(HttpClient httpClient, ILocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
            _urlPrefix = $"{ApiRouteConstants.PrefixUrl}/{ApiRouteConstants.Calendar}";
        }

        /// <summary>
        /// GetEventsByUser
        /// </summary>
        /// <param name="getEventsDTO"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<EventsResponse> GetEventsByUser(GetEventsDTO getEventsDTO)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            await AddTokenToHeaderAsync();
            string url = $"{_urlPrefix}/{ApiRouteConstants.CalendarEvents}";

            var httpResponse = await _httpClient.GetAsync(url.ToLower());
            //Read Response
            if (!httpResponse.IsSuccessStatusCode)
                return null!;

            var result = await httpResponse.Content.ReadAsStringAsync();
            return Generics.DeserializeJsonString<EventsResponse>(result);
        }

        /// <summary>
        /// SyncEvent
        /// </summary>
        /// <param name="event"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PostPutResponse> SyncEvent(CalendarEventDTO @event, string token)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            await AddTokenToHeaderAsync();
            SyncEventDTO syncEventDTO = new SyncEventDTO()
            {
                CalendarEventDTO = @event,
                Token = token
            };
            var httpResponse = await _httpClient.PostAsJsonAsync($"{_urlPrefix}/{ApiRouteConstants.CalendarEvent}", syncEventDTO);
            var response = await httpResponse.Content.ReadFromJsonAsync<PostPutResponse>();
            return response!;
        }

        /// <summary>
        /// SyncEvents
        /// </summary>
        /// <param name="syncEventDTO"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PostPutResponse> SyncEvents(SyncEventsDTO syncEventDTO)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            await AddTokenToHeaderAsync();
            var httpResponse = await _httpClient.PostAsJsonAsync($"{_urlPrefix}/{ApiRouteConstants.CalendarEvents}", syncEventDTO);
            var response = await httpResponse.Content.ReadFromJsonAsync<PostPutResponse>();
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
    }
}
