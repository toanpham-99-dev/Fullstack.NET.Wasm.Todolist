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
    public class BoardService : IBoard
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
        /// BoardService
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="localStorageService"></param>
        public BoardService(HttpClient httpClient, ILocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
            _urlPrefix = $"{ApiRouteConstants.PrefixUrl}/{ApiRouteConstants.Board}";
        }

        /// <summary>
        /// AddAsync
        /// </summary>
        /// <param name="boardDTO"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PostPutResponse> AddAsync(PostPutBoardDTO boardDTO)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            await AddTokenToHeaderAsync();
            var httpResponse = await _httpClient.PostAsJsonAsync($"{_urlPrefix}", boardDTO);
            var response = await httpResponse.Content.ReadFromJsonAsync<PostPutResponse>();
            return response!;
        }

        /// <summary>
        /// AddMemberAsync
        /// </summary>
        /// <param name="memberDTO"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PostPutResponse> AddMemberAsync(BoardMemberDTO memberDTO, Guid id)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            await AddTokenToHeaderAsync();
            var httpContent = new StringContent(JsonConvert.SerializeObject(memberDTO), Encoding.UTF8, "application/json");

            var httpResponse = await _httpClient.PostAsync(
                $"{_urlPrefix}/{id}/{ApiRouteConstants.Members}", httpContent);
            var response = await httpResponse.Content.ReadFromJsonAsync<PostPutResponse>();
            return response!;
        }

        /// <summary>
        /// RemoveMemberAsync
        /// </summary>
        /// <param name="memberDTO"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PostPutResponse> RemoveMemberAsync(BoardMemberDTO memberDTO, Guid id)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            await AddTokenToHeaderAsync();

            var httpResponse = await _httpClient.DeleteAsync(
                $"{_urlPrefix}/{id}/{ApiRouteConstants.Members}?{nameof(BoardMemberDTO.MemberId)}={memberDTO.MemberId}");
            var response = await httpResponse.Content.ReadFromJsonAsync<PostPutResponse>();
            return response!;
        }

        /// <summary>
        /// GetByIdAsync
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<GetBoardResponse> GetByIdAsync(Guid id)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            await AddTokenToHeaderAsync();
            var httpResponse = await _httpClient.GetAsync($"{_urlPrefix}/{id}");

            //Read Response
            if (!httpResponse.IsSuccessStatusCode)
                return null!;

            var result = await httpResponse.Content.ReadAsStringAsync();
            return Generics.DeserializeJsonString<GetBoardResponse>(result);
        }

        /// <summary>
        /// GetListAsync
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<GetBoardsResponse> GetListAsync(GetBoardsDTO filter)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            await AddTokenToHeaderAsync();
            string url = $"{_urlPrefix}";

            var httpResponse = await _httpClient.GetAsync(url.ToLower());
            //Read Response
            if (!httpResponse.IsSuccessStatusCode)
                return null!;

            var result = await httpResponse.Content.ReadAsStringAsync();
            return Generics.DeserializeJsonString<GetBoardsResponse>(result);
        }

        /// <summary>
        /// UpdateAsync
        /// </summary>
        /// <param name="boardDTO"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PostPutResponse> UpdateAsync(PostPutBoardDTO boardDTO)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            await AddTokenToHeaderAsync();

            var httpResponse = await _httpClient.PutAsJsonAsync($"{_urlPrefix}", boardDTO);
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
