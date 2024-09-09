using Microsoft.EntityFrameworkCore;
using System.Globalization;
using WorkManagermentWeb.Application.Contracts;
using WorkManagermentWeb.Application.DTOs.Responses;
using WorkManagermentWeb.Application.Utilities;
using WorkManagermentWeb.Domain;
using WorkManagermentWeb.Domain.Entities;
using WorkManagermentWeb.Domain.Enums;
using WorkManagermentWeb.Infrastructure.Data;

namespace WorkManagermentWeb.Infrastructure.Repositories
{
    /// <summary>
    /// GanttChartRepository
    /// </summary>
    /// <param name="context"></param>
    public class GanttChartRepository(AppDBContext context) : IGanttChart
    {
        /// <summary>
        /// GetDataAsync
        /// </summary>
        /// <param name="boardIds"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<GanttChartResponse> GetDataAsync(List<Guid> boardIds, string culture)
        {
            Helper.SetCulture(culture);
            GanttChartResponse response = new();
            IQueryable<Board> query = context.Boards
                .Where(_ => !_.IsDeleted)
                .Include(_ => _.WorkItems.Where(t => !t.IsDeleted));

            if (boardIds is null && boardIds!.Count == 0)
            {
                return response;
            }
            query = query.Where(_ => boardIds.Any(id => id == _.Id));

            List<Board> foundBoards = await query.ToListAsync();

            List<string> boardOwnerIds = foundBoards.Select(_ => _.LastUpdateBy).ToList();

            foreach (Board foundBoard in foundBoards)
            {
                response.Data.Add(new GanttData()
                {
                    Id = foundBoard.Id.ToString(),
                    Title = foundBoard.Name,
                    StartDate = foundBoard.StartDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture),
                    DueDate = foundBoard.EndDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture),
                    Assignee = foundBoard.AssigneeName,
                    Type = "project"
                });
                FillTasksData(response, foundBoard.WorkItems.Where(_ => _.Id == _.ParentWorkItemId).ToList(), foundBoard.Id.ToString());
            }
            return response;
        }

        /// <summary>
        /// FillTaskData
        /// </summary>
        /// <param name="response"></param>
        /// <param name="workItems"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        private void FillTasksData(GanttChartResponse response, List<WorkItem> workItems, string parentId)
        {
            if (workItems.Count > 0)
            {
                foreach (var workItem in workItems)
                {
                    response.Data.Add(new GanttData()
                    {
                        Id = workItem.Id.ToString(),
                        Title = $"#{workItem.Code}: {workItem.Title}",
                        Assignee = workItem.AssigneeName,
                        StartDate = workItem.StartDate!.Value.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture),
                        DueDate = workItem.EndDate!.Value.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture),
                        WorkRemain = workItem.WorkRemain,
                        Progress = Helper.CalculateProcessPercent(workItem.StartDate, workItem.EndDate, (WorkItemStatus)workItem.Status),
                        Priority = workItem.Priority.GetDisplayDescription(),
                        Parent = parentId
                    });
                    response.Links.Add(new GanttLink()
                    {
                        Source = parentId,
                        Target = workItem.Id.ToString()
                    });
                    if (workItem.SubWorkItems is not null)
                    {
                        FillTasksData(response, workItem.SubWorkItems.Where(_ => _.Id != _.ParentWorkItemId).ToList(), workItem.Id.ToString());
                    }
                }
            }
        }

        /// <summary>
        /// GetUserByIdAsync
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task<ApplicationUser?> GetUserByIdAsync(string id)
        {
            return await context.Users.FirstOrDefaultAsync(_ => _.Id == id && _.ActiveStatus);
        }
    }
}
