using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using WorkManagermentWeb.Application.DTOs;
using WorkManagermentWeb.Application.GenericModels;
using WorkManagermentWeb.Client.Models;
using WorkManagermentWeb.Infrastructure.Models;
using WorkManagermentWeb.Infrastructure.Options;

namespace WorkManagermentWeb.Infrastructure.Services
{
    /// <summary>
    /// MicrosoftCalendarService
    /// </summary>
    public class MicrosoftCalendarService : IMicrosoftCalendarService
    {
        /// <summary>
        /// HttpClient
        /// </summary>
        private readonly HttpClient _httpClient;

        /// <summary>
        /// MicrosoftGraphOptions
        /// </summary>
        private readonly MicrosoftGraphOptions _graphOptions;

        /// <summary>
        /// MicrosoftCalendarService
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="graphOptions"></param>
        public MicrosoftCalendarService(
            HttpClient httpClient,
            IOptions<MicrosoftGraphOptions> graphOptions
        )
        {
            _httpClient = httpClient;
            _graphOptions = graphOptions.Value;
        }

        /// <summary>
        /// AddEventAsync
        /// </summary>
        /// <param name="calendarEvent"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<GraphEventResponse> AddEventAsync(CalendarEventDTO calendarEvent, string token)
        {
            ProcessHttpHeader(token);
            string eventAsJson = JsonConvert.SerializeObject(new MicrosoftGraphEvent
            {
                Subject = calendarEvent.Title,
                Start = new DateTimeTimeZone
                {
                    DateTime = calendarEvent.Start,
                    TimeZone = TimeZoneInfo.Local.Id
                },
                End = new DateTimeTimeZone
                {
                    DateTime = calendarEvent.End,
                    TimeZone = TimeZoneInfo.Local.Id,
                }
            });

            var content = new StringContent(eventAsJson);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var response = await _httpClient.PostAsync(_graphOptions.BaseApiUrl + _graphOptions.EventUrl, content);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Event has been added successfully!");
                var stringContent = await response.Content.ReadAsStringAsync();
                MicrosoftGraphEvent result = Generics.DeserializeJsonString<MicrosoftGraphEvent>(stringContent);
                return new GraphEventResponse(true, result.Id);
            }
            else
            {
                Console.WriteLine(response.StatusCode);
                return new GraphEventResponse(true, string.Empty);
            }
        }

        /// <summary>
        /// DeleteEventAsync
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<GraphEventResponse> DeleteEventAsync(string eventId, string token)
        {
            ProcessHttpHeader(token);
            string url = $"{_graphOptions.BaseApiUrl}{_graphOptions.EventUrl}/{eventId}";
            var response = await _httpClient.DeleteAsync(url).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                return new GraphEventResponse(true, eventId);
            }
            else
            {
                return new GraphEventResponse(false, eventId);
            }
        }

        /// <summary>
        /// ProcessHttpHeader
        /// </summary>
        /// <param name="token"></param>
        private void ProcessHttpHeader(string token)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", token);
        }
    }
}
