using Blazored.LocalStorage;
using WorkManagermentWeb.Application.Constants;
using WorkManagermentWeb.Application.Contracts;
using WorkManagermentWeb.Application.DTOs.Responses;
using WorkManagermentWeb.Application.GenericModels;

namespace WorkManagermentWeb.Client.Services
{
    /// <summary>
    /// GanttChartService
    /// </summary>
    public class GanttChartService : IGanttChart
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
        /// GanttChartService
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="localStorageService"></param>
        public GanttChartService(HttpClient httpClient, ILocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
            _urlPrefix = $"{ApiRouteConstants.PrefixUrl}/{ApiRouteConstants.GanttChart}";
        }

        /// <summary>
        /// GetDataAsync
        /// </summary>
        /// <param name="boardIds"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public async Task<GanttChartResponse> GetDataAsync(List<Guid> boardIds, string culture)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            await AddTokenToHeaderAsync();
            string url = $"{_urlPrefix}?{nameof(culture)}={culture}";
            if (boardIds is not null && boardIds.Count > 0)
            {
                url += "&";
                foreach (var boardId in boardIds)
                {
                    url += $"ids={boardId}&";
                }
            }
            var httpResponse = await _httpClient.GetAsync(url.Remove(url.Length));
            //Read Response
            if (!httpResponse.IsSuccessStatusCode)
                return null!;

            var result = await httpResponse.Content.ReadAsStringAsync();
            return Generics.DeserializeJsonString<GanttChartResponse>(result);
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
