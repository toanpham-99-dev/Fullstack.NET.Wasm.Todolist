using Blazored.LocalStorage;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;
using WorkManagermentWeb.Application.Constants;
using WorkManagermentWeb.Application.Contracts;
using WorkManagermentWeb.Application.DTOs;
using WorkManagermentWeb.Application.DTOs.Responses;
using WorkManagermentWeb.Application.GenericModels;

namespace WorkManagermentWeb.Client.Services
{
    public class WorkSpaceService : IWorkSpace
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
        /// WorkSpaceService
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="localStorageService"></param>
        public WorkSpaceService(HttpClient httpClient, ILocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
            _urlPrefix = $"{ApiRouteConstants.PrefixUrl}/{ApiRouteConstants.WorkSpace}";
        }

        /// <summary>
        /// AddAsync
        /// </summary>
        /// <param name="workSpaceDTO"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PostPutResponse> AddAsync(WorkSpaceDTO workSpaceDTO)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            await AddTokenToHeaderAsync();
            var httpResponse = await _httpClient.PostAsJsonAsync($"{_urlPrefix}", workSpaceDTO);
            var response = await httpResponse.Content.ReadFromJsonAsync<PostPutResponse>();
            return response!;
        }

        /// <summary>
        /// AddMemberAsync
        /// </summary>
        /// <param name="workSpaceId"></param>
        /// <param name="memberId"></param>
        /// <param name="updaterId"></param>
        /// <returns></returns>
        public async Task<PostPutResponse> AddMemberAsync(Guid workSpaceId, string memberId, string updaterId)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            await AddTokenToHeaderAsync();
            WorkSpaceMemberDTO memberDTO = new WorkSpaceMemberDTO
            {
                MemberId = memberId,
                UpdaterId = updaterId
            };
            var httpContent = new StringContent(JsonConvert.SerializeObject(memberDTO), Encoding.UTF8, "application/json");

            var httpResponse = await _httpClient.PostAsync(
                $"{_urlPrefix}/{workSpaceId}/{ApiRouteConstants.Members}", httpContent);
            var response = await httpResponse.Content.ReadFromJsonAsync<PostPutResponse>();
            return response!;
        }

        /// <summary>
        /// RemoveMemberAsync
        /// </summary>
        /// <param name="workSpaceId"></param>
        /// <param name="memberId"></param>
        /// <param name="updaterId"></param>
        /// <returns></returns>
        public async Task<PostPutResponse> RemoveMemberAsync(Guid workSpaceId, string memberId, string updaterId)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            await AddTokenToHeaderAsync();

            var httpResponse = await _httpClient.DeleteAsync(
                $"{_urlPrefix}/{workSpaceId}/{ApiRouteConstants.Members}?{nameof(BoardMemberDTO.MemberId)}={memberId}");
            var response = await httpResponse.Content.ReadFromJsonAsync<PostPutResponse>();
            return response!;
        }

        /// <summary>
        /// GetByIdAsync
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<GetWorkspaceResponse> GetByIdAsync(Guid id)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            await AddTokenToHeaderAsync();
            var httpResponse = await _httpClient.GetAsync($"{_urlPrefix}/{id}");

            //Read Response
            if (!httpResponse.IsSuccessStatusCode)
                return null!;

            var result = await httpResponse.Content.ReadAsStringAsync();
            return Generics.DeserializeJsonString<GetWorkspaceResponse>(result);
        }

        /// <summary>
        /// GetListAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<GetWorkspacesResponse> GetListAsync(string userId)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            await AddTokenToHeaderAsync();
            string url = $"{_urlPrefix}";

            var httpResponse = await _httpClient.GetAsync(url.ToLower());
            //Read Response
            if (!httpResponse.IsSuccessStatusCode)
                return null!;

            var result = await httpResponse.Content.ReadAsStringAsync();
            return Generics.DeserializeJsonString<GetWorkspacesResponse>(result);
        }

        /// <summary>
        /// UpdateAsync
        /// </summary>
        /// <param name="workSpaceDTO"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PostPutResponse> UpdateAsync(WorkSpaceDTO workSpaceDTO)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            await AddTokenToHeaderAsync();
            var httpResponse = await _httpClient.PutAsJsonAsync($"{_urlPrefix}", workSpaceDTO);
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
