using Blazored.LocalStorage;
using WorkManagermentWeb.Application.Contracts;
using WorkManagermentWeb.Application.DTOs;
using WorkManagermentWeb.Application.DTOs.Responses;
using WorkManagermentWeb.Application.GenericModels;
using WorkManagermentWeb.Domain.Enums;
using WorkManagermentWeb.Application.Constants;
using System.Net.Http.Json;
using WorkManagermentWeb.Domain.Entities;

namespace WorkManagermentWeb.Client.Services
{
    /// <summary>
    /// WorkItemService
    /// </summary>
    public class WorkItemService : IWorkItem
    {
        /// <summary>
        /// WorkItemUrlPrefix
        /// </summary>
        private readonly string _workItemUrlPrefix;

        /// <summary>
        /// httpClient
        /// </summary>
        private readonly HttpClient _httpClient;

        /// <summary>
        /// ILocalStorageService
        /// </summary>
        private readonly ILocalStorageService _localStorageService;

        /// <summary>
        /// WorkItemService
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="localStorageService"></param>
        public WorkItemService(HttpClient httpClient, ILocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
            _workItemUrlPrefix = $"{ApiRouteConstants.PrefixUrl}/{ApiRouteConstants.WorkItem}";
        }

        /// <summary>
        /// AddAsync
        /// </summary>
        /// <param name="workItemDTO"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PostWorkItemResponse> AddAsync(PostPutWorkItemDTO workItemDTO)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            await AddTokenToHeaderAsync();
            var httpResponse = await _httpClient.PostAsJsonAsync($"{_workItemUrlPrefix}", workItemDTO);
            var response = await httpResponse.Content.ReadFromJsonAsync<PostWorkItemResponse>();
            return response!;
        }

        /// <summary>
        /// ChangeStatusAsync
        /// </summary>
        /// <param name="workItemId"></param>
        /// <param name="status"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PostWorkItemResponse> ChangeStatusAsync(Guid workItemId, WorkItemStatus status, string userId)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            await AddTokenToHeaderAsync();
            var httpResponse = await _httpClient.PutAsync(
                $"{_workItemUrlPrefix}/{ApiRouteConstants.ChangeStatus}/{workItemId}?{ApiRouteConstants.Status}={status}", null);
            var response = await httpResponse.Content.ReadFromJsonAsync<PostWorkItemResponse>();
            return response!;
        }

        /// <summary>
        /// DeleteAsync
        /// </summary>
        /// <param name="workItemId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PostWorkItemResponse> DeleteAsync(Guid workItemId, string userId)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            await AddTokenToHeaderAsync();
            var httpResponse = await _httpClient.DeleteAsync($"{_workItemUrlPrefix}/{workItemId}");
            var response = await httpResponse.Content.ReadFromJsonAsync<PostWorkItemResponse>();
            return response!;
        }

        /// <summary>
        /// GetByIdAsync
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<GetWorkItemResponse> GetByIdAsync(Guid id)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            await AddTokenToHeaderAsync();
            var httpResponse = await _httpClient.GetAsync($"{_workItemUrlPrefix}/{id}");

            //Read Response
            if (!httpResponse.IsSuccessStatusCode)
                return null!;

            var result = await httpResponse.Content.ReadAsStringAsync();
            return Generics.DeserializeJsonString<GetWorkItemResponse>(result);
        }

        /// <summary>
        /// GetListAsync
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<GetWorkItemsResponse> GetListAsync(WorkItemFilterDTO filter)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            await AddTokenToHeaderAsync();
            string url = $"{_workItemUrlPrefix}" +
                $"?{nameof(filter.BoardId)}={filter.BoardId}" +
                $"&{nameof(filter.Paging)}.{nameof(filter.Paging.Skip)}={filter.Paging.Skip}" +
                $"&{nameof(filter.Paging)}.{nameof(filter.Paging.Take)}={filter.Paging.Take}" +
                $"&{nameof(filter.Sorting)}.{nameof(filter.Sorting.SortBy)}={filter.Sorting!.SortBy}" +
                $"&{nameof(filter.Sorting)}.{nameof(filter.Sorting.IsAscending)}={filter.Sorting.IsAscending}";

            if (filter.StartDate != null)
            {
                url += $"&{nameof(filter.StartDate)}={filter.StartDate}";
            }
            if (filter.EndDate != null)
            {
                url += $"&{nameof(filter.EndDate)}={filter.EndDate}";
            }
            if (filter.Status != null)
            {
                url += $"&{nameof(filter.Status)}={filter.Status}";
            }
            if (filter.Type != null)
            {
                url += $"&{nameof(filter.Type)}={filter.Type}";
            }
            if (filter.Priority != null)
            {
                url += $"&{nameof(filter.Priority)}={filter.Priority}";
            }
            if (filter.CreatedBy != null)
            {
                url += $"&{nameof(filter.CreatedBy)}={filter.CreatedBy}";
            }
            if (filter.TitleOrDescription != null)
            {
                url += $"&{nameof(filter.TitleOrDescription)}={filter.TitleOrDescription}";
            }
            if (filter.OwnerId != null)
            {
                url += $"&{nameof(filter.OwnerId)}={filter.OwnerId}";
            }
            if (filter.Code != null)
            {
                url += $"&{nameof(filter.Code)}={filter.Code}";
            }
            if (filter.IsSyncToCalendar != null)
            {
                url += $"&{nameof(filter.IsSyncToCalendar)}={filter.IsSyncToCalendar}";
            }
            var httpResponse = await _httpClient.GetAsync(url.ToLower());
            //Read Response
            if (!httpResponse.IsSuccessStatusCode)
                return null!;

            var result = await httpResponse.Content.ReadAsStringAsync();
            return Generics.DeserializeJsonString<GetWorkItemsResponse>(result);
        }

        /// <summary>
        /// UpdateAsync
        /// </summary>
        /// <param name="workItemDTO"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PostWorkItemResponse> UpdateAsync(PostPutWorkItemDTO workItemDTO)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            await AddTokenToHeaderAsync();
            var httpResponse = await _httpClient.PutAsJsonAsync($"{_workItemUrlPrefix}", workItemDTO);
            var response = await httpResponse.Content.ReadFromJsonAsync<PostWorkItemResponse>();
            return response!;
        }

        /// <summary>
        /// MarkAsSyncToCalendar
        /// </summary>
        /// <param name="markAsSyncToCalendarDTO"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PostPutResponse> MarkAsSyncToCalendar(MarkAsSyncToCalendarDTO markAsSyncToCalendarDTO)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            await AddTokenToHeaderAsync();
            var httpResponse = await _httpClient.PutAsJsonAsync($"{_workItemUrlPrefix}/{ApiRouteConstants.MarkAsSyncToCalendar}", markAsSyncToCalendarDTO);
            var response = await httpResponse.Content.ReadFromJsonAsync<PostPutResponse>();
            return response!;
        }

        /// <summary>
        /// GetWorkItemSpecicalPropertiesInfoResponse
        /// </summary>
        /// <returns></returns>
        public GetPropertiesInfoResponse GetSpecicalPropertiesInfo()
        {
            return new GetPropertiesInfoResponse();
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
        /// QueryAllNearingDuesAsync
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IQueryable<WorkItem> QueryAllNearingDues()
        {
            throw new NotImplementedException();
        }
    }
}
