using WorkManagermentWeb.Application.Contracts;
using WorkManagermentWeb.Application.DTOs;
using WorkManagermentWeb.Application.DTOs.Responses;
using WorkManagermentWeb.Domain.Enums;
using WorkManagermentWeb.Domain.Entities;
using WorkManagermentWeb.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using WorkManagermentWeb.Application.Constants;
using WorkManagermentWeb.Application.Utilities;
using WorkManagermentWeb.EventHandler.Events;
using WorkManagermentWeb.EventHandler.Services;
using System.Globalization;

namespace WorkManagermentWeb.Infrastructure.Repositories
{
    /// <summary>
    /// WorkItemRepository
    /// </summary>
    /// <param name="context"></param>
    /// <param name="eventPusher"></param>
    public class WorkItemRepository(AppDBContext context, IEventPusher eventPusher) : IWorkItem
    {
        /// <summary>
        /// AddAsync
        /// </summary>
        /// <param name="workItemDTO"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PostWorkItemResponse> AddAsync(PostPutWorkItemDTO workItemDTO)
        {
            Guid foundParentId = Guid.Empty;
            if (workItemDTO.ParentWorkItemCode is not null)
            {
                WorkItem? foundParent = await GetByCodeAsync(workItemDTO.ParentWorkItemCode.Value);
                if (foundParent is null)
                {
                    return new PostWorkItemResponse(false, HttpConstants.ParentWorkItemNotFound);
                }
                foundParentId = foundParent.Id;
            }

            ApplicationUser? foundCreator = await GetUserByIdAsync(workItemDTO.CreatorId);
            ApplicationUser assignee = foundCreator!;
            if (!string.IsNullOrEmpty(workItemDTO.AssigneeId))
            {
                assignee = (await GetUserByIdAsync(workItemDTO.AssigneeId!))!;
            }
            Guid id = Guid.NewGuid();
            WorkItem newWorkItem = new WorkItem()
            {
                Id = id,
                Title = workItemDTO.Title,
                Description = workItemDTO.Description,
                Type = workItemDTO.Type,
                CreatorId = workItemDTO.CreatorId,
                CreatorName = foundCreator?.UserName,
                LastUpdaterId = workItemDTO.LastUpdaterId,
                LastUpdaterName = foundCreator?.UserName,
                AssigneeId = workItemDTO.AssigneeId,
                AssigneeName = workItemDTO.AssigneeName,
                BoardId = workItemDTO.BoardId,
                Priority = workItemDTO.Priority,
                ParentWorkItemId = workItemDTO.ParentWorkItemCode is null ? id : foundParentId,
                StartDate = workItemDTO.StartDate!.Value.ToDateOnly(TimeZoneInfo.Utc),
                EndDate = workItemDTO.EndDate!.Value.ToDateOnly(TimeZoneInfo.Utc),
                WorkRemain = double.Parse(workItemDTO.WorkRemain, CultureInfo.InvariantCulture),
                User = assignee
            };
            context.WorkItems.Add(newWorkItem);
            await context.SaveChangesAsync();
            await eventPusher.Publish(new WorkItemCreate
            {
                ObjectId = newWorkItem.Id.ToString(),
                WorkItemCode = newWorkItem.Code.ToString(),
                WorkItemTitle = newWorkItem.Title,
                CreatorName = foundCreator!.FullName,
                RecieverId = assignee.Id
            });
            return new PostWorkItemResponse(true, HttpConstants.WorkItemCreatedSuccessfully, id);
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
            WorkItem? workItem = await GetRecordByIdAsync(workItemId);
            if (workItem is null)
            {
                return new PostWorkItemResponse(false, HttpConstants.WorkItemNotFound);
            }

            if (workItem.Status == (int)status)
            {
                return new PostWorkItemResponse(true);
            }

            ApplicationUser? foundUpdater = await GetUserByIdAsync(userId);

            //Check all sub tasks done or not before change to resovled
            if (status == WorkItemStatus.Resolved)
            {
                List<WorkItem> subWorkItems = workItem.SubWorkItems.ToList();
                if (subWorkItems.Count > 0 && subWorkItems.Any(_ => _.Status != (int)WorkItemStatus.Done))
                {
                    return new PostWorkItemResponse(false, HttpConstants.WorkItemContainUnDoneTasks);
                }
            }

            //If previous status is not resolved, cant change this task to done
            if (status == WorkItemStatus.Done && workItem.Status != (int)WorkItemStatus.Resolved)
            {
                return new PostWorkItemResponse(false, HttpConstants.WorkItemMustBeResolvedBeforeChangeToDone);
            }

            workItem.Status = (int)status;
            workItem.UpdatedAt = DateTime.UtcNow;
            workItem.LastUpdaterId = userId;
            workItem.LastUpdaterName = foundUpdater?.UserName;
            context.WorkItems.Update(workItem);
            await context.SaveChangesAsync();
            return new PostWorkItemResponse(true);
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
            WorkItem? workItem = await GetRecordByIdAsync(workItemId);
            if (workItem is null)
            {
                return new PostWorkItemResponse(false, HttpConstants.WorkItemNotFound);
            }

            if (workItem.CreatorId != userId)
            {
                return new PostWorkItemResponse(false, HttpConstants.MustBeThisWorkItemCreatorToDelete);
            }
            workItem.IsDeleted = true;
            context.WorkItems.Update(workItem);
            await context.SaveChangesAsync();
            return new PostWorkItemResponse(true);
        }

        /// <summary>
        /// GetByIdAsync
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<GetWorkItemResponse> GetByIdAsync(Guid id)
        {
            WorkItem? workItem = await GetRecordByIdAsync(id);
            if (workItem is null)
            {
                return new GetWorkItemResponse(false, HttpConstants.WorkItemNotFound);
            }
            double processPercent = Helper.CalculateProcessPercent(workItem.StartDate, workItem.EndDate, (WorkItemStatus)workItem.Status);
            return new GetWorkItemResponse(true, string.Empty, new WorkItemDTO
            {
                Id = id,
                Code = workItem.Code,
                Title = workItem.Title,
                Description = workItem.Description,
                Type = workItem.Type,
                LastUpdaterId = workItem.LastUpdaterId,
                LastUpdaterName = workItem.LastUpdaterName,
                CreatorId = workItem.CreatorId,
                CreatorName = workItem.CreatorName,
                Status = (WorkItemStatus)workItem.Status,
                AssigneeId = workItem.AssigneeId,
                AssigneeName = workItem.AssigneeName,
                BoardId = workItem.BoardId,
                Priority = workItem.Priority,
                ParentWorkItemId = workItem.ParentWorkItemId,
                ParentWorkItemCode = workItem.ParentWorkItemId != id ? workItem.ParentWorkItem.Code : null,
                CreatedDate = workItem.CreatedAt.ConvertToTimeZonePlus7(),
                LastUpdatedDate = workItem.UpdatedAt.ConvertToTimeZonePlus7(),
                StartDate = workItem.StartDate,
                EndDate = workItem.EndDate,
                WorkRemain = workItem.WorkRemain,
                ProcessPercent = processPercent,
                IsSyncToCalendar = workItem.CalendarEvent is not null && workItem.CalendarEvent.IsSynced ? true : false,
                SubWorkItems = workItem.SubWorkItems
                    .Where(_ => _.ParentWorkItemId != _.Id)
                    .OrderByDescending(_ => _.Code)
                    .Select(_ => new SubWorkItem
                    {
                        Id = _.Id,
                        Code = _.Code,
                        Title = _.Title,
                        Status = (WorkItemStatus)_.Status,
                        StartDate = _.StartDate,
                        EndDate = _.EndDate,
                        AssigneeId = _.AssigneeId,
                        AssigneeName = _.AssigneeName
                    }).ToList()
            });
        }

        /// <summary>
        /// GetListAsync
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<GetWorkItemsResponse> GetListAsync(WorkItemFilterDTO filter)
        {
            IQueryable<WorkItem> query = context.WorkItems
                .Where(_ => !_.IsDeleted)
                .Include(_ => _.SubWorkItems.OrderByDescending(_ => _.Code))
                .Include(_ => _.ParentWorkItem)
                .Include(_ => _.CalendarEvent)
                .AsQueryable();

            if (filter.BoardId != Guid.Empty)
            {
                query = query.Where(_ => _.BoardId == filter.BoardId);
            }
            if (filter.StartDate != null)
            {
                query = query.Where(_ => _.StartDate != null && _.StartDate.Value.CompareTo(filter.StartDate.Value) == 0);
            }
            if (filter.EndDate != null)
            {
                query = query.Where(_ => _.EndDate != null && _.EndDate.Value.CompareTo(filter.EndDate.Value) == 0);
            }
            if (filter.Status != null)
            {
                query = query.Where(_ => _.Status == (int)filter.Status);
            }
            if (filter.CreatedBy != null)
            {
                query = query.Where(_ => _.CreatorId == filter.CreatedBy);
            }
            if (filter.Type != null)
            {
                query = query.Where(_ => _.Type == filter.Type);
            }
            if (filter.Priority != null)
            {
                query = query.Where(_ => _.Priority == filter.Priority);
            }
            if (filter.TitleOrDescription != null)
            {
                query = query.Where(_ => _.Title.Contains(filter.TitleOrDescription) || _.Description.Contains(filter.TitleOrDescription));
            }
            if (filter.OwnerId != null)
            {
                query = query.Where(_ => _.AssigneeId == filter.OwnerId);
            }
            if (filter.Code != null)
            {
                query = query.Where(_ => _.Code == filter.Code);
            }
            if (filter.IsSyncToCalendar is not null && filter.IsSyncToCalendar == true)
            {
                query = query.Where(_ => _.CalendarEvent != null && _.CalendarEvent.IsSynced);
            }
            if (filter.IsSyncToCalendar is not null && filter.IsSyncToCalendar == false)
            {
                query = query.Where(_ => _.CalendarEvent == null || !_.CalendarEvent.IsSynced);
            }
            int skip = filter.Paging.Skip;
            int take = filter.Paging.Take;
            int total = await query.CountAsync();

            if (total == 0)
            {
                return new GetWorkItemsResponse(total, new List<WorkItemDTO>());
            }
            List<WorkItem> workItems = await query.OrderByDescending(_ => _.Code).Skip(skip).Take(take).ToListAsync();

            return new GetWorkItemsResponse(total, workItems.Select(workItem => new WorkItemDTO
            {
                Id = workItem.Id,
                Code = workItem.Code,
                Title = workItem.Title,
                Description = workItem.Description,
                Type = workItem.Type,
                LastUpdaterId = workItem.LastUpdaterId,
                LastUpdaterName = workItem.LastUpdaterName,
                CreatorId = workItem.CreatorId,
                CreatorName = workItem.CreatorName,
                Status = (WorkItemStatus)workItem.Status,
                AssigneeId = workItem.AssigneeId,
                AssigneeName = workItem.AssigneeName,
                BoardId = workItem.BoardId,
                Priority = workItem.Priority,
                ParentWorkItemCode = workItem.ParentWorkItemId != workItem.Id ? workItem.ParentWorkItem.Code : null,
                ParentWorkItemId = workItem.ParentWorkItemId,
                StartDate = workItem.StartDate,
                EndDate = workItem.EndDate,
                WorkRemain = workItem.WorkRemain,
                IsSyncToCalendar = workItem.CalendarEvent is not null && workItem.CalendarEvent.IsSynced ? true : false,
                SubWorkItems = workItem.SubWorkItems
                    .Where(_ => _.ParentWorkItemId != _.Id)
                    .OrderByDescending(_ => _.Code)
                    .Select(_ => new SubWorkItem()
                    {
                        Id = _.Id,
                        Code = _.Code,
                        Title = _.Title
                    }).ToList()
            }).ToList());
        }

        /// <summary>
        /// UpdateAsync
        /// </summary>
        /// <param name="workItemDTO"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PostWorkItemResponse> UpdateAsync(PostPutWorkItemDTO workItemDTO)
        {
            WorkItem? foundWorkItem = await GetRecordByIdAsync(workItemDTO.Id);

            if (foundWorkItem is null)
            {
                return new PostWorkItemResponse(false, HttpConstants.WorkItemNotFound);
            }
            ApplicationUser? foundUpdater = await GetUserByIdAsync(workItemDTO.LastUpdaterId!);
            if (foundWorkItem.AssigneeId != workItemDTO.AssigneeId)
            {
                ApplicationUser? foundAssignee = await GetUserByIdAsync(workItemDTO.AssigneeId!);
                foundWorkItem.AssigneeId = workItemDTO.AssigneeId;
                foundWorkItem.AssigneeName = workItemDTO.AssigneeName;
                foundWorkItem.User = foundAssignee;
            }
            Guid foundParentId = Guid.Empty;
            if (workItemDTO.ParentWorkItemCode is not null)
            {
                WorkItem? foundParent = await GetByCodeAsync(workItemDTO.ParentWorkItemCode.Value);
                if (foundParent is null)
                {
                    return new PostWorkItemResponse(false, HttpConstants.ParentWorkItemNotFound);
                }
                foundParentId = foundParent.Id;
            }

            foundWorkItem.Title = workItemDTO.Title;
            foundWorkItem.Description = workItemDTO.Description;
            foundWorkItem.Type = workItemDTO.Type;
            foundWorkItem.LastUpdaterId = workItemDTO.LastUpdaterId;
            foundWorkItem.LastUpdaterName = foundUpdater?.UserName;
            foundWorkItem.Status = (int)workItemDTO.Status;
            foundWorkItem.BoardId = workItemDTO.BoardId;
            foundWorkItem.Priority = workItemDTO.Priority;
            foundWorkItem.ParentWorkItemId = workItemDTO.ParentWorkItemCode is null ?
                foundWorkItem.Id : foundParentId;
            foundWorkItem.StartDate = workItemDTO.StartDate!.Value.ToDateOnly(Helper.GetTimeZonePlus7());
            foundWorkItem.EndDate = workItemDTO.EndDate!.Value.ToDateOnly(Helper.GetTimeZonePlus7());
            foundWorkItem.WorkRemain = double.Parse(workItemDTO.WorkRemain, CultureInfo.InvariantCulture);
            foundWorkItem.UpdatedAt = DateTime.UtcNow;

            context.WorkItems.Update(foundWorkItem);
            await context.SaveChangesAsync();
            if (!string.IsNullOrEmpty(foundWorkItem.AssigneeId))
            {
                await eventPusher.Publish(new WorkItemUpdate
                {
                    ObjectId = foundWorkItem.Id.ToString(),
                    WorkItemCode = foundWorkItem.Code.ToString(),
                    WorkItemTitle = foundWorkItem.Title,
                    UpdaterName = foundUpdater!.FullName,
                    RecieverId = foundWorkItem.AssigneeId
                });
            }
            return new PostWorkItemResponse(true, HttpConstants.WorkItemSavedSuccessfully, foundWorkItem.Id);
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
        /// QueryAllNearingDues
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IQueryable<WorkItem> QueryAllNearingDues()
        {
            DateOnly utcNow = DateOnly.FromDateTime(DateTime.UtcNow);
            List<WorkItemStatus> blacklist = new List<WorkItemStatus>() { WorkItemStatus.Resolved, WorkItemStatus.Done };
            IQueryable<WorkItem> query = context.WorkItems
                .Where(_ => _.Status != (int)WorkItemStatus.Resolved &&
                       _.Status != (int)WorkItemStatus.Done &&
                      !_.IsDeleted && _.EndDate != null && _.EndDate.Value.CompareTo(utcNow) == 0);
            return query;
        }

        /// <summary>
        /// GetRecordByIdAsync
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task<WorkItem?> GetRecordByIdAsync(Guid id)
        {
            return await context.WorkItems
                .Include(_ => _.ParentWorkItem)
                .Include(_ => _.SubWorkItems)
                .Include(_ => _.CalendarEvent)
                .FirstOrDefaultAsync(_ => _.Id == id && !_.IsDeleted);
        }

        /// <summary>
        /// GetByCodeAsync
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task<WorkItem?> GetByCodeAsync(int code)
        {
            return await context.WorkItems
                .FirstOrDefaultAsync(_ => _.Code == code && !_.IsDeleted);
        }

        /// <summary>
        /// GetUserByIdAsync
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task<ApplicationUser?> GetUserByIdAsync(string id)
        {
            return await context.Users.FirstOrDefaultAsync(_ => _.Id == id);
        }

        /// <summary>
        /// MarkAsSyncToCalendar
        /// </summary>
        /// <param name="markAsSyncToCalendarDTO"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PostPutResponse> MarkAsSyncToCalendar(MarkAsSyncToCalendarDTO markAsSyncToCalendarDTO)
        {
            List<WorkItem> workItems = await context.WorkItems.Include(_ => _.CalendarEvent)
                .Where(_ => !_.IsDeleted && markAsSyncToCalendarDTO.Ids.Contains(_.Id))
                .ToListAsync();

            List<WorkItem> tasksSyncedBefore = workItems
                .Where(_ => _.CalendarEvent is not null).ToList();

            foreach (var workItem in workItems)
            {
                workItem.IsSyncToCalendar = true;
            }

            context.UpdateRange(workItems);
            await context.SaveChangesAsync();
            return new PostPutResponse(true);
        }
    }
}
