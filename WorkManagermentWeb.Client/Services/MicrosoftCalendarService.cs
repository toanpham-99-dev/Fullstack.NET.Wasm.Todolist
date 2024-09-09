using Blazored.LocalStorage;
using Newtonsoft.Json;
using System.Globalization;
using WorkManagermentWeb.Application.Constants;
using WorkManagermentWeb.Application.Contracts;
using WorkManagermentWeb.Application.DTOs;
using WorkManagermentWeb.Application.DTOs.Responses;
using WorkManagermentWeb.Client.Interfaces;
using WorkManagermentWeb.Client.Models;
using WorkManagermentWeb.Client.Options;

namespace WorkManagermentWeb.Client.Services
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
        /// ILocalStorageService
        /// </summary>
        private readonly ILocalStorageService _localStorageService;

        /// <summary>
        /// MicrosoftGraphOptions
        /// </summary>
        private readonly MicrosoftGraphOptions _microsoftGraphOptions;

        /// <summary>
        /// IWorkItem
        /// </summary>
        private readonly IWorkItem _workItem;

        /// <summary>
        /// MicrosoftCalendarService
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="localStorageService"></param>
        /// <param name="configuration"></param>
        public MicrosoftCalendarService(
            HttpClient httpClient,
            ILocalStorageService localStorageService,
            IConfiguration configuration,
            IWorkItem workItem)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
            _workItem = workItem;
            _microsoftGraphOptions = new MicrosoftGraphOptions()
            {
                BaseApiUrl = configuration[$"{nameof(MicrosoftGraphOptions)}:{nameof(MicrosoftGraphOptions.BaseApiUrl)}"]!,
                GetAllEventsUrl = configuration[$"{nameof(MicrosoftGraphOptions)}:{nameof(MicrosoftGraphOptions.GetAllEventsUrl)}"]!,
                GetEventsUrl = configuration[$"{nameof(MicrosoftGraphOptions)}:{nameof(MicrosoftGraphOptions.GetEventsUrl)}"]!,
                EventUrl = configuration[$"{nameof(MicrosoftGraphOptions)}:{nameof(MicrosoftGraphOptions.EventUrl)}"]!
            };
        }

        /// <summary>
        /// CheckExternalAuthState
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> CheckExternalAuthState()
        {
            string? graphToken = await GetAccessTokenAsync();
            if (string.IsNullOrEmpty(graphToken))
            {
                return false;
            }
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", graphToken);

            var response = await _httpClient.GetAsync(ConstructGraphUrl(2000, 1));

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// AddEventAsync
        /// </summary>
        /// <param name="calendarEvent"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task AddEventAsync(Application.DTOs.CalendarEventDTO calendarEvent)
        {
            var accessToken = await GetAccessTokenAsync();
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", accessToken);

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

            var response = await _httpClient.PostAsync(_microsoftGraphOptions.BaseApiUrl + _microsoftGraphOptions.EventUrl, content);

            if (response.IsSuccessStatusCode)
                Console.WriteLine("Event has been added successfully!");
            else
                Console.WriteLine(response.StatusCode);
        }

        /// <summary>
        /// GetEvents
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<GetMSEventsResponse> GetEvents(int? year, int? month)
        {
            var accessToken = await GetAccessTokenAsync();
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", accessToken);

            var response = await _httpClient.GetAsync(ConstructGraphUrl(year, month));

            if (!response.IsSuccessStatusCode)
            {
                return new GetMSEventsResponse(false, default!);
            }

            var contentAsString = await response.Content.ReadAsStringAsync();

            var microsoftEvents = JsonConvert.DeserializeObject<GraphEventsResponse>(contentAsString);

            var events = new List<Application.DTOs.CalendarEventDTO>();
            foreach (var item in microsoftEvents!.Value)
            {
                events.Add(new Application.DTOs.CalendarEventDTO
                {
                    Title = string.IsNullOrEmpty(item.Subject) ? string.Empty : item.Subject,
                    Start = item.Start.ConvertToLocalDateTime().ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    End = item.End.ConvertToLocalDateTime().ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
                });
            }
            return new GetMSEventsResponse(true, events);
        }

        /// <summary>
        /// SyncEvents
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task SyncEvents(string userId)
        {
            GetWorkItemsResponse response = await _workItem.GetListAsync(new WorkItemFilterDTO()
            {
                OwnerId = userId,
                IsSyncToCalendar = false,
                Paging = new PagingDTO()
                {
                    Skip = 0,
                    Take = int.MaxValue
                }
            }).ConfigureAwait(false);

            List<CalendarEventDTO> calendarEvents = response.WorkItems.Select(_ => new CalendarEventDTO()
            {
                Title = $"#{_.Code}-{_.Title}",
                Start = _.StartDate!.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)!,
                End = _.EndDate!.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)!
            }).ToList();

            await _workItem.MarkAsSyncToCalendar(new MarkAsSyncToCalendarDTO()
            {
                Ids = response.WorkItems.Select(_ => _.Id).ToList(),
            }).ConfigureAwait(false);

            int maxConcurrency = 3;
            var messages = new List<string>();
            var semaphore = new SemaphoreSlim(maxConcurrency);
            var tasks = new List<Task>();
            foreach (var item in calendarEvents)
            {
                tasks.Add(Task.Run(async () =>
                {
                    await semaphore.WaitAsync();
                    try
                    {
                        await AddEventAsync(item);
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                }));
            }
            await Task.WhenAll(tasks);
        }

        /// <summary>
        /// GetAccessTokenAsync
        /// </summary>
        /// <returns></returns>
        private async Task<string> GetAccessTokenAsync()
        {
            return (await _localStorageService.GetItemAsStringAsync(AuthorizationConstants.GraphToken))!;
        }

        /// <summary>
        /// ConstructGraphUrl
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        private string ConstructGraphUrl(int? year, int? month)
        {
            if (year is null && month is null)
            {
                return $"{_microsoftGraphOptions.BaseApiUrl}{_microsoftGraphOptions.GetAllEventsUrl}";
            }
            var daysInMonth = DateTime.DaysInMonth(year!.Value, month!.Value);
            string start = $"{year}-{month}-1";
            string end = $"{year}-{month}-{daysInMonth}";
            return $"{_microsoftGraphOptions.BaseApiUrl}{String.Format(_microsoftGraphOptions.GetEventsUrl, start, end)}";
        }
    }
}
