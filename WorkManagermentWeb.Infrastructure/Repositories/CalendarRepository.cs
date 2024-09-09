using Microsoft.EntityFrameworkCore;
using System.Globalization;
using WorkManagermentWeb.Application.Contracts;
using WorkManagermentWeb.Application.DTOs;
using WorkManagermentWeb.Application.DTOs.Responses;
using WorkManagermentWeb.Domain.Entities;
using WorkManagermentWeb.Infrastructure.Data;
using WorkManagermentWeb.Infrastructure.Services;

namespace WorkManagermentWeb.Infrastructure.Repositories
{
    /// <summary>
    /// CalendarRepository
    /// </summary>
    public class CalendarRepository : ICalendar
    {
        /// <summary>
        /// IWorkItem
        /// </summary>
        private readonly IWorkItem _workItem;

        /// <summary>
        /// IWorkItem
        /// </summary>
        private readonly AppDBContext _context;

        /// <summary>
        /// IMicrosoftCalendarService
        /// </summary>
        private readonly IMicrosoftCalendarService _microsoftCalendarService;

        /// <summary>
        /// CalendarRepository
        /// </summary>
        /// <param name="workItem"></param>
        /// <param name="context"></param>
        /// <param name="microsoftCalendarService"></param>
        public CalendarRepository(IWorkItem workItem, AppDBContext context, IMicrosoftCalendarService microsoftCalendarService)
        {
            _workItem = workItem;
            _context = context;
            _microsoftCalendarService = microsoftCalendarService;
        }

        /// <summary>
        /// GetEventsByUser
        /// </summary>
        /// <param name="getEventsDTO"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<EventsResponse> GetEventsByUser(GetEventsDTO getEventsDTO)
        {
            GetWorkItemsResponse response = await _workItem.GetListAsync(new WorkItemFilterDTO()
            {
                OwnerId = getEventsDTO.UserId,
                Paging = new PagingDTO()
                {
                    Skip = 0,
                    Take = int.MaxValue
                }
            }).ConfigureAwait(false);
            List<CalendarEventDTO> events = response.WorkItems.Select(_ => new CalendarEventDTO()
            {
                Title = $"#{_.Code} - {_.Title}",
                Start = _.StartDate!.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)!,
                End = _.EndDate!.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)!,
                Url = $"/task/{_.Id}"
            }).ToList();

            return new EventsResponse(events);
        }

        /// <summary>
        /// SyncEvents
        /// </summary>
        /// <param name="syncEventDTO"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PostPutResponse> SyncEvents(SyncEventsDTO syncEventDTO)
        {
            await RemoveEvents(syncEventDTO.UserId, syncEventDTO.GraphToken);
            await AddEvents(syncEventDTO.UserId, syncEventDTO.GraphToken);
            return new PostPutResponse(true);
        }

        /// <summary>
        /// RemoveEvents
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private async Task RemoveEvents(string userId, string token)
        {
            List<WorkItem> workItems = await _context.WorkItems.Include(_ => _.CalendarEvent)
                .Where(_ => _.AssigneeId == userId && _.CalendarEvent != null && _.CalendarEvent.IsSynced)
                .ToListAsync();

            List<string> eventIds = workItems.Select(_ => _.CalendarEvent.Id).ToList();
            int maxConcurrency = 3;
            var messages = new List<string>();
            var semaphore = new SemaphoreSlim(maxConcurrency);
            var tasks = new List<Task>();
            foreach (var id in eventIds)
            {
                tasks.Add(Task.Run(async () =>
                {
                    await semaphore.WaitAsync();
                    try
                    {
                        await _microsoftCalendarService.DeleteEventAsync(id, token);
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                }));
            }
            await Task.WhenAll(tasks);

            _context.CalendarEvents.RemoveRange(workItems.Select(_ => _.CalendarEvent).ToList());
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// AddEvents
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private async Task AddEvents(string userId, string token)
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

            List<WorkItem> workItems = await _context.WorkItems.Include(_ => _.CalendarEvent)
                .Where(_ => _.AssigneeId == userId)
                .ToListAsync();

            List<CalendarEventDTO> calendarEvents = workItems.Select(_ => new CalendarEventDTO()
            {
                WorkItemId = _.Id,
                Title = $"#{_.Code}-{_.Title}",
                Start = _.StartDate!.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)!,
                End = _.EndDate!.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)!
            }).ToList();

            List<CalendarEvent> trackingEvents = new List<CalendarEvent>();
            int maxConcurrency = 3;
            var messages = new List<string>();
            var semaphore = new SemaphoreSlim(maxConcurrency);
            var tasks = new List<Task>();
            foreach (var @event in calendarEvents)
            {
                tasks.Add(Task.Run(async () =>
                {
                    await semaphore.WaitAsync();
                    try
                    {
                        var response = await _microsoftCalendarService.AddEventAsync(@event, token);

                        trackingEvents.Add(new CalendarEvent()
                        {
                            Id = response.EventId,
                            WorkItemId = @event.WorkItemId,
                            IsSynced = true
                        });
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                }));
            }
            await Task.WhenAll(tasks);

            _context.CalendarEvents.AddRange(trackingEvents);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// AddEvent
        /// </summary>
        /// <param name="@event"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<PostPutResponse> SyncEvent(CalendarEventDTO @event, string token)
        {
            var response = await _microsoftCalendarService.AddEventAsync(@event, token);

            CalendarEvent calendarEvent = new CalendarEvent()
            {
                Id = response.EventId,
                WorkItemId = @event.WorkItemId,
            };
            _context.CalendarEvents.Add(calendarEvent);
            await _context.SaveChangesAsync();
            return new PostPutResponse(true);
        }
    }
}
